using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public float edgePadding;
	// speed should be the same as the reticles speed;
	float speed = 0.02f;
	public float speedVariable;
	Vector3 reticlePos;

	void Start() {
		reticlePos = Camera.main.WorldToViewportPoint(GameObject.Find ("RETICLE").transform.position);	
	}
	void Update () {
		reticlePos = Camera.main.WorldToViewportPoint(GameObject.Find ("RETICLE").transform.position);
	}

	void FixedUpdate() {
		if (reticlePos.x > 0.5 + edgePadding && this.transform.position.x < 1.065f) {
			transform.Translate (speed * speedVariable, 0, 0);
			SpeedUp ();
		} else if (reticlePos.x < 0.5 - edgePadding && this.transform.position.x > -1.065f) {
			transform.Translate (-speed * speedVariable, 0, 0);
			SpeedUp ();
		} else
			speedVariable = 0;

		if (this.transform.position.x > 1.065f)
			transform.position = new Vector3 (1.065f, 0, -10);

		if (this.transform.position.x < -1.065f)
			transform.position = new Vector3 (-1.065f, 0, -10);
	}

	void SpeedUp(){
		if (reticlePos.x > 0.5 + edgePadding || reticlePos.x < 0.5 - edgePadding)
			speedVariable = 0.6f;
		if (reticlePos.x > 0.6 + edgePadding || reticlePos.x < 0.4 - edgePadding)
			speedVariable = 0.8f;
		if (reticlePos.x > 0.65 + edgePadding || reticlePos.x < 0.35 - edgePadding)
			speedVariable =1f;
	}
}
