using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void Reset()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = new Vector3(2, 10, 2);
    }
}
