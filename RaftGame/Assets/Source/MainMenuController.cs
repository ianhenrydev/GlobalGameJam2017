using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {

	public GameObject mainCanvas;
	public GameObject playerSelectCanvas;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayOnClick(){
		mainCanvas.SetActive (false);
		playerSelectCanvas.SetActive (true);
	}
}
