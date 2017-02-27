using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class GorillaEventLink  {
    public static readonly GorillaEventLink DEFAULT_LINK = new GorillaEventLink("default", 1);
    /// <summary>
    /// The name of the next event to play. Special animation values:
    ///     RANDOM: Choose a random event to play out of all available events, with no weighting.
    ///     REPEAT: Play the current event again.
    /// </summary>
    public string next;
    public int weight;

    public GorillaEventLink(string nextEvent, int eventWeight)
    {
        this.next = nextEvent;
        this.weight = eventWeight;
    }

    public string GetNextEvent()
    {
        return next;
    }

    public int GetEventWeight()
    {
        if (weight == 0) weight = 1;
        return weight;
    }
}

