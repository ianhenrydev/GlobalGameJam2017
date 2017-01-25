using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public int Team;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            RaftGame.GameManager.GivePoints(Team, 1);
            other.gameObject.GetComponent<GameBall>().Kill(Team);

            print("GOALLLLLL!!!!");
        }
    }
}
