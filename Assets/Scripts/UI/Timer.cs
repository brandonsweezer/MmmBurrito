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

	private bool isfast;
	private bool isvfast;

	private float totalSeconds;

	private string timeDisplay;
	public Text timeDisplayText;


	private bool running;

	private const float SIGNAL_COOLDOWN = 9f;
	private float lastSignalTime = -SIGNAL_COOLDOWN*2f;
	private bool alreadySignaledLevelStart;


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
	Vector2 signalingPos = new Vector2 (-Screen.width / 2, -Screen.height * 0.5f);
	Vector3 signalingScale = new Vector3(1.5f, 1.5f, 1.5f);
	Color signalingTint = new Color(1, 1, 1, 0.7f);

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

		isfast = false;
		isvfast = false;

		timeLeftWarningText = timeLeftWarningContainer.transform.GetChild (0).GetComponent<Text> ();
	}

	public void TimerInit (int maxTime) {
		time = maxTime;
		maxT = maxTime;
		timeLeftWarningContainer.SetActive (false);
		UpdateDisplayedTime ();

		// Reset animations.
		animManager.StopAllAnimations ();
		animManager.ResetToInitialValues();
		signalingTimeLeft = false;
		alreadySignaledLevelStart = false;
		lastSignalTime = -SIGNAL_COOLDOWN * 2f;
	}

	public void startTimer () {
		timeDisplayText.color = Color.black;
		running = true;
		isfast = false;
		isvfast = false;
		signalingTimeLeft = false;
	}

	public void TimerUpdate () {
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
			SoundController.instance.audSrc.PlayOneShot(SoundController.instance.lose, SoundController.instance.SoundEffectVolume.value);
			//            GameController.instance.levelEnd = true;
			//            GameController.instance.levelComplete = true;


		}

		UpdateDisplayedTime ();

		GameController.instance.gameTime = (int)time;

		// Signal the time left (clock animation).
		bool tryToSignal = false;
		// check if at one of the schedules signals
		foreach (float timeTarget in timeLeftSignals) {
			if (Mathf.Abs (time - timeTarget) < 0.25f) {
				tryToSignal = true;
				break;
			}
		}
		// signal if appropriate
		if (tryToSignal && !signalingTimeLeft) {
			if (!IsSignalCooldownOver () && Mathf.Abs (time - 0) > 0.25f) {
				return;
			}
			SignalTimeLeft ();
		}
	}

	void UpdateDisplayedTime() {
		totalSeconds = Mathf.Ceil (time);
		int minutes = (int) totalSeconds / 60;
		int seconds = (int)totalSeconds % 60;
		string secondsDisplay;
		if (seconds < 10) {
			secondsDisplay = "0" + seconds.ToString();
		} else {
			secondsDisplay = seconds.ToString ();
		}
		timeDisplay = minutes.ToString() +":" +secondsDisplay;
		if (seconds <= 30 && minutes == 0) {
			timeDisplayText.color = Color.red;
		}
		timeDisplayText.text = timeDisplay;
	}


	IEnumerator DisplayLoseScreen() {
		yield return new WaitForSeconds (2.2f);
		//OrderUI.instance.setLoseMessage("You Lose! No time left!\nPress escape to return to the main menu");
		OrderUI.instance.setScore (GameController.instance.score.ToString ());
		OrderUI.instance.textfields.currentLevelLose.text = "Level "+GameController.instance.currentLevel;
		LevelLoader.instance.SetEndCanvas (); 
		OrderUI.instance.gameobjectfields.LoseScreen.gameObject.SetActive (true);
	}

	private void signalingTimeEnd() {
		signalingTimeLeft = false;
	}

	// Animates the clock to the middle of the screen, and then back to its default position.
	public void SignalTimeLeft(float duration = 1.75f, float tween1 = 0.75f, float tween2 = 0.5f) {
		lastSignalTime = Time.time;
		signalingTimeLeft = true;
		animManager.MoveToPosAndBack    (signalingPos,   duration, tween1, tween2, signalingTimeEnd);
		animManager.ScaleToValueAndBack (signalingScale, duration, tween1, tween2);
		// animManager.TintToColorAndBack (signalingTint, duration, tween1, tween2);
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

	private bool IsSignalCooldownOver() {
		return (lastSignalTime + SIGNAL_COOLDOWN) <= Time.time;
	}
}
