using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GorillaArrows : MonoBehaviour {

	GameObject arrow;
	GameObject gorilla;

	void Start () {
		gorilla = GameObject.Find ("Gorilla");

		arrow = GameObject.Find ("location arrow");
		arrow.SetActive (false);
	}


	void Update () {
		if (Camera.main.WorldToViewportPoint (gorilla.transform.position).x > 1) {
			arrow.transform.localPosition = new Vector3 (1.25f, 0, 10);
			arrow.transform.localScale = new Vector3 (1, 1, 1);
			arrow.SetActive (true);
		} else if (Camera.main.WorldToViewportPoint (gorilla.transform.position).x < 0) {
			arrow.transform.localPosition = new Vector3 (-1.25f, 0, 10);
			arrow.transform.localScale = new Vector3 (-1, 1, 1);
			arrow.SetActive (true);
		} else
			arrow.SetActive (false);

	}
}
