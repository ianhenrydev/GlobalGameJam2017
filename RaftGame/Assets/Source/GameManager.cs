﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RaftGame;
using UnityEngine;
using UnityEngine.Events;

<<<<<<< HEAD

public enum E_GAME_STATE
{
    STARTGAME,
    WAITING,
    INROUND,
    ENDGAME
}

namespace RaftGame
{
    public struct Player
    {
        public int Id;
        public int Team;
        public GameObject ParentActor;

        public Player(int id, int team)
        {
            Id = id;
            ParentActor = null;
            Team = team;
        }
    }

    public class GameManager : MonoBehaviour
    {
        //Important Component Instances
        private GameBall BallInstance;

        //Game HUD
        private int CurrentCanvas = -1; // 0 - pause. 1 - inGame. 2 - postGame.
        private int LastCanvas = 1; // n > 0
        private PauseScreenController UI_HUD_PauseGame;
        private GameScreenController UI_HUD_Game;
        private EndGameScreenController UI_HUD_EndGame;

        public static GameManager Instance;

        //Game state
        public E_GAME_STATE CurrentGameState { get; private set; }
        public bool GameIsPaused { get; private set; }

        //Object prefabs
        public GameObject m_RaftPrefab;
        public GameObject m_BallPrefab;

        public Transform[] TeamASpawns;
        public Transform[] TeamBSpawns;

        //Round start/end info
        public float RoundStartDelay = 3.0f;
        public float RoundEndDelay = 3.0f;

        //Events
        public UnityEvent OnMatchStart = new UnityEvent();
        public UnityEvent OnRoundStart = new UnityEvent();
        public UnityEvent OnRoundEnd = new UnityEvent();
        public UnityEvent OnEndMatch = new UnityEvent();
        public UnityEvent OnResetTempObjects = new UnityEvent();
        public UnityEvent OnResetGame = new UnityEvent();

        //Time containers
        private float GameTime = 0.0f;
        private float WarmupTime = 0.0f;

        //Score containers
        public int TeamScoreA { get; private set; }
        public int TeamScoreB { get; private set; }

        [HideInInspector]
        public static List<Player> Players = new List<Player>();

        public void Awake()
        {
            Instance = this;
            CurrentGameState = E_GAME_STATE.WAITING;
        }

        public void Start()
        {
            CurrentGameState = E_GAME_STATE.WAITING;
            StartCoroutine(StartMatch());
        }

        public void Update()
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
            CurrentGameState = E_GAME_STATE.STARTGAME;

            //Swap hud
            SetGameCanvas(1);

            //Spawn the ball
            BallInstance =
                GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("GameBall")).GetComponent<GameBall>();

            BallInstance.RigidBodyComponent.isKinematic = true;
            BallInstance.transform.position = new Vector3(-3.5f, 10, 0);

            //Spawn players
            //List<Transform> remainingSpawns = PlayerSpawnTransforms.ToList();
            /*
            while (remainingSpawns.Count > 0)
            {

                yield return null;
            }
            */

            WarmupTime = RoundStartDelay;

            OnMatchStart.Invoke();

