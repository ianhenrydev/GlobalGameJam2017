using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class RaftMover : MonoBehaviour
{
    private const float MOVE_SPEED = 600.0f;
    private const float BOOST_MOVE_SPEED = 1300.0f;

    #region Private Members
    private RaftInput InputComponent;
    private Rigidbody RigidBodyComponent;
    private Quaternion TargetRotation = Quaternion.identity;

    private float BoostAmount = 0.0f;
    private float MoveSpeed = 0.0f;
    #endregion

    #region Public Members
    public float MaxBoostAmount = 100;
    public float TurnSpeed = 30.0f;
    public float BoostDecaySpeed = 1.0f;
    #endregion

    public void Awake()
    {
        InputComponent = GetComponent<RaftInput>();
        RigidBodyComponent = GetComponent<Rigidbody>();

        MoveSpeed = MOVE_SPEED;
    }

    private void Update()
    {
        if (RigidBodyComponent != null
            && InputComponent != null)
        {
            if (InputComponent.InputVector != Vector3.zero)
            {
                //Rotate raft to input vector direction
                Vector3 relativePos = (transform.position + InputComponent.InputVector) - transform.position;
                TargetRotation = Quaternion.Slerp(TargetRotation,
                    Quaternion.LookRotation(relativePos), Time.deltaTime * TurnSpeed);

                transform.rotation = TargetRotation;
            }

            print(InputComponent.InputThrust);
        }

        //Handle boost
        if (InputComponent.IsBoosting)
        {
            Boost();
        }
        else if (MoveSpeed == BOOST_MOVE_SPEED)
        {
            EndBoost();
        }
    }

    private void FixedUpdate()
    {
        //Move raft
        RigidBodyComponent.AddForce(
            new Vector3(
                InputComponent.InputVector.x,
                0,
                InputComponent.InputVector.z) * Time.deltaTime * MoveSpeed);
    }

    private void Boost()
    {
        if (BoostAmount > 0)
        {
            //Turn on boost fx
            MoveSpeed = BOOST_MOVE_SPEED;

            //Decay boost
            BoostAmount -= BoostDecaySpeed * Time.deltaTime;
        }
        else
        {
            EndBoost();
        }
    }

    private void EndBoost()
    {
        //Turn off boost particle fx
        MoveSpeed = MOVE_SPEED;
    }
}