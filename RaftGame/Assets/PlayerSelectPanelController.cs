using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectPanelController : MonoBehaviour {

	public Text promptText;
	private bool playerJoined = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")) {
			playerJoined = !playerJoined;
			if (playerJoined)
				promptText.text = "Press space to join...";
			else
				promptText.text = "Player joined";
		}
	}
}
