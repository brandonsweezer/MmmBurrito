using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {


	public Image circle;

	public GameObject timeLeftWarningContainer;
	public GameObject TimerObject;
	private Text timeLeftWarningText;


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
    private AudioClip urgent;
    private AudioClip veryUrgent;
    private AudioClip regular;

	// showing time left
	private bool signalingTimeLeft;
	private float[] timeLeftSignals = {30f, 10f, 0f};

    bool thirty;
    bool ten;

    void Start () {
		running = false;
		timeLeftWarningContainer.SetActive (false);

        audSrc = gameObject.AddComponent<AudioSource>();
        tickReg = Resources.Load<AudioClip>("Sound/30 secs");
        tickUrgent = Resources.Load<AudioClip>("Sound/10 secs");
        urgent = Resources.Load<AudioClip>("Sound/musicUrgent");
        veryUrgent = Resources.Load<AudioClip>("Sound/musicExtraUrgent");
        regular = Resources.Load<AudioClip>("Sound/music");
        thirty = false;
        ten = false;

		timeLeftWarningText = timeLeftWarningContainer.transform.GetChild (0).GetComponent<Text> ();
    }

	public void TimerInit (int maxTime) {
		time = maxTime;
		maxT = maxTime;
		signalingTimeLeft = false;
		timeLeftWarningContainer.SetActive (false);
	}

	public void startTimer () {
		timeDisplayText.color = Color.black;
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
			OrderUI.instance.setLoseMessage("You Lose! No time left!\nPress escape to return to the main menu");
			OrderUI.instance.gameobjectfields.LoseScreen.gameObject.SetActive (true);
            LoggingManager.instance.RecordEvent(7, "Level quit, timer at 0");
            GameController.instance.levelEnd = true;
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
			timeDisplayText.color = Color.red;
		}

        if (time == 0)
        {
            audSrc.Stop();
        }

        GameController.instance.gameTime = (int)time;


		timeDisplayText.text = timeDisplay;
	
		// Signal the time left (clock animation).
		foreach (float timeTarget in timeLeftSignals) {
			if (Mathf.Abs (time - timeTarget) < 0.25f) {
				if (!signalingTimeLeft) {
					SignalTimeLeft ();
				}
				break;
			}
			signalingTimeLeft = false;
		}
	}

	// Animates the clock to the middle of the screen, and then back to its default position.
	private void SignalTimeLeft() {
		signalingTimeLeft = true;
		UIAnimationManager animManager = TimerObject.GetComponent<UIAnimationManager> ();
		Vector2 targetPos = new Vector2 (-Screen.width / 2, -Screen.height / 2);
		Vector3 targetScale = new Vector3(1.7f, 1.7f, 1f);
		animManager.MoveToPosAndBack    (targetPos,   1.75f, 0.75f, 0.5f);
		animManager.ScaleToValueAndBack (targetScale, 1.75f, 0.75f, 0.5f);
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
