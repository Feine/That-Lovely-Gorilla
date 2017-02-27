using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudLooping : MonoBehaviour {

	public float speed =0;
	public Vector3 startPOS;


	void FixedUpdate () 
	{

		transform.position =Vector3.MoveTowards (transform.position, new Vector3 (12.65f,
			this.transform.position.y,
			this.transform.position.z),
			speed * Time.deltaTime);

		if (transform.position.x < -2.65f) {
			transform.position = new Vector3 (2.65f, this.transform.position.y, this.transform.position.z);
		}
	}
}
