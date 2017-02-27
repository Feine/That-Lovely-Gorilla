using UnityEngine;
using System.Collections;
using System;

public class GorillaHandlerPrefab : GorillaEventLifecycle {
    /// <summary>
    /// Current copy of the prefab, if any
    /// </summary>
    GameObject prefabInstance;

    public override void StartEvent(GorillaEventData data, GorillaEventFSM fsm)
    {

        if (data.prefab != null)
        {
            prefabInstance = GameObject.Instantiate<GameObject>(Resources.Load(data.prefab) as GameObject);
            Debug.Log("Instantiated prefab at " + Time.time + "  for " + data.name, prefabInstance);
            GorillaEventLifecycle lifecycle = prefabInstance.GetComponent<GorillaEventLifecycle>();
            if (lifecycle != null)
            {
                // Check prefab instance for GorillaEventLifecyle component - if it exists then call EndEvent on it (it may ignore it if desired)
                lifecycle.StartEvent(data, fsm);
                // If the prefab has a lifecycle handler and there's no delay then we shouldn't auto-kill
                if(data.delay <= 0) {
                    fsm.EndEventAfter(float.PositiveInfinity);
                }
            }
        }
    }

    public override void EndEvent(GorillaEventData data, GorillaEventFSM fsm)
    {
        if (prefabInstance != null)
        {
            GorillaEventLifecycle lifecycle = prefabInstance.GetComponent<GorillaEventLifecycle>();
            if (lifecycle != null)
            {
                // Check prefab instance for GorillaEventLifecyle component - if it exists then call EndEvent on it (it may ignore it if desired)
                Debug.Log("Calling EndEvent on prefab instance", prefabInstance);
                lifecycle.EndEvent(data, fsm);
            }
            else
            {
                // Otherwise just delete the GameObject
                Debug.Log("Destroying prefab instance", prefabInstance);
                GameObject.Destroy(prefabInstance);
            }
            prefabInstance = null;
        }
    }
}
