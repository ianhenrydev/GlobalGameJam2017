using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaftGame {
	
	public class RaftShooting : MonoBehaviour {

		// Sibling Scripts
		private RaftInput InputComponent;
		private Rigidbody RigidbodyComponent;

		// Wave Prefab to shoot
		public Rigidbody Wave;		// Wave Prefab class
		private Transform FireTransform;		// Child of Raft

		/**
		 *	Gameplay properties 
		 */
		#region Public Members
		public float MinLaunchForce = 1.0f;			// Minimum launching force. Often the start value
		public float MaxLaunchForce = 15.0f;		// Maximum launching force. If reached, fires the wave

		public float ChargeTime = 0.75f;			// Duration to (fully?) charge up a shot

		public float MaxWaveLifetime = 4.0f;		// TODO: Playtest different values. Do charged up shots live longer?
		public float MinWaveLifetime = 1.0f;
		#endregion

		#region Private Members
		private float CurrentLaunchForce;			
		private float ChargeSpeed;

		private bool isFired;						// State management
			
		private float WaveLifetime;				
		#endregion

		void Awake() {
			InputComponent = GetComponent<RaftInput> ();
			RigidbodyComponent = GetComponent<Rigidbody> ();
		}

		private void OnEnable() {
			CurrentLaunchForce = MinLaunchForce;
		}

		// Use this for initialization
		void Start () {
			ChargeSpeed = (MaxLaunchForce - MinLaunchForce) / ChargeTime;
		}

		// Update is called once per frame
		void Update () {
			// If max force has been reached and have not fired yet
			if (CurrentLaunchForce >= MaxLaunchForce && !isFired) {
				// Set the launch force to max value and fire
				CurrentLaunchForce = MaxLaunchForce;
				Fire ();

			} else if (InputComponent.IsFiring) {
				// Reset the fire state
				isFired = false;
				CurrentLaunchForce = MinLaunchForce;
			} else if (InputComponent.IsFiring && !isFired) {
				// Charging the shots
				CurrentLaunchForce += ChargeSpeed * Time.deltaTime;

				// Aimslider value change
			} else if (InputComponent.IsFiring && !isFired) {
				Fire ();
			}
		}

		/**
		 * Activate Fired state. Spawn new wave instance, set velocities, and play sound
		 */
		private void Fire() {
			print ("Firing weapon!");

			isFired = true;

			// Create an instance of the shell and store a reference to it's rigidbody.
			Rigidbody waveInstance =
				// QUESTION: Where is transform getting set
				// Instantiate (Wave, FireTransform.position, FireTransform.rotation) as Rigidbody;
				Instantiate(Wave, transform.position, transform.rotation) as Rigidbody;

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