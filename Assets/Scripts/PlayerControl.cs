using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public int index;
	public bool isAlive;
    public void Reset()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = new Vector3(2, 10, 2);
    }

	public void IsLoser()
	{
		var playerRigidBody = GetComponent<Rigidbody>();
		playerRigidBody.constraints = RigidbodyConstraints.FreezeAll;

		var camera = GetComponentInChildren<Camera>();
		camera.clearFlags = CameraClearFlags.SolidColor;
		camera.backgroundColor = Color.red;

		GetComponent<PlayerControl>().isAlive = false;
	}

	public void IsWinner()
	{
		var playerRigidBody = GetComponent<Rigidbody>();
		playerRigidBody.constraints = RigidbodyConstraints.FreezeAll;

		var camera = GetComponentInChildren<Camera>();
		camera.clearFlags = CameraClearFlags.SolidColor;
		camera.backgroundColor = Color.green;
	}
}
