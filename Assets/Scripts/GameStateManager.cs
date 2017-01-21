using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

    PlayerControl player1, player2, player3, player4;
    List<PlayerControl> players;


    // Use this for initialization
    void Start () {
        player1 = GameObject.Find("Player 1").GetComponent<PlayerControl>();
        player2 = GameObject.Find("Player 2").GetComponent<PlayerControl>();
        player3 = GameObject.Find("Player 3").GetComponent<PlayerControl>();
        player4 = GameObject.Find("Player 4").GetComponent<PlayerControl>();
        players = new List<PlayerControl>();

        if (player1 != null)
        {
            players.Add(player1);
            player1.gameObject.SetActive(true);
        }
        if (player2 != null)
        {
            players.Add(player2);
            player2.gameObject.SetActive(true);
        }
        if (player3 != null)
        {
            players.Add(player3);
            player3.gameObject.SetActive(true);
        }
        if (player4 != null)
        {
            players.Add(player4);
            player4.gameObject.SetActive(true);
        }

        AdjustViewports(Input.GetJoystickNames().Length);

        StartCoroutine(AreYouAlive());
	}

    void AdjustViewports(int numPlayers)
    {
        switch (numPlayers)
        {
            case 0:
            case 1:
                player1.GetComponentInChildren<Camera>().rect = new Rect(0, 0, 1, 1);
                break;
            case 2:
                player1.GetComponentInChildren<Camera>().rect = new Rect(0, .5f, 1, .5f);
                player2.GetComponentInChildren<Camera>().rect = new Rect(0, 0f, 1, .5f);
                break;
            case 3:
                player1.GetComponentInChildren<Camera>().rect = new Rect(0, .5f, 1, .5f);
                player2.GetComponentInChildren<Camera>().rect = new Rect(0, 0f, .5f, .5f);
                player3.GetComponentInChildren<Camera>().rect = new Rect(.5f, 0f, .5f, .5f);
                break;
            case 4:
                player1.GetComponentInChildren<Camera>().rect = new Rect(0, .5f, .5f, .5f);
                player2.GetComponentInChildren<Camera>().rect = new Rect(.5f, .5f, .5f, .5f);
                player3.GetComponentInChildren<Camera>().rect = new Rect(0f, 0f, .5f, .5f);
                player4.GetComponentInChildren<Camera>().rect = new Rect(.5f, 0f, .5f, .5f);
                break;
        }
    }

    IEnumerator AreYouAlive()
    {
        while(true)
        {
            foreach (PlayerControl player in players)
            {
                if (Vector3.Distance(transform.position, player.transform.position) > 75)
                {
                    player.Reset();
                }
                yield return null;
            }
            yield return new WaitForSeconds(1);
        }
    }

}
