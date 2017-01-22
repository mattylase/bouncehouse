using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhysicsGun2 : MonoBehaviour {

	public GameObject bubbleGun;
	public GameObject waveBlast;
	public float maxDistance = 75;
	public float maxPower = 20;
	public int chargeFrames = 60;
	private int charging = 0;
	private Camera cam;
	private float power = 0f;
	private float distance = 0f;
	private bool shooting = false;
	private float dir = 0.0f;
	private float[] rayGridY = new float[] {0.60f, 0.59f, 0.58f, 0.57f, 0.56f, 0.55f, 0.54f, 0.53f, 0.52f, 0.51f, 0.50f, 0.49f, 0.48f, 0.47f, 0.46f, 0.45f, 0.44f, 0.43f, 0.42f, 0.41f, 0.40f};
	private float[] rayGridX = new float[] {0.43f, 0.45f, 0.47f, 0.48f, 0.49f, 0.50f, 0.51f, 0.52f, 0.53f, 0.55f, 0.57f};
	private string pushAxis;
	private string pullAxis;
	private Color col;
	public ParticleSystem chargeEffect;
	public GameObject chargeShot;

	void Start ()
	{
		chargeShot = transform.FindChild ("ChargeBall").gameObject;
		chargeEffect = transform.GetComponentInChildren<ParticleSystem> ();
		var em = chargeEffect.emission;
		bubbleGun = transform.FindChild ("BubbleBlaster").gameObject;
		cam = GetComponent<Camera> ();
		col = GetComponentInChildren<Light> ().color;
		chargeEffect.startColor = col;
		em.rate = 0;
		chargeShot.GetComponent<Renderer>().material.SetColor("_EmissionColor", col);
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
		//DebugVision ();
		var em = chargeEffect.emission;
		em.rate = 0;
		if (charging == 0) {
			if (GameStateManager.joysticksCount != 0 && Input.GetAxis (pushAxis) < -.5f) {
				charging = 1;
			} else if (Input.GetButtonDown (pushAxis)) {
				charging = 1;
			} else if (GameStateManager.joysticksCount != 0 && Input.GetAxis (pullAxis) > .5f) {
				charging = -1;
			} else if (Input.GetButtonDown (pullAxis)) {
				charging = -1;
			}
		} else if(Mathf.Abs (power) < maxPower){
			float scale = 0.3f * (power / maxPower);
			chargeEffect.startSpeed = -10 * power/maxPower;
			power += charging * maxPower / chargeFrames;
			distance += maxDistance / chargeFrames;
			em.rate = power * 10;
			if(charging > 0)
			chargeShot.transform.localScale = new Vector3 (scale, scale, scale);
		}
		if (((Input.GetAxis (pushAxis) == 0 || Input.GetButtonUp (pushAxis)) && charging > 0) || ((Input.GetAxis (pullAxis) == 0 || Input.GetButtonUp (pullAxis)) && charging < 0)) {
			shooting = true;
			dir = charging;
			charging = 0;
		}
	}

	void FixedUpdate() {
		if (shooting) {
			VisionCheck ();
			shooting = false;
			power = 0;
			distance = 0;
		}
	}

	void VisionCheck(){
		chargeShot.transform.localScale = new Vector3 (0, 0, 0);
		if (dir > 0) {
			List<Rigidbody> bodies = new List<Rigidbody> ();
			foreach (float y in rayGridY) {
				foreach (float x in rayGridX) {
					Ray ray = cam.ViewportPointToRay (new Vector3 (x, y, 0));
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit, distance)) {
						GameObject wave = Instantiate (waveBlast) as GameObject;
						wave.transform.parent = transform;
						LineRenderer lr = wave.GetComponent<LineRenderer> ();
						lr.material = new Material (Shader.Find ("Particles/Additive"));
						lr.startColor = col;
						lr.startWidth = 0.2f;
						lr.endWidth = 0.001f;
						lr.endColor = Color.clear;
						lr.numPositions = 2;
						lr.SetPosition (0, bubbleGun.transform.position);
						lr.SetPosition (1, hit.point);
						Rigidbody rb = hit.collider.GetComponent<Rigidbody> ();
						if (rb && !bodies.Contains(rb)) {
							float hitDist = Vector3.Distance (ray.origin, hit.point);
							bodies.Add (rb);
							rb.AddForce (ray.direction.normalized * power);
							if (hitDist < 10) {
								Rigidbody p = GetComponentInParent<Rigidbody> ();
								p.AddForce (ray.direction.normalized * -0.5f * power);
							}
						}
					}
				}
			}
			bodies.Clear ();
		} else {
			Ray ray = cam.ViewportPointToRay (new Vector3 (.5f, .5f, 0));
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, distance * 2)) {
				GameObject wave = Instantiate (waveBlast) as GameObject;
				wave.transform.parent = transform;
				LineRenderer lr = wave.GetComponent<LineRenderer> ();
				lr.material = new Material (Shader.Find ("Particles/Additive"));
				lr.startColor = col;
				lr.startWidth = 0.2f;
				lr.endWidth = 0.001f;
				lr.endColor = Color.clear;
				lr.numPositions = 2;
				lr.SetPosition (0, bubbleGun.transform.position);
				lr.SetPosition (1, hit.point);
				Rigidbody p = GetComponentInParent<Rigidbody> ();
				p.AddForce (ray.direction.normalized * power * -1.5f);
			}
		}

	}

	void DebugVision() {
			foreach (float y in rayGridY) {
				foreach (float x in rayGridX) {
					Ray ray = cam.ViewportPointToRay (new Vector3 (x, y, 0));
				Debug.DrawRay (ray.origin, ray.direction * maxDistance);
				}
			}
	}

}
