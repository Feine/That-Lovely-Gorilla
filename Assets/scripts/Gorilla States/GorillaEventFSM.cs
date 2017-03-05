using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Controller and loader for all GorillaEvents
/// </summary>
public class GorillaEventFSM : MonoBehaviour {
    // TODO: Remove this, it's just easier than watching the logs
    public Text currentEventLabel;

    public TextAsset jsonIdles;
	public TextAsset jsonBoulders;
	public TextAsset jsonPullups;

    public GorillaEventData[] idleEvents;
	public GorillaEventData[] boulderEvents;
	public GorillaEventData[] pullupEvents;


	public bool toggleJsonList;
    private GorillaEventData currentEvent;

    private GorillaEventData previousEvent;

    private Dictionary<string, GorillaEventData> eventsByNameIdle = new Dictionary<string, GorillaEventData>();
	private Dictionary<string, GorillaEventData> eventsByNameBoulder = new Dictionary<string, GorillaEventData>();
	private Dictionary<string, GorillaEventData> eventsByNamePullup = new Dictionary<string, GorillaEventData>();


	private Dictionary<string, GorillaEventData> currentDictionary = new Dictionary<string, GorillaEventData>();

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
		string idles= WrapToClass(jsonIdles.text, "events");
        Debug.Log("Loading json:\n" + idles, gameObject);
		idleEvents = JsonUtility.FromJson<GorillaEventWrapper>(idles).events;

		string boulder = WrapToClass(jsonBoulders.text, "events");
		Debug.Log("Loading json:\n" + boulder, gameObject);
		boulderEvents = JsonUtility.FromJson<GorillaEventWrapper>(boulder).events;

		string pullup = WrapToClass(jsonPullups.text, "events");
		Debug.Log("Loading json:\n" + pullup, gameObject);
		pullupEvents = JsonUtility.FromJson<GorillaEventWrapper>(pullup).events;


		initializeLists (idleEvents, eventsByNameIdle);
		initializeLists (boulderEvents, eventsByNameBoulder);
		initializeLists (pullupEvents, eventsByNamePullup);
    }

    public void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        eventHandlers = GetComponents<GorillaEventLifecycle>();
        TriggerNextEvent();

		if(currentEvent == null)
		{
			ScheduleEvent("default");
		}
    }
	float timer = 0;
	bool chosen =false;
    public void LateUpdate()
    {
		if (timer < 5) {
			timer += Time.deltaTime;
			currentDictionary = eventsByNameIdle;
		}
		else {
			if (!toggleJsonList && !chosen) {
				toggleJsonList = true;
				chooseProp ();
				chosen = true;
			}
		}

        if (!float.IsInfinity(scheduledEventEnd) && scheduledEventEnd < Time.time)
        {
            TriggerNextEvent();
        }
        if (nextEventName != null)
        {
			TriggerEvent(nextEventName,currentDictionary);
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

	private void TriggerEvent(string eventName, Dictionary<string, GorillaEventData> dict)
    {
			Debug.Log ("Executing event at " + Time.time + " : " + eventName);
			if (dict.ContainsKey (eventName)) {
				if (currentEvent != null)
					currentEvent.EndEvent (this);
				previousEvent = currentEvent;
				currentEvent = dict [eventName];
				currentEvent.StartEvent (this);
				currentEventLabel.text = "Current event: " + eventName
				+ "\nPrev event: " + (previousEvent == null ? "" : previousEvent.name)
				+ "\nAnim: " + currentEvent.anim
				+ "\nPrefab: " + currentEvent.prefab;
			if (currentEvent.name == "completed") {
				toggleJsonList = false;
				chosen = false;
				timer = 0;
			}
			} else {
				Debug.LogError ("Unknown event name at " + Time.time + " : '" + eventName + "'");
				TriggerEvent ("default",dict);
			}
    }

    public void TriggerRandomEvent()
    {
   //     currentEvent = events[Random.Range(0, events.Length)];
    //    ScheduleEvent(currentEvent.name);
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

	private void chooseProp()
	{
		int i = Random.Range (1, 3);
		switch(i)  {
		case 1:
			currentDictionary = eventsByNameBoulder;
			Debug.Log ("Chose Boulder");
			break;
		case 2:
			currentDictionary = eventsByNamePullup;
			Debug.Log ("Chose pull-up");
			break;
		}

	}

	// fills the various lists with events
	private void initializeLists(GorillaEventData[] list, Dictionary<string, GorillaEventData> dict){
		foreach (GorillaEventData e in list)
		{
			e.Init();
			if (e.name == null)
			{
				Debug.LogError("Unexpectedly empty event name: anim=" + e.anim + ", prefab=" + e.prefab);
				continue;
			}
			if (dict.ContainsKey(e.name))
			{
				Debug.LogWarning("Duplicated event name, overwriting the original: '" + e.name + "'");
			}
			dict[e.name] = e;
		}
	}
}

public class GorillaEventWrapper
{
    public GorillaEventData[] events;
}
