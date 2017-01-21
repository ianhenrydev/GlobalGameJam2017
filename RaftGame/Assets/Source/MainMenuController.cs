using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
public class MainMenuController : MonoBehaviour {

	public GameObject mainCanvas;
	public GameObject playerSelectCanvas;
	public static List<Player> activePlayers;
	// Use this for initialization
	void Start () {
		activePlayers = new List<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Start")) {
			int team1count = 0;
			int team2count = 0;
			foreach (Player player in activePlayers) {
				if (player.team == 1)
					team1count++;
				else
					team2count++;
			}
			if (team1count > 0 && team2count > 0)
				Debug.Log ("Good to go");
			else
				Debug.Log ("Empty team");
		}
	}

	public void PlayOnClick(){
		mainCanvas.SetActive (false);
		playerSelectCanvas.SetActive (true);
	}
}
