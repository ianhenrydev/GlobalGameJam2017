using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaftGame {
	
	public class RaftShooting : MonoBehaviour {

		public int PlayerNumber = 1;
		public Rigidbody Wave;		// Wave Prefab class
		public Transform FireTransform;		// Child of Raft

		/**
		 *	Gameplay properties 
		 */
		// Constants
		public float MinLaunchForce = 1.0f;
		public float MaxLaunchForce = 15.0f;
		public float MaxChargeTime = 0.75f;

		public float MaxWaveLifetime = 4.0f;
		public float MinWaveLifetime = 1.0f;

		public float CurrentLaunchForce;
		public float ChargeSpeed;

		private string FireButton;			// Input axis for launching waves
		private bool isFired;

		private float WaveLifetime;

		private void OnEnable() {
			CurrentLaunchForce = MinLaunchForce;
		}

		void Awake() {

		}

		// Use this for initialization
		void Start () {
			FireButton = "Fire" + PlayerNumber;

			ChargeSpeed = (MaxLaunchForce - MinLaunchForce) / MaxChargeTime;
		}

		// Update is called once per frame
		void Update () {
			// If max force has been reached and have not fired yet
			if (CurrentLaunchForce >= MaxLaunchForce && !isFired) {
				// Set the launch force to max value and fire
				CurrentLaunchForce = MaxLaunchForce;
				Fire ();

			} else if (Input.GetButtonDown (FireButton)) {
				// Reset the fire state
				isFired = false;
				CurrentLaunchForce = MinLaunchForce;
			} else if (Input.GetButton (FireButton) && !isFired) {
				// Charging the shots
				CurrentLaunchForce += ChargeSpeed * Time.deltaTime;

				// Aimslider value change
			} else if (Input.GetButtonUp(FireButton) && !isFired) {
				Fire ();
			}
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
			waveInstance.velocity = CurrentLaunchForce * FireTransform.forward; ;

			// Sound
			// Change the clip to the firing clip and play it.
			// m_ShootingAudio.clip = m_FireClip;
			// m_ShootingAudio.Play ();

			// Reset the launch force.  This is a precaution in case of missing button events.
			CurrentLaunchForce = MinLaunchForce;
		}
	}

}