using System.Collections;
using System.Collections.Generic;
using RaftGame;
using UnityEngine;

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