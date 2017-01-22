using System.Collections;
using System;
using System.Collections.Generic;
using RaftGame;
using UnityEngine;

public class RaftInput : MonoBehaviour
{
    public int OwnerId { get; private set; }
	private string FireButton;
	private string ThrustAxis;
	private string BoostButton;
	private string SteerAxis;
	// private string Boost

	// Return left right values
    public Vector3 InputVector
    {
        get
        {
            if (OwnerId == -1
                || RaftGame.GameManager.Instance == null
                 || RaftGame.GameManager.Instance.CurrentGameState != E_GAME_STATE.INROUND)
                return Vector3.zero;


            return new Vector3(
                Input.GetAxis(SteerAxis),
                0,
                -Input.GetAxis(SteerAxis));
        }
    }

    public float InputThrust
    {
        get
        {
            if (OwnerId == -1
                || RaftGame.GameManager.Instance == null
                 || RaftGame.GameManager.Instance.CurrentGameState != E_GAME_STATE.INROUND)
                return 0;

			// Clamp the thrust value between [0,1]
            return Mathf.Clamp01(-Input.GetAxis(ThrustAxis));
        }
    }

    public bool IsBoosting
    {
        get
        {
            if (OwnerId == -1
                || RaftGame.GameManager.Instance == null
                 || RaftGame.GameManager.Instance.CurrentGameState != E_GAME_STATE.INROUND)
                return false;

            return Input.GetButton(BoostButton);
        }
    }
		
	// This should be changed into an axis.
	public bool IsFiring
    {
		get {
            // This check is stupid
            //it's scalable!
            if (OwnerId == -1
                || RaftGame.GameManager.Instance == null
                 || RaftGame.GameManager.Instance.CurrentGameState != E_GAME_STATE.INROUND)
                return false;

            return Input.GetButton (FireButton);
		}
	}

    public void Awake()
    {
		SetOwner (-1);
    }

    public void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            GameManager.TogglePause();
        }
    }

    public void SetOwner(int newId)
    {
		if (newId < 0)
        {
			newId = -1;
		}

		FireButton = "Fire" + newId;
		ThrustAxis = "Thrust" + newId;
		BoostButton = "Boost" + newId;
		SteerAxis = "Steer" + newId;

		OwnerId = newId;
    }
}