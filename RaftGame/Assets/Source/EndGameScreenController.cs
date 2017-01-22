using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreenController : MonoBehaviour {

	public Text winText;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void printScore(int team1score, int team2score) {
		string endText = "";
		string formatString = "Team {0} Wins!\nFinal Score: {1}-{2}\n";
		if (team1score > team2score) {
			endText = string.Format (formatString, 1, team1score, team2score);
		} else if (team2score > team1score) {
			endText = string.Format (formatString, 2, team2score, team1score);
		} else {
			endText = "Tie game!\nEveryone wins (or loses)\n";
		}
		endText += "Press start/enter to return to menu";
	}
}
