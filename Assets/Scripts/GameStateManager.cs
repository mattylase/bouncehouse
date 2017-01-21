using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

    List<PlayerControl> players;

	// Use this for initialization
	void Start () {
        players = new List<PlayerControl>();
        players.AddRange(FindObjectsOfType<PlayerControl>());

        StartCoroutine(AreYouAlive());
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
	
	// Update is called once per frame
	void Update () {
		
	}
}
