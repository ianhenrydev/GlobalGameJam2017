using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RaftMover : MonoBehaviour
{
    #region Private Members
    private RaftInput InputComponent;
    private Rigidbody RigidBodyComponent;
    #endregion

    #region Public Members
    public float MovePower = 5.0f;
    #endregion

    public void Awake()
    {
        InputComponent = GetComponent<RaftInput>();
        RigidBodyComponent = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (RigidBodyComponent != null
            && InputComponent != null)
        {
            //Rotate raft to input vector direction
            Debug.Log(InputComponent.InputVector);
        }
    }

    private void FixedUpdate()
    {
        
    }
}