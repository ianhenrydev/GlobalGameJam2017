using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectPanelController : MonoBehaviour {

	public GameObject teamLayout;
	public Text promptText;
	public Text teamText;

	private bool playerJoined = false;
	private bool team1 = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")) {
			playerJoined = !playerJoined;
			if (playerJoined) {
				promptText.text = "Press space to join...";
				teamLayout.SetActive (false);
			} else {
				promptText.text = "Player joined";
				teamLayout.SetActive (true);
			}
		}
	}

	public void onClickTeam() {
		team1 = !team1;
		if (team1) {
			teamText.text = "Team 1";
		} else {
			teamText.text = "Team 2";
		}
	}
}
