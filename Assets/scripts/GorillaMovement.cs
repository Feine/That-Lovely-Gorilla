using UnityEngine;
using System.Collections;


public class GorillaMovement : MonoBehaviour {
    public float speed;
	public float timer, switchPace;
	public int GorillaBehaviour = 1, lastBehaviour;
	Animator an;
	BoxCollider2D points;
	void Start()
	{
		an = GetComponent<Animator> ();
		points = GetComponent<BoxCollider2D	> ();
		points.enabled = false;
	}

	void Update()
	{
		if (Input.GetKey (KeyCode.Q)) {
			GorillaBehaviour = 3;
			timer = 0;
		}
	}

	void FixedUpdate () 
	{
		switch (GorillaBehaviour) 
		{
		// Gorillas walking movement.
		case 1:
			transform.position = Vector3.MoveTowards (transform.position, (new Vector3 (movementWaypoint, transform.position.y)), speed * Time.deltaTime);
			// sets the animator to the walking animation, and flips the sprite if necessary.
			ResetAnimator ();
			an.SetBool ("walking", true);
			if (transform.position.x == movementWaypoint)
				BehaviourUpdate ();
			break;
		// Gorilla taking a break (Standing)
		case 2:
			ResetAnimator ();
					timer += Time.deltaTime;
					if (timer > switchPace) 
						BehaviourUpdate ();
					
			break;
		// Regular Gorilla Chance
		case 3:
			ResetAnimator ();
			an.SetBool ("posing", true);
			points.enabled = true;
			RegularGorillaChance ();

			timer += Time.deltaTime;
			if (timer > 5) 
				BehaviourUpdate ();
			break;
		}
	}

	float movementWaypoint;

	void BehaviourUpdate()
	{
		// Keeps track of what the last used behaviour is. Used for determining linking behaviours, such as standing to sitting.
		lastBehaviour = GorillaBehaviour;

		GorillaBehaviour = Random.Range (1, 3);
			
		// Creates a waypoint for the gorilla to travel to.
		movementWaypoint = Random.Range (-1.205f, 1.206f);

		if (GorillaBehaviour == 1) 
			WalkingBehaviour ();	
	
		if (GorillaBehaviour == 2) 
			IdleBehaviour ();
	}

	void WalkingBehaviour()
	{
		idleCounter = 0;
		// flips the gorillas sprite based on where the waypoint is and whether it will be moving or not. Also chooses a random walking animation
		if (movementWaypoint < transform.position.x)
			transform.localScale = new Vector3 (-1f, transform.localScale.y);
		else
			transform.localScale = new Vector3 (1f, transform.localScale.y);
		// picks an animation to be used for the sub machine
		an.SetInteger ("Picker", Random.Range (1, 3));
	}

	int idleCounter, idleDicider;
	bool sitting;
	/* picker options for idling:
	 * 101 = face camera
	 * 102 = chest pound
	 */

	void IdleBehaviour()
	{
		// keeps track of how many times in a row the gorilla has idled, and plays an animation based upon it.
			idleCounter++;
		// Sets how long the gorilla idles.
		switchPace = Random.Range (2, 6);
			// Looks around or decides to sit
		if (idleCounter == 2) 
		{
			idleDicider = Random.Range (1, 4);
			if (idleDicider == 1)
				an.SetTrigger ("Idle Look Around");
			else if (idleDicider == 2)
				an.SetInteger ("Picker", 101);
			else
				an.SetBool ("sitting", true);
		}
			// Chest Pound
			if (idleCounter == 3 && !sitting) 
			{
				an.SetInteger ("Picker", 102);
				switchPace = 1.5f;
			}
			// chest pounds then forces the gorilla to move. could perhaps be modified to allow the gorilla to stay still afterwards or give it the chance to
			if (idleCounter > 3) 
			{
				GorillaBehaviour = 1;
				WalkingBehaviour ();
			}
		timer = 0;
	}

	void RegularGorillaChance ()
	{
		
	}
		
	//sets all animations off and defaults to an idle animation
	void ResetAnimator()
	{
		if(GorillaBehaviour != 3)
		points.enabled = false;
		an.SetBool ("sitting", false);
		an.SetBool ("walking", false);
		an.SetBool ("posing", false);
	}
}
