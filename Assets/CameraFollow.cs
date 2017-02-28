using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public float edgePadding;
	// speed should be the same as the reticles speed;
	float speed = 0.02f;
	Vector3 reticlePos;

	void Start() {
		reticlePos = Camera.main.WorldToViewportPoint(GameObject.Find ("RETICLE").transform.position);	
	}
	void Update () {
		reticlePos = Camera.main.WorldToViewportPoint(GameObject.Find ("RETICLE").transform.position);	
	}

	void FixedUpdate() {
		if (reticlePos.x > 0.5+edgePadding && this.transform.position.x < 1.0565f) 
			transform.Translate (speed, 0, 0);
		
		if (reticlePos.x < 0.5-edgePadding && this.transform.position.x > -1.0565f) 
			transform.Translate (-speed, 0, 0);

		if (this.transform.position.x > 1.0565f)
			transform.position = new Vector3 (1.0565f, 0, -10);

		if (this.transform.position.x < -1.0565f)
			transform.position = new Vector3 (-1.0565f, 0, -10);
	}
}
