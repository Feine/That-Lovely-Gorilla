using UnityEngine;

public abstract class GorillaEventLifecycle : MonoBehaviour
{
    public abstract void StartEvent(GorillaEventData data, GorillaEventFSM fsm);
    public abstract void EndEvent(GorillaEventData data, GorillaEventFSM fsm);
}