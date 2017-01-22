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

    private void OnCollisionEnter(Collision collision)
    {
        //Play bounce audio

        //Play splash fx if water
        if (collision.gameObject.layer == 4)
        {
            print("Touched Water");
        }
        else
        {
            print("Touched Something else");
        }
        
    }
}