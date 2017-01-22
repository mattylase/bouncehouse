using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamRotator : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 2 * Time.deltaTime, 0);
        GetComponentInChildren<Camera>().transform.LookAt(transform.position);
	}
}
