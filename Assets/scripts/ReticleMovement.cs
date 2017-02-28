using UnityEngine;
using System.Collections;

public class ReticleMovement : MonoBehaviour {
	public float speed;
	// Defines the movement of the reticle and its boundaries
	void FixedUpdate () 
	{
		// Horizontal movement and boundaries
		if(Input.GetButton("Right") && transform.position.x < 2.28f)
			transform.Translate (Input.GetAxisRaw ("Right") * speed, 0, 0);

			if (transform.position.x > 2.28f)
				transform.position = new Vector3 (2.28f, transform.position.y,-5f);

		if(Input.GetButton("Left") && transform.position.x > -2.28f)
			transform.Translate (Input.GetAxisRaw ("Left") * speed, 0, 0);

			if (transform.position.x < -2.28f)
			transform.position = new Vector3 (-2.28f, transform.position.y,-5f);


		// Vertical movement and boundaries
		if(Input.GetButton("Up") && transform.position.y < .669f)
			transform.Translate (0,Input.GetAxisRaw ("Up") * speed, 0);

		if (transform.position.y > .669f)
			transform.position = new Vector3 (transform.position.x, .669f,-5f);

		if(Input.GetButton("Down")&& transform.position.y > -.669f)
			transform.Translate (0,Input.GetAxisRaw ("Down") * speed, 0);
		
		if (transform.position.y < -.669f)
			transform.position = new Vector3 (transform.position.x, -.669f,-5f);
	}
}
