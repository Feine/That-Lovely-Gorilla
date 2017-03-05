using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Clock : MonoBehaviour {

	float timer;
	Text timeText;

	public int minutes,seconds;
	public int totalSeconds;
//	int totalSeconds;
	void Start()
	{
		totalSeconds = (minutes * 60) + seconds;
		timeText = GetComponent<Text> ();
	}

	void Update () {
		timeText.text = minutes + ":" + seconds.ToString("00");

		if (seconds > -1 || minutes > 0) {
			timer += Time.deltaTime;

			if (timer > 1) {
				seconds--;
				timer = 0;
			}
			if (seconds == -1 && minutes != 0)
				MinuteToSeconds ();
		} else
			timeText.text = "0:00";
	}

	void MinuteToSeconds(){
		minutes--;
		seconds = 59;
	}
}
