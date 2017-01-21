using System.Collections;
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
	private bool team1 = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Submit" + playerNum.ToString())) {
			if (!playerJoined) {
				promptText.SetActive (false);
				joinedLayout.SetActive (true);
				playerJoined = !playerJoined;
			}
		}
		if (Input.GetButtonDown("Cancel" + playerNum.ToString())) {
			if (playerJoined) {
				promptText.SetActive (true);
				joinedLayout.SetActive (false);
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
		team1 = !team1;
		if (team1) {
			teamText.text = "Team 1";
		} else {
			teamText.text = "Team 2";
		}
	}
}
