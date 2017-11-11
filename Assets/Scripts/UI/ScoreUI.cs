using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour {

	public Text scoreText;

	// Make this class a singleton
	public static ScoreUI instance = null;
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (this);
		}
	}

	public void SetScore() {
		scoreText.text = ""+GameController.instance.score;
	}
}
