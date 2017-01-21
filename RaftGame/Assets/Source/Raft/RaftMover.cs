using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class RaftMover : MonoBehaviour
{
    #region Private Members
    private RaftInput InputComponent;
    private Rigidbody RigidBodyComponent;
    private Quaternion TargetRotation = Quaternion.identity;

    private float BoostAmount = 0.0f;
    private float MoveSpeed = 0.0f;

    //Timers
    private float BoostTimer = 0.0f;

    private bool HasBoosted = false;
    #endregion

    #region Public Members
    [Header("Movement")]
    public float Speed = 600.0f;
    public float Acceleration = 100.0f;
    public float TurnSpeed = 30.0f;

    [Header("Boost Properties")] public float BoostCooldownTime = 1.0f;
    #endregion

    public void Awake()
    {
        InputComponent = GetComponent<RaftInput>();
        RigidBodyComponent = GetComponent<Rigidbody>();

        MoveSpeed = Speed;


    }

    private void Update()
    {
        if (RigidBodyComponent != null
            && InputComponent != null)
        {
            if (InputComponent.InputThrust != 0)
            {
                Thrust(InputComponent.InputThrust);
            }

            if (InputComponent.InputVector.x != 0)
            {
                Rotate(InputComponent.InputVector.x);
            }

            HandleBoost();
        }
    }

    private void Thrust(float amount)
    {
        RigidBodyComponent.AddForce(transform.forward * MoveSpeed * Time.deltaTime);
    }

    private void Rotate(float amount)
    {
        transform.Rotate(transform.up, amount * TurnSpeed * Time.deltaTime);
    }

    private void HandleBoost()
    {
        if (!HasBoosted)
        {
            if (InputComponent.IsBoosting)
            {
                //Do boost stuff here
                //....

                BoostTimer = BoostCooldownTime;
                HasBoosted = true;
            }
        }
        else
        {
            BoostTimer -= Time.deltaTime;
            if (BoostTimer <= 0)
            {
                HasBoosted = false;
            }
        }
    }
}