            StartCoroutine(StartRound());
            yield return null;
        }

        /// <summary>
        /// Set countdown and do countdown, go InRound afterwards
        /// </summary>
        /// <returns></returns>
        private IEnumerator StartRound()
        {
            CurrentGameState = E_GAME_STATE.INROUND;

            while (WarmupTime > 0)
            {
                WarmupTime -= Time.deltaTime;
                yield return null;
            }

            //Init the ball
            BallInstance.RigidBodyComponent.isKinematic = false;
            GameTime = 5.0f;

            OnRoundStart.Invoke();
            yield return null;
        }

        /// <summary>
        /// End the round and stop timers
        /// </summary>
        /// <returns></returns>
        private IEnumerator EndRound()
        {
            CurrentGameState = E_GAME_STATE.WAITING;

            OnRoundEnd.Invoke();
            yield return null;
        }

        /// <summary>
        /// Destroy ball and show scoreboard
        /// </summary>
        /// <returns></returns>
        private IEnumerator EndMatch()
        {
            CurrentGameState = E_GAME_STATE.ENDGAME;

            //Swap hud
            SetGameCanvas(2);

            OnEndMatch.Invoke();
            yield return null;
        }

        /// <summary>
        /// Reset Ball and players
        /// </summary>
        /// <returns></returns>
        private IEnumerator ResetTemporaryObjects()
        {
            OnResetTempObjects.Invoke();
            yield return null;
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
            switch (newCanvas)
            {
                case 0:
                    if (UI_HUD_PauseGame == null)
                    {
                        UI_HUD_PauseGame =
                            PauseScreenController.Instantiate(
                                Resources.Load<PauseScreenController>("PauseGameCanvas"));
                    }
                    break;



                case 1:
                    if (UI_HUD_Game == null)
                    {
                        UI_HUD_Game =
                            GameScreenController.Instantiate(
                                Resources.Load<GameScreenController>("GameOverlayCanvas"));
                    }
                    break;



                case 2:
                    if (UI_HUD_EndGame == null)
                    {
                        UI_HUD_EndGame =
                            EndGameScreenController.Instantiate(
                                Resources.Load<EndGameScreenController>("EndGameCanvas"));
                    }
                    break;
            }

            CurrentCanvas = newCanvas;

            //Toggle on/off
            UI_HUD_PauseGame.gameObject.SetActive(CurrentCanvas == 0);
            UI_HUD_Game.gameObject.SetActive(CurrentCanvas == 1);
            UI_HUD_EndGame.gameObject.SetActive(CurrentCanvas == 2);
        }

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
            }
        }

        public static void TogglePause()
        {
            Instance.GameIsPaused = !Instance.GameIsPaused;
            if (Instance.GameIsPaused)
            {
                Instance.LastCanvas = Instance.CurrentCanvas;
                Instance.SetGameCanvas(0);
            }
            else
            {
                Instance.SetGameCanvas(Instance.LastCanvas);
            }
        }
    }
}
=======
namespace RaftGame {
		
	public struct Player {
		private int idVal, teamVal;
		private Color _color;

		public Color color {
			get {
				return _color;
			}
			set {
				_color = value;
			}
		}

		public int id {
			get {
				return idVal;
			}
			set {
				idVal = value;
			}
		}
		public int team {
			get {
				return teamVal;
			}
			set {
				teamVal = value;
			}
		}
	}

	public class GameManager : MonoBehaviour  {

		public GameObject m_RaftPrefab;				// Prefab of raft to spawn for players
		public GameObject m_BallPrefab;				// Prefab of game ball

		public float RoundStartDelay = 3.0f;		// Round start and end delays to show ui
		public float RoundEndDelay = 3.0f;

		// Global List of Player Initialization data
		// Modified by main menu controller
		[HideInInspector] public static List<Player> players = new List<Player>();

		private List<RaftManager> m_Rafts;			// Raft manager for each player
		private GameObject m_BallInstance;			// Instance of round ball

		private WaitForSeconds StartWait;			// Timers for delaying coroutines
		private WaitForSeconds EndWait;

		void Awake() {
			m_Rafts = new List<RaftManager> ();
		}

		// Use this for initialization
		void Start () {
			SpawnRafts ();

			StartWait = new WaitForSeconds (RoundStartDelay);
			EndWait = new WaitForSeconds (RoundEndDelay);

			StartCoroutine (GameLoop ());
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		private IEnumerator GameLoop() {
			yield return StartCoroutine (RoundStarting ());

			yield return StartCoroutine (RoundPlaying ());

			yield return StartCoroutine (RoundEnding ());
		}

		private IEnumerator RoundStarting() {
			print ("Round starting");

			SpawnBall ();

			yield return StartWait;
		}

		private IEnumerator RoundPlaying() {
			print ("round playing");

			// Move onto next frame
			yield return null;
		}

		private IEnumerator RoundEnding() {
			yield return EndWait;
		}

		void ResetRafts() {
			foreach (RaftManager manager in m_Rafts) {
				manager.Reset ();
			}	
		}

		private void SpawnBall() {
			Debug.Log ("Spawning ball");

			GameObject ball_spawn = GameObject.FindGameObjectWithTag ("Ball Spawn");
			if (ball_spawn) {
				// Transform incorrect
				m_BallInstance = Instantiate(m_BallPrefab, ball_spawn.transform);
			}
		}

		void SpawnRafts() {		
			Debug.Log ("Spawning rafts");
			var player_spawns = new Stack<GameObject>(GameObject.FindGameObjectsWithTag ("Player Spawn"));

			foreach (Player player in players) {
				Debug.Log ("Spawning player " + player.id);
				Transform spawn_transform = player_spawns.Pop ().transform;

				RaftManager raft = new RaftManager();
				raft.m_Instance = Instantiate (m_RaftPrefab, spawn_transform);
				raft.Setup ();
				raft.SetPlayer(player);

				m_Rafts.Add (raft);
			}
		}
	}
} // end of namespace
>>>>>>> 8ba437e0ee803ca30dbb616edcb86942cbe7d947
