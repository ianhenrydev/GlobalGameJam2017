using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RaftGame
{

    public class MainMenuController : MonoBehaviour
    {

        public GameObject mainCanvas;
        public GameObject playerSelectCanvas;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Start"))
            {
                int team1count = 0;
                int team2count = 0;

                foreach (Player player in GameManager.Players)
                {
                    if (player.Team == 1)
                    {
                        team1count++;
                    }
                    else
                    {
                        team2count++;
                    }
                }

                // If both teams have at least one player then load the game round
                if (team1count > 0 && team2count > 0)
                {
                    SceneManager.LoadScene("Arena");
                }
                else
                {
                    Debug.Log("Empty team");
                }
            }
        }

        public void PlayOnClick()
        {
            mainCanvas.SetActive(false);
            playerSelectCanvas.SetActive(true);
        }
    }

}