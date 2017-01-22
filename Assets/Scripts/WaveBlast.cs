using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBlast : MonoBehaviour {
	public float speed = 1;

	private LineRenderer lr;
	private float startTime;
	private float distance;
	private Vector3 startPoint;
	private Vector3 endPoint;

	void Start() {
		lr = transform.GetComponent<LineRenderer> ();
		startPoint = lr.GetPosition (0);
		endPoint = lr.GetPosition (1);
		distance = Vector3.Distance (startPoint, endPoint);
		startTime = Time.time;
		}

	void Update () {
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / distance;
		lr.SetPosition(0, Vector3.Lerp(startPoint, endPoint, fracJourney));
		if (fracJourney >= 0.9f) {
			Destroy (gameObject);
		}
	}
}
