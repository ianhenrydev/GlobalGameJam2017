using System.Collections;
using System.Collections.Generic;
using RaftGame.Wave;
using UnityEngine;

namespace RaftGame
{
    public class RaftShooting : MonoBehaviour
    {
        private RaftInput RaftInputcomponent;
        private bool HasFired = false;
        private float CooldownTimer = 0.0f;

        public Rigidbody WavePrefab;

        public float PushStrength = 3.0f;
        public float CooldownTime = 1.0f;
        public float SpawnDistance = 3.0f;

        private void Awake()
        {
            RaftInputcomponent = GetComponent<RaftInput>();
        }

        private void Update()
        {
            //Needs to check for game state
            if (RaftInputcomponent != null)
            {
                if (!HasFired)
                {
                    if (RaftInputcomponent.IsFiring)
                    {
                        Fire();
                    }
                }
                else
                {
                    CooldownTimer -= Time.deltaTime;
                    if (CooldownTimer <= 0)
                    {
                        HasFired = false;
                    }
                }
            }
        }

        private void Fire()
        {
            if (WavePrefab != null)
            {
                Rigidbody waveInst = Resources.Load<Rigidbody>("ShootWave");
                if (waveInst != null)
                {
                    Vector3 waveSpawnPosition = transform.position + (transform.forward * SpawnDistance);

                    waveInst =
                        GameObject.Instantiate(
                            waveInst.gameObject,
                            waveSpawnPosition,
                            transform.rotation)
                                .GetComponent<Rigidbody>();

                    waveInst.velocity = transform.forward * PushStrength;

                    WorldWaveController.SpawnWave(new Vector4(transform.position.x, transform.position.z, Vector3.Angle(transform.forward, Vector3.forward)));

                    CooldownTimer = CooldownTime;
                    HasFired = true;
                }
            }
        }
    }
}