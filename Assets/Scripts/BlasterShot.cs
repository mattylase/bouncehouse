using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterShot : MonoBehaviour {

	public Animator myAnimator;
	public KeyCode trigger001;


	void Start () {
				
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (trigger001)){
			myAnimator.SetTrigger ("TriggerFire");

		}

		
	}
}
