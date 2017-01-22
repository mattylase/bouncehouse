using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PhysicsGun : MonoBehaviour
{

	public float blastHeightModifier = 4.0f;
	public float maxPower = 6.0f;
	private float range;
	private float power;
	private int charging;
	Text chargeText;
	RigidbodyFPSController player;
	Transform aim;

	string pushAxis;
	string pullAxis;

	void Start ()
	{

		chargeText = GameObject.Find ("Charge").GetComponent<Text> ();
		player = GetComponentInParent<RigidbodyFPSController> ();
		aim = GameObject.Find ("AimDirection").GetComponent<Transform> ();

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
		if (charging != 0 && Mathf.Abs (power) < maxPower) {
			power += charging * maxPower / 25;
			range += maxPower / 25;
		}

		if (GameStateManager.joysticksCount != 0 && Input.GetAxis (pushAxis) < .5f) {
			charging = 1;
			Debug.Log ("left down");
		} else if (Input.GetButtonDown (pushAxis)) {
			Debug.Log ("left down");
			charging = 1;
		}

		if (GameStateManager.joysticksCount != 0 && Input.GetAxis (pullAxis) < .5f) {
			Debug.Log ("right down");
			charging = -1;
		} else if (Input.GetButtonDown (pullAxis)) {
			Debug.Log ("right down");
			charging = -1;
		}

		if (Input.GetAxis (pushAxis) == 0 || Input.GetButtonUp (pushAxis) || Input.GetAxis (pullAxis) == 0 || Input.GetButtonUp (pullAxis)) {
			Collider[] hitColliders = Physics.OverlapSphere (transform.position, range * 2f);
			foreach (Collider c in hitColliders) {
				Rigidbody rb = c.GetComponent<Rigidbody> ();
				if (rb) {
					Vector3 dir = rb.transform.position - aim.transform.position;
					Vector3 targetDir = rb.transform.position - transform.position;
					float angle = Vector3.Angle (targetDir, transform.forward);
					if (angle < 45.0f) {
						if (charging > 0 || player.grounded) {
							rb.AddForce ((rb.transform.position - transform.position) * power, ForceMode.VelocityChange);
							//rb.GetComponent<MeshRenderer> ().material.SetColor ("_Color", Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));
						}
					}

					if ((transform.IsChildOf (c.transform) && c.transform.tag == "Player" && hitColliders.Length > 2) && ((charging > 0) || ((!player.grounded && charging < 0)))) {
						if (charging > 0) {
							dir.x = Mathf.Clamp (dir.x, -0.5f, 0.5f);
							dir.z = Mathf.Clamp (dir.z, -0.5f, 0.5f);
						} 
						Debug.Log ("player moved");
						rb.AddForce (dir * power * blastHeightModifier, ForceMode.VelocityChange);
					}
				}
			}
			charging = 0;
			range = power = 0;
		}
	}
}