using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaftGame {
	
	public class RaftManager {
		public Transform m_SpawnPoint;

		[HideInInspector] public Color m_PlayerColor;
		[HideInInspector] public int m_Id;
		[HideInInspector] public int m_Wins;
		[HideInInspector] public int m_Team;

		private RaftShooting m_ShootingComponent;
		private RaftMover m_MovementComponent;
		[HideInInspector] public GameObject m_Instance;
		// Canvasgameobject - used to disable world space ui

		public void Setup() {

		}

		public void SetPlayer(Player data) {
			m_Id = data.Id;
			m_Team = data.Team;
		}

		public void Reset() {
			Debug.Log ("Reset");
		}
	}

}	// end of namespace RaftGame