using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {


	public Image circle;


	private float time;
	private float tick;
	private float maxT;

	private string timeDisplay;
	public Text textInstance;
	public Text losetext; 

	private bool running;

	void Start () {
		running = false;
	}

	public void TimerInit (int maxTime) {

		time = maxTime;
		maxT = maxTime;
		tick = .0001f;
	}

	public void startTimer () {
		textInstance.color = Color.black;
		running = true;
	}

	public void TimerUpdate () {
		// only update timer if level is in progress
		if (GameController.instance.levelComplete) {
			return;
		}

		time -= Time.deltaTime;
		circle.fillAmount = time/maxT;
		if (time < 0) {
			time = 0.0f;
			losetext.text = "You Lose! No time left!\nPress escape to return to the main menu";
			GameController.instance.levelComplete = true;
		}
		bool timeEnding = false;
		float totalSeconds = Mathf.Ceil (time);
		int minutes = (int) totalSeconds / 60;
		int seconds = (int) totalSeconds % 60; 

		string secondsDisplay;
		if (seconds < 10) {
			secondsDisplay = "0" + seconds.ToString ();
		} else {
			secondsDisplay = seconds.ToString ();
		}
		if (seconds <= 30 && minutes == 0) {
			timeEnding = true;
		}
			

		timeDisplay = minutes.ToString() +":" +secondsDisplay;
		if (timeEnding) {
			textInstance.color = Color.red;
		}

        GameController.instance.gameTime = (int)time;


        textInstance.text = timeDisplay;
	

	}
	
	// Update is called once per frame
	void Update () {
		if (running) {
			TimerUpdate ();
		}
	}

	public void AddSeconds(float seconds) {
		time += seconds;
	}
}
