﻿using UnityEngine;
using System.Collections;
using System;

public class GorillaHandlerMovement : GorillaEventLifecycle {
    public float speed = 0.01f;

    float? movementWaypoint;

    GorillaEventFSM fsm;

    public override void EndEvent(GorillaEventData data, GorillaEventFSM fsm)
    {
        movementWaypoint = null;
    }

    public override void StartEvent(GorillaEventData data, GorillaEventFSM fsm)
    {
        if (data.targetPosition == null) return;

        this.fsm = fsm;
        // Keep moving until we reach our goal
        fsm.EndEventAfter(float.PositiveInfinity);
        switch (data.targetPosition)
        {
            case "RANDOM":
                movementWaypoint = UnityEngine.Random.Range(-1f, 1f);
                break;
            default:
                throw new NotImplementedException("Unexpected waypoint: " + data.targetPosition);
        }

        Debug.Log("Walking to " + movementWaypoint.Value);

        switch (data.targetSpeed)
        {
            case "run": speed = 0.2f;
                break;
            case "walk":
                speed = 0.1f;
                break;
            default:
                speed = 0.1f;
                break;
        }
    }
    
	void FixedUpdate () {
        if (movementWaypoint.HasValue)
        {
            transform.position = Vector3.MoveTowards(transform.position, (new Vector3(movementWaypoint.Value, transform.position.y, transform.position.z)), speed * Time.deltaTime);
            if(Math.Abs(transform.position.x - movementWaypoint.Value) < 0.01f)
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