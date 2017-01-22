using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerObject;

public class GameStateManager : MonoBehaviour {

    private Object playerPrefab;
    PlayerControl player1, player2, player3, player4;
    List<GameObject> players;

	public static int joysticksCount;

    // Use this for initialization
    void Start () {
        playerPrefab = Resources.Load("Prefabs/Player");
        players = new List<GameObject>();

		joysticksCount = 0;

		foreach (string joystickName in Input.GetJoystickNames())
			if (joystickName != "" && joystickName != null && joystickName != "Object")
				joysticksCount++;

		if (joysticksCount == 0)
        {
            GameObject go = Instantiate(playerPrefab, new Vector3(2, 10, 2), Quaternion.identity) as GameObject;
            go.name = "Player 1";
            go.GetComponent<Renderer>().material.SetColor("_Color", new Color(Random.insideUnitCircle.x, Random.insideUnitCircle.x, Random.insideUnitCircle.x));
            go.GetComponent<PlayerControl>().index = 1;
            players.Add(go);
        } else
        {
			for (int i = 1; i <= joysticksCount; i++)
            {
                GameObject go = Instantiate(playerPrefab, new Vector3(2 * i, 10, 2 * i), Quaternion.identity) as GameObject;
                go.name = "Player " + i;
                go.GetComponent<Renderer>().material.SetColor("_Color", new Color(Random.insideUnitCircle.x, Random.insideUnitCircle.x, Random.insideUnitCircle.x));
                go.GetComponent<PlayerControl>().index = i;
				go.GetComponent<PlayerControl>().isAlive = true;
                players.Add(go);
            }
        }

		AdjustViewports(joysticksCount);
        StartCoroutine(AreYouAlive());
	}

    void AdjustViewports(int numPlayers)
    {
        switch (numPlayers)
        {
            case 0:
            case 1:
                players[0].GetComponentInChildren<Camera>().rect = new Rect(0, 0, 1, 1);
                break;
            case 2:
                players[0].GetComponentInChildren<Camera>().rect = new Rect(0, .5f, 1, .5f);
                players[1].GetComponentInChildren<Camera>().rect = new Rect(0, 0f, 1, .5f);
                break;
            case 3:
                players[0].GetComponentInChildren<Camera>().rect = new Rect(0, .5f, 1, .5f);
                players[1].GetComponentInChildren<Camera>().rect = new Rect(0, 0f, .5f, .5f);
                players[2].GetComponentInChildren<Camera>().rect = new Rect(.5f, 0f, .5f, .5f);
                break;
            case 4:
                players[0].GetComponentInChildren<Camera>().rect = new Rect(0, .5f, .5f, .5f);
                players[1].GetComponentInChildren<Camera>().rect = new Rect(.5f, .5f, .5f, .5f);
                players[2].GetComponentInChildren<Camera>().rect = new Rect(0f, 0f, .5f, .5f);
                players[3].GetComponentInChildren<Camera>().rect = new Rect(.5f, 0f, .5f, .5f);
                break;
        }
    }

    IEnumerator AreYouAlive()
    {
		int loserIndex = 0;
        while(true)
        {
            foreach (GameObject player in players)
            {
                if (Vector3.Distance(transform.position, player.transform.position) > 75)
                {
                    //player.GetComponent<PlayerControl>().Reset();
					player.GetComponent<PlayerControl>().IsLoser();
                }
                yield return null;
            }
			if (loserIndex == players.Count - 1 && players.Count > 1) {
				foreach (GameObject player in players) {
					if (player.GetComponent<PlayerControl>().isAlive = true) {
						player.GetComponent<PlayerControl>().IsWinner();
					};
				}
			}
            yield return new WaitForSeconds(1);
        }
    }
}
