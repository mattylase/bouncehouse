using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube : MonoBehaviour {
	void Update () {
		transform.parent.transform.rotation = Quaternion.identity;
	}

	void OnCollisionEnter(Collision col) {
		if (col.transform.tag == "Player") {
			col.transform.SetParent (transform.parent);
		}
	}

	void OnCollisionExit(Collision col) {
		if (col.transform.IsChildOf (transform.parent)) {
			col.transform.SetParent (null);
		}
	}
}
