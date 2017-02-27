using UnityEngine;
using System.Collections;
using System;

public class GorillaHandlerAnimation : GorillaEventLifecycle {
    public override void StartEvent(GorillaEventData data, GorillaEventFSM fsm)
    {
        if (data.anim != null)
        {
            Debug.Log("Starting animation at "+Time.time+": " + data.anim, fsm);
            fsm.animator.CrossFade(data.anim, 0f);
        }
    }

    public override void EndEvent(GorillaEventData data, GorillaEventFSM fsm)
    {
    }
}
