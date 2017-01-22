using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaftGame {

	[System.Serializable]
	public class RaftManager {
		public Transform m_SpawnPoint;

		[HideInInspector] public Color m_PlayerColor;
		[HideInInspector] public int m_Id;
		[HideInInspector] public int m_Wins;
		[HideInInspector] public int m_Team;

		private RaftShooting m_ShootingComponent;
		[HideInInspector] private RaftMover m_MovementComponent;
		[HideInInspector] public RaftInput m_InputComponent;
		[HideInInspector] public GameObject m_Instance;
		// Canvasgameobject - used to disable world space ui

		public void Setup() {
			m_InputComponent = m_Instance.GetComponent<RaftInput> ();
			m_MovementComponent = m_Instance.GetComponent<RaftMover> ();
			m_ShootingComponent = m_Instance.GetComponent<RaftShooting> ();
		}

		public void SetPlayer(Player data) {
			m_Id = data.id;
			m_Team = data.team;
			m_InputComponent.SetOwner(data.id);
		}

		public void Reset() {
			Debug.Log ("Reset");
		}
	}

}	// end of namespace RaftGame