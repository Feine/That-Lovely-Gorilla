using UnityEngine;
using System.Collections;
using System;

public class GorillaHandlerMovement : GorillaEventLifecycle {
    public float speed = 0.01f;

	bool flip= false;

    float? movementWaypoint;

    GorillaEventFSM fsm;

    public override void EndEvent(GorillaEventData data, GorillaEventFSM fsm)
    {
        movementWaypoint = null;
    }

    public override void StartEvent(GorillaEventData data, GorillaEventFSM fsm)
    {
		// flips the sprite if the animation requires it
		if (data.flipSprite = true)
			flip = true;
		else
			flip = false;


        if (data.targetPosition == null) return;

        this.fsm = fsm;
        // Keep moving until we reach our goal
        fsm.EndEventAfter(float.PositiveInfinity);
        switch (data.targetPosition)
        {
            case "RANDOM":
                movementWaypoint = UnityEngine.Random.Range(-2.24f, 2.24f);
                break;
			case "BOULDER":
			movementWaypoint = -0.392f;
				break;
			case "PULLUP":
				movementWaypoint = -1.49f;
				break;	
            default:
                throw new NotImplementedException("Unexpected waypoint: " + data.targetPosition);
        }

        Debug.Log("Walking to " + movementWaypoint.Value);

        switch (data.targetSpeed)
        {
            case "run": speed = 0.3f;
                break;
            case "walk":
                speed = 0.2f;
                break;
            default:
                speed = 0.1f;
                break;
        }
    }
    
	void FixedUpdate () {
		if(flip)
			transform.localScale = new Vector3(1f, transform.localScale.y);
		if (movementWaypoint.HasValue) {
			transform.position = Vector3.MoveTowards (transform.position, (new Vector3 (movementWaypoint.Value, transform.position.y, transform.position.z)), speed * Time.deltaTime);
			// causes issues with the gorilla lining up with the props; moveTowards puts them in an exact position without having to worry about overshooting
          //  if(Math.Abs(transform.position.x - movementWaypoint.Value) < 0.01f)
			if(transform.position.x == movementWaypoint.Value)
            {            
                // if we reached our target, end the event
                fsm.EndEventAfter(0);
            }

            // flips the gorillas sprite based on where the waypoint is and whether it will be moving or not. Also chooses a random walking animation
            if (movementWaypoint.Value < transform.position.x)
                transform.localScale = new Vector3(-1f, transform.localScale.y);
			else
                transform.localScale = new Vector3(1f, transform.localScale.y);
        }
    }

    void WalkingBehaviour()
    {
        // flips the gorillas sprite based on where the waypoint is and whether it will be moving or not. Also chooses a random walking animation
        if (movementWaypoint < transform.position.x)
            transform.localScale = new Vector3(-1f, transform.localScale.y);
        else
            transform.localScale = new Vector3(1f, transform.localScale.y);
    }

}
