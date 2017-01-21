using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaftGame {
	
	public class RaftManager : MonoBehaviour {
		public Color m_PlayerColor;
		public Transform m_SpawnPoint;

		[HideInInspector] public int m_Id;
		[HideInInspector] public int m_Wins;

		private RaftShooting m_ShootingComponent;
		private RaftMover m_MovementComponent;
		private GameObject m_Instance;
		// Canvasgameobject - used to disable world space ui

		public void Setup() {

		}

		public void Reset() {
			print ("Resetting Raft " + m_Id);
		}
	}

}	// end of namespace RaftGame