using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using RaftGame;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum E_GAME_STATE
{
    STARTGAME,
    WAITING,
    INROUND,
    ENDGAME
}

namespace RaftGame
{
    /// <summary>
    /// The struct for every player in the game.
    /// Should not be instanced directly.
    /// </summary>
    public struct Player
    {
        [DefaultValue(-1)]
        public int Id;

        [DefaultValue(-1)]
        public int Team;

        public Player(int id, int team)
        {
            Id = id;
            Team = team;
        }
    }

    public class GameManager : MonoBehaviour
    {
        #region Private Members
        //Important Component Instances
        private GameBall BallInstance;

        //Game HUD
        private int CurrentCanvas = -1; // 0 - pause. 1 - inGame. 2 - postGame.
        private int LastCanvas = 1; // n > 0

        private PauseScreenController UI_HUD_PauseGame;
        private GameScreenController UI_HUD_Game;
        private EndGameScreenController UI_HUD_EndGame;

        private List<GameObject> PlayerRafts = new List<GameObject>();
        #endregion

        #region Static Members
        public static GameManager Instance;
        public static List<Player> Players { get; private set; }
        #endregion

        #region Public Members
        //Game state
        public E_GAME_STATE CurrentGameState { get; private set; }
        public bool GameIsPaused { get; private set; }

        //Object prefabs
        public GameObject m_RaftPrefab;
        public GameObject m_BallPrefab;

        public Transform[] TeamASpawns;
        public Transform[] TeamBSpawns;

        //Round start/end info
        public float TimeForRoundStart = 3.0f;
        public float TimeForCompleteMatch = 160.0f;

        //Events
        public UnityEvent OnMatchStart = new UnityEvent();
        public UnityEvent OnRoundStart = new UnityEvent();
        public UnityEvent OnRoundEnd = new UnityEvent();
        public UnityEvent OnEndMatch = new UnityEvent();
        public UnityEvent OnResetTempObjects = new UnityEvent();
        public UnityEvent OnResetGame = new UnityEvent();

        //Time containers
        public float GameTime { get; private set; }
        public float WarmupTime { get; private set; }

        //Score containers
        public int TeamScoreA { get; private set; }
        public int TeamScoreB { get; private set; }
        #endregion

        private void Awake()
        {
            Instance = this;
            CurrentGameState = E_GAME_STATE.WAITING;
        }

        private void Start()
        {
            GameTime = 99;
            CurrentGameState = E_GAME_STATE.WAITING;
            StartCoroutine(StartMatch());
        }

        private void Update()
        {
            if (CurrentGameState == E_GAME_STATE.INROUND)
            {
                GameTime -= Time.deltaTime;
                if (GameTime <= 0.0f)
                {
                    StartCoroutine(EndMatch());
                }
            }
        }

        /// <summary>
        /// Spawn players and set countdown time
        /// </summary>
        /// <returns></returns>
        private IEnumerator StartMatch()
        {
            //Swap hud
            SetGameCanvas(1);

            //Create single player game where the player is on team 0
            //if we ran this scene only.
            if (Players == null 
                || Players.Count == 0)
            {
                Players = new List<Player>()
                {
                    new Player(0, 0)
                };
            }

            HandlePlayerSpawn();

            GameTime = TimeForCompleteMatch;
            CurrentGameState = E_GAME_STATE.STARTGAME;
            OnMatchStart.Invoke();

            StartCoroutine(StartRound());
            yield return null;
        }

        private void HandlePlayerSpawn()
        {
            if (Players[0].Id >= 0)
                StartCoroutine(SpawnRaft(TeamASpawns[0], Players[0]));

            if (Players.Count > 1 && Players[1].Id >= 0)
                StartCoroutine(SpawnRaft(TeamASpawns[1], Players[1]));

            if (Players.Count > 2 && Players[2].Id >= 0)
                StartCoroutine(SpawnRaft(TeamBSpawns[0], Players[2]));

            if (Players.Count > 3 && Players[3].Id >= 0)
                StartCoroutine(SpawnRaft(TeamBSpawns[1], Players[3]));
        }

        /// <summary>
        /// Set countdown and do countdown, go InRound afterwards
        /// </summary>
        /// <returns></returns>
        private IEnumerator StartRound()
        {
            CurrentGameState = E_GAME_STATE.WAITING;

            //Spawn the ball
            BallInstance =
                GameObject.Instantiate<GameObject>(
                    Resources.Load<GameObject>("GameBall")).GetComponent<GameBall>();

            BallInstance.OnBallDeath += OnBallDeath;

            BallInstance.RigidBodyComponent.isKinematic = true;
            BallInstance.transform.position = new Vector3(-3.5f, 10, 0);

            //Start countdown
            WarmupTime = TimeForRoundStart;
            while (WarmupTime > 0)
            {
                WarmupTime -= Time.deltaTime;
                yield return null;
            }

            BallInstance.RigidBodyComponent.isKinematic = false;
            CurrentGameState = E_GAME_STATE.INROUND;
            OnRoundStart.Invoke();
            yield return null;
        }

        /// <summary>
        /// End the round and stop timers.
        /// Plays OnGoal
        /// </summary>
        /// <returns></returns>
        private IEnumerator EndRound()
        {
            OnRoundEnd.Invoke();

            var goalTimer = 4.0f;

            while (goalTimer > 0.0f)
            {
                goalTimer -= Time.deltaTime;
                Time.timeScale = 0.6f;
                yield return null;
            }

            Time.timeScale = 1.0f;
            
            //Reset round
            ResetTemporaryObjects();

            StartCoroutine(StartRound());
            yield return null;
        }

