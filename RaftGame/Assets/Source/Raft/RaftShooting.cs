using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaftShooting : MonoBehaviour {

	public int PlayerNumber = 1;
	public Rigidbody Wave;		// Wave Prefab class
	public Transform FireTransform;		// Child of Raft

	/**
	 *	Gameplay properties 
	 */
	public float CurrentLaunchForce = 15.0f;
	public float MinLaunchForce = 1.0f;

	private string FireButton;			// Input axis for launching waves
	private bool isFired;

	// Use this for initialization
	void Start () {
		FireButton = "Fire" + PlayerNumber;
	}

	// Update is called once per frame
	void Update () {

	}

	/**
	 * Activate Fired state. Spawn new wave instance, set velocities, and play sound
	 */
	private void Fire() {
		isFired = true;

		// Create an instance of the shell and store a reference to it's rigidbody.
		Rigidbody waveInstance =
			Instantiate (Wave, FireTransform.position, FireTransform.rotation) as Rigidbody;

		// Set the shell's velocity to the launch force in the fire position's forward direction.
		shellInstance.velocity = CurrentLaunchForce * FireTransform.forward; ;

		// Sound
		// Change the clip to the firing clip and play it.
		// m_ShootingAudio.clip = m_FireClip;
		// m_ShootingAudio.Play ();

		// Reset the launch force.  This is a precaution in case of missing button events.
		CurrentLaunchForce = MinLaunchForce;
	}
}
