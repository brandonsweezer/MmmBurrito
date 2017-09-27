using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Slider sliderInstance;

	private float time;

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
	}

	public void startTimer () {
		running = true;
	}

	public void TimerUpdate () {
		time -= Time.deltaTime; 
		sliderInstance.value = time;
		if (time < 10) {
			textInstance.color = Color.red;
			if (time < 0) {
				time = 0.0f;
				losetext.text = "Level Over";
			}
		}
		if (Mathf.Ceil (time) == 1) {
			timeDisplay = "Time Remaining: " + (Mathf.Ceil (time)).ToString () + " Second";
		} 
		else { 
			timeDisplay = "Time Remaining: " + (Mathf.Ceil (time)).ToString () + " Seconds";
		}
		textInstance.text = timeDisplay;
	

	}
	
	// Update is called once per frame
	void Update () {
		if (running) {
			TimerUpdate ();
		}
	}
}
