using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RaftGame;
using System;

public class PlayerSelectPanelController : MonoBehaviour
{

    public GameObject joinedLayout;
    public GameObject promptText;
    public Text teamText;
    public int playerNum;

    // private string SubmitButton;

    private bool playerJoined = false;
    private Player player;
    // Use this for initialization
    void Start()
    {
        player = new Player();
        player.Id = playerNum;
        player.Team = 1;
    }

    // Update is called once per frame
    void Update()
    {
        string JoinButton = "Submit" + playerNum.ToString();
        if (Input.GetButtonDown(JoinButton))
        {
            if (!playerJoined)
            {
                promptText.SetActive(false);
                joinedLayout.SetActive(true);
                //RaftGame.GameManager.Players.Add(player);
                playerJoined = !playerJoined;
            }
        }
        if (Input.GetButtonDown("Cancel" + playerNum.ToString()))
        {
            if (playerJoined)
            {
                promptText.SetActive(true);
                joinedLayout.SetActive(false);
                //RaftGame.GameManager.Players.Remove(player);
                playerJoined = !playerJoined;
            }
        }
        if (Input.GetButtonDown("ChangeTeam" + playerNum.ToString()))
        {
            if (playerJoined)
            {
                switchTeam();
            }
        }
    }

    public void onClickTeam()
    {
        switchTeam();
    }

    private void switchTeam()
    {
        //RaftGame.GameManager.Players.Remove(player);
        if (player.Team == 1)
        {
            player.Team = 2;
        }
        else
        {
            player.Team = 1;
        }
        teamText.text = "Team " + player.Team.ToString();
        //RaftGame.GameManager.Players.Add(player);
    }
}
