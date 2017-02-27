using UnityEngine;
using System.Collections;

public class EventPoints : MonoBehaviour {
	public int points;
	public float interest;

	void OnTriggerEnter2D(Collider2D col)
	{
		Scoring.score += points;
		GameObject.Find("INTEREST BAR").GetComponent< InterestBar>().AddInterest (interest);
	}
}
