using UnityEngine;
using System.Collections;

public class InterestBar : MonoBehaviour {
	public float drainTime, drainRate;	
	float timer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		timer += Time.deltaTime;
		if(timer > drainTime && transform.localScale.x > 0)
		{
			transform.localScale = new Vector3 (transform.localScale.x - drainRate, transform.localScale.y);
			timer = 0;
		}
	}

	public void AddInterest(float addRate)
	{
		if(transform.localScale.x + addRate < 1)
		transform.localScale = new Vector3 (transform.localScale.x + addRate, transform.localScale.y);
		else
			transform.localScale = new Vector3 (1, transform.localScale.y);
	}
}