using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PhysicsGun : MonoBehaviour
{
	public float maxPower = 200.0f;
	private float range;
	private float power;
	bool charging;
	Text chargeText;
	Camera cam;
	private Transform shape;

	string pushAxis;
	string pullAxis;

	void Start ()
	{
		power = 0;
		range = 0;
		cam = GetComponent<Camera> ();
		var textObj = GameObject.Find ("Charge");
		chargeText = textObj.GetComponent<Text> ();

		if (GameStateManager.joysticksCount != 0) {
			pushAxis = "FireP" + transform.parent.GetComponent<PlayerControl> ().index;
			pullAxis = "AltFireP" + transform.parent.GetComponent<PlayerControl> ().index;
		} else {
			pushAxis = "FireP0";
			pullAxis = "AltFireP0";
		}
	}

	void Update ()
	{
		chargeText.text = power.ToString ();
		if (charging && power < maxPower) {
			power += 5;
			range += 5;
		}
			
		if (GameStateManager.joysticksCount != 0 && Input.GetAxis (pushAxis) < .5f) {
			charging = true;
		} else if (Input.GetButtonDown (pushAxis)) {
			charging = true;
		}
			
		if (charging && (Input.GetAxis(pushAxis) == 0 || Input.GetButtonUp(pushAxis))) {
			Collider[] hitColliders = Physics.OverlapSphere (transform.position, Mathf.Min (range, 10));
			foreach (Collider c in hitColliders) {
				Rigidbody rb = c.GetComponent<Rigidbody> ();
				if (rb) {
					Vector3 screenPoint = cam.WorldToViewportPoint (rb.transform.position);
					bool onScreen = (screenPoint.z > 0 && screenPoint.x > .3 && screenPoint.x < .7 && screenPoint.y > .2 && screenPoint.y < .8);
					if (onScreen) {
						rb.AddForce ((rb.transform.position - transform.position) * power);
					}
				
					if (transform.IsChildOf (c.transform) && c.transform.tag == "Player") {
						rb.AddForce (transform.forward * Mathf.Min (power * power, 4000) * -1);
					}
				}
			}

			range = power = 0;
			charging = false;
		}
	}
}