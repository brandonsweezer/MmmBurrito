using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.Tween;
using System;

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

	public void SetScoreDisplay(int value) {
		scoreText.text = ""+value;
	}

	public void AnimateScore(int newScore) {
		int prevScore = Int32.Parse(scoreText.text);
		scoreText.gameObject.Tween ("scoreTween"+Time.time, prevScore, newScore, .3f, TweenScaleFunctions.QuinticEaseOut, (t) => {
			SetScoreDisplay((int)t.CurrentValue);
		});
	}
}
