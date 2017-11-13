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


	// Make this class a singleton
	public static Timer instance = null;
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (this);
		}
		animManager = TimerObject.GetComponent<UIAnimationManager> ();
	}

	// showing time left
	private bool signalingTimeLeft;
	private float[] timeLeftSignals = {30f, 10f, 0f};
	private UIAnimationManager animManager;
	Vector2 signalingPos = new Vector2 (-Screen.width / 2, -Screen.height * 0.27f);
	Vector3 signalingScale = new Vector3(1.5f, 1.5f, 1.5f);

    bool thirty;
    bool ten;

//	void Awake() {
//		animManager = TimerObject.GetComponent<UIAnimationManager> ();
//	}

    void Start () {
		running = false;
		timeLeftWarningContainer.SetActive (false);
        thirty = false;
        ten = false;

		timeLeftWarningText = timeLeftWarningContainer.transform.GetChild (0).GetComponent<Text> ();
    }

	public void TimerInit (int maxTime) {
		time = maxTime;
		maxT = maxTime;
		timeLeftWarningContainer.SetActive (false);
	}

	public void startTimer () {
		timeDisplayText.color = Color.black;
		running = true;
        SoundController.instance.audSrc.clip = SoundController.instance.music;
        SoundController.instance.audSrc.loop = true;
        SoundController.instance.audSrc.Play();
        SoundController.instance.audSrc.volume = SoundController.instance.MasterVolume;

		// Reset animations.
		animManager.StopAllAnimations ();
		animManager.ResetToInitialValues();
		signalingTimeLeft = false;
	}

	public void TimerUpdate () {
		// only update timer if level is in progress
//		if (GameController.instance.levelComplete) {
//			return;
//		}
		if (GameController.instance.gamestate!=GameController.GameState.Play) {
			return;
		}

		time -= Time.deltaTime;
		circle.fillAmount = time/maxT;
		if (time < 0) {
			time = 0.0f;
			StartCoroutine (DisplayLoseScreen ());
            LoggingManager.instance.RecordEvent(7, "Level quit, timer at 0");
			GameController.instance.gamestate = GameController.GameState.Lose;
//            GameController.instance.levelEnd = true;
//            GameController.instance.levelComplete = true;


		}
		bool timeEnding = false;
		totalSeconds = Mathf.Ceil (time);
		int minutes = (int) totalSeconds / 60;
        int seconds = (int)totalSeconds % 60;

		string secondsDisplay;
        if (totalSeconds == 30 && thirty == false) //TICKING
        {
            thirty = true;
            SoundController.instance.audSrc.Stop();
            SoundController.instance.audSrc.PlayOneShot(SoundController.instance.ticking,SoundController.instance.SoundEffectVolume);
            SoundController.instance.audSrc.clip = SoundController.instance.musicUrgent;
            SoundController.instance.audSrc.Play();

        }
        if (totalSeconds == 10 && ten == false) //URGENT TICKING
        {
            ten = true;
            SoundController.instance.audSrc.Stop();
            SoundController.instance.audSrc.PlayOneShot(SoundController.instance.urgentTicking, SoundController.instance.SoundEffectVolume);
            SoundController.instance.audSrc.clip = SoundController.instance.musicExtraUrgent;
            SoundController.instance.audSrc.Play();
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
            SoundController.instance.audSrc.Stop();
        }

        GameController.instance.gameTime = (int)time;


		timeDisplayText.text = timeDisplay;
	
		// Signal the time left (clock animation).
		bool tryToSignal = false;
		// check if at one of the schedules signals
		foreach (float timeTarget in timeLeftSignals) {
			if (Mathf.Abs (time - timeTarget) < 0.25f) {
				tryToSignal = true;
				break;
			}
		}
		// check if at level start
		if (LevelJustStarted()) {
			tryToSignal = true;
		}
		// signal if appropriate
		if (tryToSignal && !signalingTimeLeft) {
			if (LevelJustStarted ()) {
				SignalTimeLeft (1.5f, 0f);
			} else {
				SignalTimeLeft ();
			}
		}
	}

	IEnumerator DisplayLoseScreen() {
		yield return new WaitForSeconds (2.2f);
		//OrderUI.instance.setLoseMessage("You Lose! No time left!\nPress escape to return to the main menu");
		OrderUI.instance.setScore (GameController.instance.score.ToString ());
		LevelLoader.instance.SetEndCanvas (); 
		OrderUI.instance.gameobjectfields.LoseScreen.gameObject.SetActive (true);
	}

	private void signalingTimeEnd() {
		signalingTimeLeft = false;
	}

	// Animates the clock to the middle of the screen, and then back to its default position.
	private void SignalTimeLeft(float duration = 1.75f, float tween1 = 0.75f, float tween2 = 0.5f) {
		signalingTimeLeft = true;
		animManager.MoveToPosAndBack    (signalingPos,   duration, tween1, tween2, signalingTimeEnd);
		animManager.ScaleToValueAndBack (signalingScale, duration, tween1, tween2);
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

	public string getDisplayTime() {
		return timeDisplay;
	}

	private bool LevelJustStarted() {
		return time >= (maxT - 1);
	}
}
