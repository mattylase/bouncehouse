using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    bool gameStarted;

    private void Start()
    {
        gameStarted = false;
    }

    // Update is called once per frame
    void Update () {
		if (!gameStarted && Input.anyKeyDown)
        {
            gameStarted = true;
            GetComponent<MenuCamRotator>().enabled = false;
            GetComponentInChildren<Camera>().gameObject.SetActive(false);
            transform.parent.GetComponent<GameStateManager>().NotifyReady();
        }
	}
}
