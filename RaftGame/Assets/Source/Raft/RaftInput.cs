using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaftInput : MonoBehaviour
{
    public int OwnerId { get; private set; }

    public Vector2 InputVector
    {
        get
        {
            if (OwnerId == -1)
                return Vector2.zero;

            return new Vector2(
                Input.GetAxis("Horizontal" + OwnerId),
                Input.GetAxis("Vertical" + OwnerId));
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