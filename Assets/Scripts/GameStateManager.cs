using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour {
	
	private static readonly Vector3[] startingLocations = new Vector3[] 
	{
		new Vector3(MapGenerator.Size.x / 2, 10, MapGenerator.Size.y / 2),
		new Vector3(MapGenerator.Size.x / 2, 10, MapGenerator.Size.y / 2 * -1),
		new Vector3(MapGenerator.Size.x / 2 * -1, 10, MapGenerator.Size.y / 2),
		new Vector3(MapGenerator.Size.x / 2 * -1, 10, MapGenerator.Size.y / 2 * -1)
	};
    private Object playerPrefab;
    PlayerControl player1, player2, player3, player4;
    List<GameObject> players;
	private static int playerStartingLives = 3;
	private Color[] colors = {Color.blue, Color.red, Color.green, Color.magenta};
	public static int joysticksCount;

    // Use this for initialization
    void Start () {
        playerPrefab = Resources.Load("Prefabs/Player");
        players = new List<GameObject>();
        GetComponent<MapGenerator>().Generate();
    }

    public void NotifyReady()
    {
        LoadPlayers();
    }

    private void LoadPlayers()
    {
        joysticksCount = 0;

        foreach (string joystickName in Input.GetJoystickNames())
           if (joystickName != "" && joystickName != null && joystickName != "Object")
			joysticksCount++;

        if (joysticksCount == 0)
        {
			GameObject go = Instantiate(playerPrefab, startingLocations[0], Quaternion.identity) as GameObject;
            go.name = "Player 1";
            go.GetComponent<Renderer>().material.SetColor("_Color", colors[0]);
			go.GetComponentInChildren<Light>().color = colors[0];
            go.GetComponent<PlayerControl>().index = 1;
			go.GetComponent<PlayerControl> ().isAlive = true;
			go.GetComponent<PlayerControl> ().lives = playerStartingLives;
            players.Add(go);
        }
        else
        {
            for (int i = 1; i <= joysticksCount; i++)
            {
				GameObject go = Instantiate(playerPrefab, startingLocations[i-1], Quaternion.identity) as GameObject;
                go.name = "Player " + i;
				go.GetComponent<Renderer>().material.SetColor("_Color", colors[i-1]);
				go.GetComponentInChildren<Light>().color = colors[i-1];
                go.GetComponent<PlayerControl>().index = i;
				go.GetComponent<PlayerControl>().isAlive = true;
				go.GetComponent<PlayerControl> ().lives = playerStartingLives;
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
        while(true)
        {
            foreach (GameObject player in players)
            {
				var p = player.GetComponent<PlayerControl>();
                if (player.transform.position.y < -75)
                {
					if (p.lives > 0) 
					{
						p.lives--;
					}

					if (p.lives > 0) 
					{
						p.Reset();
					}

					if (p.lives == 0) {
						p.IsLoser();
					}
				}
                yield return null;
            }
			var remainingPlayers = players.Where (x => x.GetComponent<PlayerControl>().isAlive == true).Count();
			if ( remainingPlayers == 1 && players.Count > 1) 
			{
				foreach (GameObject player in players) {
					if (player.GetComponent<PlayerControl>().isAlive == true) 
					{
						player.GetComponent<PlayerControl>().IsWinner();
						yield return new WaitForSeconds(5);
						SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
					};
				}
			}
            yield return new WaitForSeconds(1);
        }
    }
}