        /// <summary>
        /// Destroy ball and show scoreboard
        /// </summary>
        /// <returns></returns>
        private IEnumerator EndMatch()
        {
            //Swap hud
            SetGameCanvas(2);
            CurrentGameState = E_GAME_STATE.ENDGAME;
            OnEndMatch.Invoke();

			UI_HUD_EndGame.printScore (TeamScoreA, TeamScoreB);
            print("SCORE: " + TeamScoreA + " to " + TeamScoreB);
            yield return null;
        }

        /// <summary>
        /// Reset Ball and players
        /// </summary>
        /// <returns></returns>
        private void ResetTemporaryObjects()
        {
            for (int i = 0; i < PlayerRafts.Count; i++)
            {
                GameObject.Destroy(PlayerRafts[i]);
            }

            HandlePlayerSpawn();

            OnResetTempObjects.Invoke();
        }

        /// <summary>
        /// Reset Score
        /// </summary>
        /// <returns></returns>
        private IEnumerator ResetGame()
        {
            TeamScoreA = 0;
            TeamScoreB = 0;

            OnResetGame.Invoke();
            yield return null;
        }

        /// <summary>
        /// Swaps out the game canvases.
        /// </summary>
        /// <param name="newCanvas"></param>
        private void SetGameCanvas(int newCanvas)
        {
            if (newCanvas >= 0)
            {
                if (UI_HUD_PauseGame == null)
                {
                    UI_HUD_PauseGame =
                        PauseScreenController.Instantiate(
                            Resources.Load<PauseScreenController>("PauseGameCanvas"));
                }

                if (UI_HUD_Game == null)
                {
                    UI_HUD_Game =
                        GameScreenController.Instantiate(
                            Resources.Load<GameScreenController>("GameOverlayCanvas"));
                }

                if (UI_HUD_EndGame == null)
                {
                    UI_HUD_EndGame =
                        EndGameScreenController.Instantiate(
                            Resources.Load<EndGameScreenController>("EndGameCanvas"));
                }

                CurrentCanvas = newCanvas;

                //Toggle on/off
                UI_HUD_PauseGame.gameObject.SetActive(CurrentCanvas == 0);
                UI_HUD_Game.gameObject.SetActive(CurrentCanvas == 1);
                UI_HUD_EndGame.gameObject.SetActive(CurrentCanvas == 2);
            }
        }

        /// <summary>
        /// Fully spawns raft for [player] then assigns it to them.
        /// </summary>
        /// <param name="spawnPoint"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        private IEnumerator SpawnRaft(Transform spawnPoint, Player player)
        {
            //TODO: Play spawn fx

            yield return new WaitForSeconds(1);

            //Spawn raft
            var newRaft = GameObject.Instantiate(Resources.Load<GameObject>("Raft"));
            if (newRaft != null)
            {
                newRaft.transform.position = spawnPoint.position;
                newRaft.transform.rotation = spawnPoint.rotation;

                //TODO: Color the raft

                //Assign raft to this player
                PlayerRafts.Add(newRaft);
            }

            yield return null;
        }

        /// <summary>
        /// Called when the ball has been scored on [team].
        /// </summary>
        private void OnBallDeath(int team)
        {
            if (team != -1)
            {
                StartCoroutine(EndRound());
                BallInstance = null;
            }
        }

        /// <summary>
        /// Gives [team] [points].
        /// </summary>
        /// <param name="team"></param>
        /// <param name="points"></param>
        public static void GivePoints(int team, int points)
        {
            if (Instance != null)
            {
                if (team == 0)
                {
                    Instance.TeamScoreA += points;
                }
                else
                {
                    Instance.TeamScoreB += points;
                }
				Instance.UI_HUD_Game.updateTeamScore (Instance.TeamScoreA, Instance.TeamScoreB);
            }
        }

        /// <summary>
        /// Pauses the game and toggles on/off the pause screen canvas.
        /// </summary>
        public static void TogglePause()
        {
            Instance.GameIsPaused = !Instance.GameIsPaused;
            if (Instance.GameIsPaused)
            {
                Instance.LastCanvas = Instance.CurrentCanvas;
                Instance.SetGameCanvas(0);

                Time.timeScale = 0;
            }
            else
            {
                Instance.SetGameCanvas(Instance.LastCanvas);
                Time.timeScale = 1;
            }
        }

        public static void AddPlayerToGame(int team)
        {
            if (Players == null)
            {
                Players = new List<Player>();
            }

            Players.Add(new RaftGame.Player(Players.Count - 1, team));
        }

        public static void RemovePlayerFromGame(int playerId)
        {
            if (Players == null)
            {
                Debug.LogError("There are no players to remove, be sure to use AddPlayerToGame first.");
                return;
            }

            var playerList = Players;
            for (int i = 0; i < playerList.Count; i++)
            {
                if (playerList[i].Id == playerId)
                {
                    playerList.RemoveAt(i);
                }
            }

            Players = playerList;
        }

        public void LeaveToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void RestartGame()
        {
            SceneManager.LoadScene("Arena");
        }
    }
}