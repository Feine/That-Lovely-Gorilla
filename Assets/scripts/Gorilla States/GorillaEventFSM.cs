using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Controller and loader for all GorillaEvents
/// </summary>
public class GorillaEventFSM : MonoBehaviour {
    // TODO: Remove this, it's just easier than watching the logs
    public Text currentEventLabel;

    public TextAsset jsonConfigFile;
    
    public GorillaEventData[] events;

    private GorillaEventData currentEvent;

    private GorillaEventData previousEvent;

    private Dictionary<string, GorillaEventData> eventsByName = new Dictionary<string, GorillaEventData>();

    internal Animator animator;

    private string nextEventName;
    private float scheduledEventEnd;

    public GorillaEventLifecycle[] eventHandlers { get; private set; }

    private string WrapToClass(string source, string topClass)
    {
        return string.Format("{{ \"{0}\": {1}}}", topClass, source);
    }

    public GorillaEventData GetCurrentEvent()
    {
        return currentEvent;
    }

    public GorillaEventData GetPreviousEvent()
    {
        return previousEvent;
    }

    public void Awake()
    {
        string json = WrapToClass(jsonConfigFile.text, "events");
        Debug.Log("Loading json:\n" + json, gameObject);
        events = JsonUtility.FromJson<GorillaEventWrapper>(json).events;
        foreach (GorillaEventData e in events)
        {
            e.Init();
            if (e.name == null)
            {
                Debug.LogError("Unexpectedly empty event name: anim=" + e.anim + ", prefab=" + e.prefab);
                continue;
            }
            if (eventsByName.ContainsKey(e.name))
            {
                Debug.LogWarning("Duplicated event name, overwriting the original: '" + e.name + "'");
            }
            eventsByName[e.name] = e;
        }
    }

    public void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        eventHandlers = GetComponents<GorillaEventLifecycle>();
        TriggerNextEvent();
    }

    public void LateUpdate()
    {
        if (!float.IsInfinity(scheduledEventEnd) && scheduledEventEnd < Time.time)
        {
            TriggerNextEvent();
        }
        if (nextEventName != null)
        {
            TriggerEvent(nextEventName);
            nextEventName = null;
        }
    }

    public void ScheduleEvent(string eventName)
    {
        Debug.Log("Replacing current scheduled event ("+nextEventName+") at "+Time.time+" with new scheduled event: " + eventName);
        nextEventName = eventName;
    }

    public void EndEventAfter(float seconds)
    {
        Debug.Log("Scheduling end of " + currentEvent.name + " at " + Time.time + "  for " + (Time.time + seconds));
        scheduledEventEnd = Time.time + seconds;
    }

    private void TriggerEvent(string eventName)
    {
        Debug.Log("Executing event at " + Time.time + " : " + eventName);
        if (eventsByName.ContainsKey(eventName))
        {
            if (currentEvent != null) currentEvent.EndEvent(this);
            previousEvent = currentEvent;
            currentEvent = eventsByName[eventName];
            currentEvent.StartEvent(this);
            currentEventLabel.text = "Current event: " + eventName 
                + "\nPrev event: "+ (previousEvent==null?"":previousEvent.name)
                + "\nAnim: " + currentEvent.anim 
                + "\nPrefab: " + currentEvent.prefab;
        }
        else
        {
            Debug.LogError("Unknown event name at " + Time.time + " : '" + eventName + "'");
            TriggerEvent("default");
        }
    }

    public void TriggerRandomEvent()
    {
        currentEvent = events[Random.Range(0, events.Length)];
        ScheduleEvent(currentEvent.name);
    }

    public void TriggerNextEvent()
    {
        if(currentEvent == null)
        {
            ScheduleEvent("default");
        } else
        {
            string nextEventName = currentEvent.GetNextEvent();
            switch(nextEventName)
            {
                case "REPEAT": ScheduleEvent(currentEvent.name); break;
                case "RANDOM": TriggerRandomEvent(); break;
                default: ScheduleEvent(nextEventName); break;
            }
        }
    }
}

public class GorillaEventWrapper
{
    public GorillaEventData[] events;
}
