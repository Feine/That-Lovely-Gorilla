using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

[Serializable]
public enum GorillaMoveSpeed
{
    WALK, RUN
}

/// <summary>
/// Holds data about an event that can happen around/to a gorilla
/// </summary>
[Serializable]
public class GorillaEventData {

    /// <summary>
    /// Should contain a unique name for this event. Used to reference other events for linked events, etc.
    /// Can be omitted if you provide either 'proto' or 'anim', and the value of 'proto' or 'anim' is unique among all event names.
    /// </summary>
    public string name;

    /// <summary>
    /// If non-null, the gorilla should play this animation.
    /// </summary>
    public string anim;
	/// <summary>
	/// Flips the sprite back the right way for certain animations
	/// </summary>
	public bool flipSprite;
	/// <summary>
	/// If non-null, hides the named prop sprite;
	/// </summary>
	public string hideSprite;
	/// <summary>
	/// if non-null, shows hidden sprite.
	/// </summary>
	public string showSprite;
    /// <summary>
    /// If non-null, the prefab with this name should be cloned. The prototype may optionally contain a component which subclasses GorillaEventLifecycle to handle
    /// lifecycle events. Otherwise, the default behavior is to play the animationName for this event (if any) and then immediately transition to a new event.
    /// </summary>

	public string swapSprite;

    public string prefab;

    /// <summary>
    /// How long to hold the current state before ending it.
    /// </summary>
    public float delay = 0;
	/// <summary>
	/// adjusts the gorilla to allign with objects
	/// </summary>
	public float verticalCorrection;
    /// <summary>
    /// Updates the gorilla's desired position. Uses position names that should be defined elsewhere.
    /// A value of "RANDOM" should cause a random valid position to be chosen.
    /// </summary>
    public string targetPosition;

    /// <summary>
    /// Updates the gorilla's move speed. 
    /// </summary>
    public string targetSpeed;

    /// <summary>
    /// How much a photo of the gorilla is worth while he's in this state
    /// </summary>
    public int score;

    /// <summary>
    /// A list of links which can be triggered from this event. 
    /// If this is left null or empty then there is a static implied link, GorillaEventLink.DEFAULT_LINK, which will be used instead.
    /// </summary>
    public GorillaEventLink[] links;

    /// <summary>
    /// Stores the total weight of all the registered event links, so we can reliably select a random event with weighting.
    /// A given link's probability of being chosen is (weight/totalWeight)
    /// </summary>
    private int totalWeight;

    private bool isInitialized = false;

    /// <summary>
    /// Initialize internal data. Should be called before trying to use any public methods that rely on the total weight of the links.
    /// </summary>
    public void Init()
    {
        if (isInitialized) return;
        if (name == null || name.Equals(""))
        {
            if (prefab != null) name = prefab;
            else if (anim != null) name = anim;
        }
        if (links == null || links.Length == 0)
        {
            links = new GorillaEventLink[] { GorillaEventLink.DEFAULT_LINK };
        }
        foreach(GorillaEventLink link in links)
        {
            totalWeight += link.GetEventWeight();
        }
        isInitialized = true;
    }

    /// <summary>
    /// Chooses a random 'next event' based on the weighting of this object's event links.
    /// </summary>
    /// <returns></returns>
    public string GetNextEvent()
    {
        Init();
        // Simple case - there's only one event, so just return it. Saves some work in generating random numbers, iteration, etc.
        if (links.Length == 1)
            return links[0].GetNextEvent();

        // Multiple 'next event's are possible, so pick a random one taking into account the weight of each one. 
        int chosenIdx = 0;
        int weight = UnityEngine.Random.Range(0, totalWeight);
        do
        {
            var curLink = links[chosenIdx];
            if (weight < curLink.GetEventWeight())
            {
                Debug.Log("Randomly chose option "+curLink.GetNextEvent()+" at " + Time.time + ", idx=" + (chosenIdx+1) + " out of " + links.Length +" with totalWeight="+totalWeight);
                return curLink.GetNextEvent();
            }
            weight -= curLink.GetEventWeight();
            chosenIdx++;
        } while (weight >= 0);

        // This should never happen, but just in case...
        Debug.LogError("Something's wrong with random chooser, it fell through for " + name);
        return "default";
    }


    /// <summary>
    /// Do whatever you need to shut down a running event here. 
    /// </summary>
    /// <param name="fsm"></param>
    internal void EndEvent(GorillaEventFSM fsm)
    {
		if (showSprite != null)
			GameObject.Find (showSprite).GetComponent<SpriteRenderer> ().enabled = true;
        foreach (GorillaEventLifecycle handler in fsm.eventHandlers)
        {
            handler.EndEvent(this, fsm);
        }
    }
    /// <summary>
    /// Do whatever you need to start a new event up here.
    /// </summary>
    /// <param name="fsm"></param>
    internal void StartEvent(GorillaEventFSM fsm)
    {
		if (hideSprite != null)
			GameObject.Find (hideSprite).GetComponent<SpriteRenderer> ().enabled = false;

        // If there's a fixed delay set, schedule the end of the event
        if (delay > 0)
        {
            fsm.EndEventAfter(delay);
        }
        else if (prefab == null)
        {
            if (anim == null)
            {
                // If no prefab and no animation, this is a zero-length intermediate event that's probably chaining directly into another event
                if (anim == null && prefab == null)
                {
                    fsm.EndEventAfter(0);
                }
            }
            else
            {
                // Only auto-end the event if there's an animation but no prefab and no delay set. If there's a prefab we assume that it will be responsible
                // for ending the event.
                fsm.EndEventAfter(4);
            }
        }
        foreach (GorillaEventLifecycle handler in fsm.eventHandlers)
        {
            handler.StartEvent(this, fsm);
        }
    }
}
