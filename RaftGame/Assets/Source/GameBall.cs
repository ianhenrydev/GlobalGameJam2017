using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBall : MonoBehaviour
{
    public delegate void OnGameBallDeath(int team);
    public event OnGameBallDeath OnBallDeath;

    public Rigidbody RigidBodyComponent;

    public ParticleSystem OnKillFX;
    public ParticleSystem OnSplashFX;

    public AudioSource OnBounceSFX;
    public AudioSource OnSplashSFX;

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

        if (OnKillFX != null)
        {
            OnKillFX.gameObject.SetActive(true);
            OnKillFX.transform.SetParent(null, true);
            OnKillFX.Play();
            GameObject.Destroy(OnKillFX.gameObject, 8);
        }
        
        GameObject.Destroy(gameObject, 0.1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Play bounce audio

        //Play splash fx if water
        if (collision.gameObject.layer == 4)
        {
            print("Touched Water");
            if (OnSplashFX != null)
            {
                OnKillFX.gameObject.SetActive(true);
                OnSplashFX.Emit(250);
            }

            if (OnSplashSFX != null)
            {
                OnSplashSFX.Play();
            }
        }
        else
        {
            if (OnBounceSFX != null)
            {
                OnBounceSFX.Play();
            }

            print("Touched Something else");
        }
    }
}