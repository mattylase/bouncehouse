using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class push : MonoBehaviour
{
	public float range;
	public float power;
	bool charging;
	Text chargeText;
	Camera cam;
	private Transform shape;

	void Start ()
	{
		power = 0;
		range = 0;
		cam = GetComponent<Camera> ();
		GameObject textObj;
		textObj = GameObject.Find ("Charge");
		chargeText = textObj.GetComponent<Text> ();
	}

	void Update ()
	{
		chargeText.text = power.ToString ();
		if (charging && power < 200) {
			power += 5;
			range += 5;
		}
		if (Input.GetButtonDown ("Fire1")) {
			charging = true;
		}
		if (Input.GetButtonUp ("Fire1")) {
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