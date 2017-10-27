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

    //Audio vars
    private AudioSource audSrc;
    private AudioClip tickReg;
    private AudioClip tickUrgent;
    private AudioClip urgent;
    private AudioClip veryUrgent;
    private AudioClip regular;

    bool thirty;
    bool ten;

    void Start () {
		running = false;

        audSrc = gameObject.AddComponent<AudioSource>();
        tickReg = Resources.Load<AudioClip>("Sound/30 secs");
        tickUrgent = Resources.Load<AudioClip>("Sound/10 secs");
        urgent = Resources.Load<AudioClip>("Sound/musicUrgent");
        veryUrgent = Resources.Load<AudioClip>("Sound/musicExtraUrgent");
        regular = Resources.Load<AudioClip>("Sound/music");
        thirty = false;
        ten = false;
    }

	public void TimerInit (int maxTime) {

		time = maxTime;
		maxT = maxTime;
		tick = .0001f;
	}

	public void startTimer () {
		textInstance.color = Color.black;
		running = true;
        audSrc.clip = regular;
        audSrc.loop = true;
        audSrc.Play();
        audSrc.volume = .6f;
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
        int seconds = (int)totalSeconds % 60;

		string secondsDisplay;
        if (totalSeconds == 30 && thirty == false) //TICKING
        {
            thirty = true;
            audSrc.Stop();
            audSrc.PlayOneShot(tickReg);
            audSrc.clip = urgent;
            audSrc.Play();

        }
        if (totalSeconds == 10 && ten == false) //URGENT TICKING
        {
            ten = true;
            audSrc.Stop();
            audSrc.PlayOneShot(tickUrgent);
            audSrc.PlayOneShot(veryUrgent);
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
