using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public ParticleSystem[] WaveFX;

    public float BallHitPower = 2.0f;
    public float PlayerHitPower = 3.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.isStatic)
        {

        }
        else if (collision.gameObject.tag == "Player")
        {
            //Hurt other player
        }

        GameObject.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rbComponent = GetComponent<Rigidbody>();
        Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();

        if (rbComponent != null && otherRb != null)
        {
            switch (other.gameObject.layer)
            {
                case 8:
                    otherRb.AddForce(rbComponent.velocity * BallHitPower);
                    break;

                case 9:
                    otherRb.AddForce(rbComponent.velocity * PlayerHitPower, ForceMode.VelocityChange);
                    break;
            }
        }

        if (WaveFX != null)
        {
            for (int i = 0; i < WaveFX.Length; i++)
            {
                WaveFX[i].transform.SetParent(null, true);
                WaveFX[i].Stop(true);
                GameObject.Destroy(WaveFX[i].gameObject, 3);
            }
        }

        GameObject.Destroy(gameObject, 0.1f);
    }
}