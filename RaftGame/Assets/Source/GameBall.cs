using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBall : MonoBehaviour
{
    public delegate void OnGameBallDeath(int team);
    public event OnGameBallDeath OnBallDeath;

    public Rigidbody RigidBodyComponent;

    public void Awake()
    {
        RigidBodyComponent = GetComponent<Rigidbody>();
    }

    public virtual void Kill(int team)
    {
        if (OnBallDeath != null)
        {
            OnBallDeath(team);
        }
    }
}