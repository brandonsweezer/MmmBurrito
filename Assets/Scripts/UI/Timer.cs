using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {


	public Image circle;


	private float time;
	private float maxT;

	private float totalSeconds;

	private string timeDisplay;
	public Text timeDisplayText;


	private bool running;

    //Audio vars
    private AudioSource audSrc;
    private AudioClip tickReg;
    private AudioClip tickUrgent;

    bool thirty;
    bool ten;

    void Start () {
		running = false;

        audSrc = gameObject.AddComponent<AudioSource>();
        tickReg = Resources.Load<AudioClip>("Sound/30 secs");
        tickUrgent = Resources.Load<AudioClip>("Sound/10 secs+");
        thirty = false;
        ten = false;
    }

	public void TimerInit (int maxTime) {

		time = maxTime;
		maxT = maxTime;
	}

	public void startTimer () {
		timeDisplayText.color = Color.black;
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
			OrderUI.instance.setLoseMessage("You Lose! No time left!\nPress escape to return to the main menu");
			GameController.instance.levelComplete = true;
		}
		bool timeEnding = false;
		totalSeconds = Mathf.Ceil (time);
		int minutes = (int) totalSeconds / 60;
        int seconds = (int)totalSeconds % 60;

		string secondsDisplay;
        if (totalSeconds == 30 && thirty == false) //TICKING
        {
            thirty = true;
            audSrc.PlayOneShot(tickReg);
        }
        if (totalSeconds == 10 && ten == false) //URGENT TICKING
        {
            ten = true;
            audSrc.PlayOneShot(tickUrgent);
        }

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
			timeDisplayText.color = Color.red;
		}

        GameController.instance.gameTime = (int)time;


        timeDisplayText.text = timeDisplay;
	

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

	public float getTime() {
		return totalSeconds;
	}
}
