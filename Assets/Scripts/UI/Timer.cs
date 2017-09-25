using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Slider sliderInstace; 
	public float maxTime;
	public float minTime;
	private float time;

	private string timeDisplay;
	public Text textInstance;
	public Text losetext; 






	public void TimerInit () {
		sliderInstace.minValue = minTime;
		sliderInstace.maxValue =maxTime;
		time = maxTime;
		losetext.text = "";

		//textInstance = GetComponent<Text>();

	}

	public void TimerUpdate () {
		time -= Time.deltaTime; 
		sliderInstace.value = time;
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

	// Use this for initialization
	void Start () {
		TimerInit ();
		
	}
	
	// Update is called once per frame
	void Update () {
		TimerUpdate ();
	}
}
