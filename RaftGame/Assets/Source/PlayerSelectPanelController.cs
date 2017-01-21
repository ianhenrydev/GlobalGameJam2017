﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerSelectPanelController : MonoBehaviour {

	public GameObject joinedLayout;
	public GameObject promptText;
	public Text teamText;
	public int playerNum;

	private bool playerJoined = false;
	private Player player;
	// Use this for initialization
	void Start () {
		player = new Player ();
		player.id = playerNum;
		player.team = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Submit" + playerNum.ToString())) {
			if (!playerJoined) {
				promptText.SetActive (false);
				joinedLayout.SetActive (true);
				MainMenuController.activePlayers.Add (player);
				playerJoined = !playerJoined;
			}
		}
		if (Input.GetButtonDown("Cancel" + playerNum.ToString())) {
			if (playerJoined) {
				promptText.SetActive (true);
				joinedLayout.SetActive (false);
				MainMenuController.activePlayers.Remove (player);
				playerJoined = !playerJoined;
			}
		}
		if (Input.GetButtonDown("ChangeTeam" + playerNum.ToString())) {
			if (playerJoined) {
				switchTeam ();
			}
		}
	}

	public void onClickTeam() {
		switchTeam ();
	}

	private void switchTeam() {
		MainMenuController.activePlayers.Remove (player);
		if (player.team == 1) {
			player.team = 2;
		} else {
			player.team = 1;
		}
		teamText.text = "Team " + player.team.ToString ();
		MainMenuController.activePlayers.Add (player);
	}
}