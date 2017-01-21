using System.Collections;
using System.Collections.Generic;
using RaftGame;
using UnityEngine;

namespace RaftGame {
		
	public class GameManager : MonoBehaviour  {

		public GameObject m_RaftPrefab;
		public RaftManager[] m_Rafts;
		public static List<Player> players = new List<Player>();

		private int m_PlayerCount; 

		void Awake() {
			print ("Initializing Game Manager");
			// players = new List<Player>();
			// TODO: This should be set by MainMenu after player lobby finishes
			PlayerPrefs.SetInt ("Player Count", 2);
			PlayerPrefs.SetString ("Player1.Color", Color.red.ToString ());
		}

		// Use this for initialization
		void Start () {
			print ("Starting Game Manager");
			m_PlayerCount = PlayerPrefs.GetInt ("Player Count");
			print ("Number of players: " + m_PlayerCount);

			SpawnRafts (m_PlayerCount);
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		void ResetRafts() {
			foreach (RaftManager manager in m_Rafts) {
				manager.Reset ();
			}	
		}

		void SpawnRafts(int player_count) {		
			m_Rafts = new RaftManager[player_count];

			for (int i = 0; i < m_Rafts.Length; ++i) {
				m_Rafts[i] = new RaftManager ();

				string player_color_key = "Player" + i + ".Color";

				if (PlayerPrefs.HasKey(player_color_key)) {
					string player_color = PlayerPrefs.GetString(player_color_key); 
					print(player_color_key + " found: " + player_color);
				}
			}
		}
	}
} // end of namespace
public struct Player {
	int idVal;
	int teamVal;
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