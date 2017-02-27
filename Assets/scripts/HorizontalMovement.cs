using UnityEngine;
using System.Collections;

public class HorizontalMovement : MonoBehaviour {

	public float speed;
	

	void FixedUpdate () {
		transform.Translate (new Vector3 (speed, 0, 0));
	}
}
