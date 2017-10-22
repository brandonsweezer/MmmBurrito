using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Slider sliderInstance;
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
		sliderInstance.minValue = 0;
		sliderInstance.maxValue = maxTime;
		time = maxTime;
		maxT = maxTime;
		tick = .0001f;
	}

	public void startTimer () {
		running = true;
	}

	public void TimerUpdate () {
		// only update timer if level is in progress
		if (GameController.instance.levelComplete) {
			return;
		}

		time -= Time.deltaTime;
		circle.fillAmount = time/maxT;
		sliderInstance.value = time;
		if (time < 10) {
			textInstance.color = Color.red;
			if (time < 0) {
				time = 0.0f;
				losetext.text = "You Lose! No time left!\nPress escape to return to the main menu";
				GameController.instance.levelComplete = true;
			}
		}
		timeDisplay = (Mathf.Ceil (time)).ToString ();
        GameController.instance.gameTime = (int)time;


        textInstance.text = timeDisplay;
	

	}
	
	// Update is called once per frame
	void Update () {
		if (running) {
			TimerUpdate ();
		}
	}
}
