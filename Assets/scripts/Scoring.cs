using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Scoring : MonoBehaviour {
	public static int score;
	Text scoreText;
	void Start () {
		scoreText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		scoreText.text = score.ToString ("00000");
	}
}
