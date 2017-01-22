﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScreenController : MonoBehaviour {

	public Text team1text;
	public Text team2text;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void updateTeamScore(int team1, int team2) {
		team2text.text = team1.ToString ();
		team1text.text = team2.ToString ();
	}
}
