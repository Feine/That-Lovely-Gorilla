using UnityEngine;
using System.Collections;

public class Shutter : MonoBehaviour {
	// Defines the camera input. Allowing the camera to be turned on, then off.
	BoxCollider2D col;
	float timer;
	void Start () {
		col = GetComponent<BoxCollider2D> (); 
	}
	

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			col.enabled = true;
	
		}
		if (col.enabled) {
			timer += Time.deltaTime;
			if (timer > .05f) {
				col.enabled = false;
				timer = 0;
			}
		}
	}
}
