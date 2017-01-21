using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RaftInput : MonoBehaviour
{
    public int OwnerId { get; private set; }

    public Vector3 InputVector
    {
        get
        {
            if (OwnerId == -1)
                return Vector3.zero;

            return new Vector3(
                Input.GetAxis("Steer" + OwnerId.ToString()),
                0,
                -Input.GetAxis("Steer" + OwnerId.ToString()));
        }
    }

    public float InputThrust
    {
        get
        {
            if (OwnerId == -1)
                return 0;

            return Input.GetAxis("Thrust" + OwnerId.ToString());
        }
    }

    public bool IsBoosting
    {
        get
        {
            if (OwnerId == -1)
                return false;

            return Input.GetButton("Boost" + OwnerId.ToString());
        }
    }

    public void Awake()
    {
        OwnerId = 0;
    }

    public void Update()
    {

    }

    public void SetOwner(int newId)
    {
        OwnerId = newId;
    }
}