using UnityEngine;
using System.Collections;
using System;

public class GorillaHandlerScore : GorillaEventLifecycle {
    private EventPoints scoring;

    public void Start()
    {
        scoring = gameObject.GetComponent<EventPoints>();
    }

    public override void StartEvent(GorillaEventData data, GorillaEventFSM fsm)
    {
        scoring.points = data.score;
    }

    public override void EndEvent(GorillaEventData data, GorillaEventFSM fsm)
    {
        scoring.points = 0;
    }
}
