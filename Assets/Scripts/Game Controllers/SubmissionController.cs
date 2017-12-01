using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DigitalRuby.Tween;
using UnityEngine.UI;


public class SubmissionController : MonoBehaviour {

	public GameObject scorePopupPrefab;

	private string submissionText; 
	private string winText; 

    private CaughtIngredientSet burritoCaughtIngredients;

    private Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, new Vector3(1, 0, 1));

    private AudioSource audSrc;

    private Camera mainCam;

	private int SCORE_PER_SECOND = 5;

	void Awake() {
		mainCam = Camera.main;
	}

	void Start () {
        audSrc = SoundController.instance.audSrc;

    }

	private float timeOfLastChange = -1;
	private float submitDelayAfterChange = 0.4f;
	private int lastNumIngredients = -1;
	private bool changeWasMade = false;

	void OnTriggerStay (Collider other) {
		// dont do anything if we didn't do anything new
		if (Time.time > timeOfLastChange + submitDelayAfterChange && changeWasMade) {
			Debug.Log ("last change was long enough ago. gogogo");
			OnTriggerEnter (other);
		}
	}

	void Update() {
		// dont do anything if we didn't do anything new
		if (GameController.instance.player != null) {
			int newNumIngredients = GameController.instance.player.GetComponent<ObjectCatcher> ().GetNumCaughtIngredients ();
			if (lastNumIngredients != newNumIngredients) {
				lastNumIngredients = newNumIngredients;
				timeOfLastChange = Time.time;
				changeWasMade = true;
			}
		}
	}
	
	void OnTriggerEnter(Collider other) {
		// Disregard any collisions that aren't with the burrito
		GameObject burrito = other.gameObject;
		if (burrito.tag != "Player") {
			return;
		}

		timeOfLastChange = Time.time;
		changeWasMade = false;

		// Prevent submission if we haven't caught any objects.
		if (burrito.GetComponent<ObjectCatcher> ().IsEmpty ()) {
			return;
		}

		SubmitBurrito (burrito);


		if (GameController.instance.gamestate != GameController.GameState.Play) {
			SpawnController.instance.DestroyBurrito ();
		}
	}

	/** Submits a burrito */
	void SubmitBurrito (GameObject burrito) {
		Debug.Log ("Submitted a burrito with contents: " + burrito.GetComponent<ObjectCatcher> ().CaughtObjectsToString ());

		// Get a reference to the caught ingredients
		burritoCaughtIngredients = GameController.instance.player.GetComponent<ObjectCatcher> ().GetIngredients ();

		// Logic regarding ordering system.
		List<Order> orders = OrderController.instance.activeOrders;
		bool matched = false;
		foreach (Order order in orders) {
            if (OrderController.instance.BurritoContentsFulfillOrder (order)) {
				//MATCHES
				matched = true;
				ProcessSuccessfulOrderSubmission(order);
				break;
			}
		} 
		if (!matched) {
            //DOES NOT MATCH
            audSrc.PlayOneShot(SoundController.instance.wrongSubmission);
			Debug.Log("Submitted burrito does not match");
			OrderUI.instance.setGeneralMessage ("Incorrect Burrito");
					//setTextString ("Invalid Burrito Submission");
            LoggingManager.instance.RecordEvent(1, "Submitted ingredients: " + GameController.instance.player.GetComponent<ObjectCatcher>().GetIngredients().ToString()
            + ". Did not match.");
        }

		// Update UI
	}

	void ProcessSuccessfulOrderSubmission(Order order) {
		audSrc.PlayOneShot(SoundController.instance.dingding);
		Debug.Log ("Matches one of the orders!");

		// Add the score
		int score = burritoCaughtIngredients.getSumOfQualities ()*50;
		GameController.instance.SetScore(score + GameController.instance.GetScore());
		CreateScorePopup (score);
		Debug.Log("You just got "+score+" score!");
		Debug.Log("Total Score: "+GameController.instance.score);

        float rnd = Random.value;

        if (score >= 100)
        {
            if (rnd < .33)
            {
                audSrc.PlayOneShot(SoundController.instance.mmmHi);
            }
            else if (rnd >= .33 && rnd < .66)
            {
                audSrc.PlayOneShot(SoundController.instance.mmmMed);
            }
            else if (rnd >= .66)
            {
                audSrc.PlayOneShot(SoundController.instance.mmmLo);
            }

        }

		// Log the successful submission
		LoggingManager.instance.RecordEvent(2, "Submitted ingredients: " + GameController.instance.player.GetComponent<ObjectCatcher>().GetIngredients().ToString()
			+ ". Gained score: " + score);

		// Fulfill the order and empty burrito
		OrderController.instance.FulfillOrder (order);
		GameController.instance.player.GetComponent<ObjectCatcher> ().GetIngredients ().Empty();

		// Determine if we won the level
		if (OrderController.instance.activeOrders.Count != 0){
			//OrderUI.instance.setGeneralMessage("Burrito Submitted");
			Debug.Log ("Remaining " + OrderController.instance.OrderListToString ()); // print remaining orders
		} else {
			ProcessLevelWin ();
		}
	}

	void ProcessLevelWin() {
		Debug.Log ("All orders completed");

		LoggingManager.instance.RecordEvent (7, "Level quit, timer at " + GameController.instance.gameTime);
		LoggingManager.instance.RecordEvent (8, "Won level, timer at " + GameController.instance.gameTime);
		GameController.instance.gamestate = GameController.GameState.Win;
		OrderUI.instance.textfields.currentLevelWin.text = "Level "+GameController.instance.currentLevel;

		// calculate score
		int scoreOrders = GameController.instance.score;
		int scoreTime = (int)Mathf.Floor(Timer.instance.getTime()) * SCORE_PER_SECOND;
		int levelScore = scoreOrders + scoreTime;
		StartCoroutine (DisplayWinScreen());

		int nextStar = 0; 
		// find number of stars
		int numStars = 0;
		if (levelScore >= GameController.instance.starScore [0]) { 
			numStars++;
			nextStar = GameController.instance.starScore [1];
		}
		if (levelScore >= GameController.instance.starScore [1]) {
			numStars++;
			nextStar = GameController.instance.starScore [2];
		}
		if (levelScore >= GameController.instance.starScore [2]) {
			numStars++;
		}


		Text nextStarText = OrderUI.instance.gameobjectfields.WinScreen.gameObject.transform.GetChild (12).gameObject.GetComponent<Text> ();
		nextStarText.text = nextStar.ToString();
		OrderUI.instance.gameobjectfields.WinScreen.gameObject.transform.GetChild (11).gameObject.SetActive (false);
		OrderUI.instance.gameobjectfields.WinScreen.gameObject.transform.GetChild (12).gameObject.SetActive (false);
		OrderUI.instance.gameobjectfields.WinScreen.gameObject.transform.GetChild (13).gameObject.SetActive (false);

		// animate the win
		OrderUI.instance.setWinTime(Timer.instance.getDisplayTime ());
		OrderUI.instance.AnimateScoreWin(scoreOrders, scoreTime, Timer.instance.getTime(), numStars);

		// save level
		SaveManager.instance.ProcessLevelCompletion(GameController.instance.currentLevel, levelScore, numStars);
		int i = GameController.instance.currentLevel;
		Debug.Log ("curr= " + GameController.instance.currentLevel);
		Debug.Log ("stars= " + SaveManager.instance.totalStars());
		while (LevelLoader.instance.maxLevelUnlocked < LevelLoader.instance.maxLevelNumber && SaveManager.instance.totalStars() >= GameController.instance.starUnlock[i-1]) {
			LevelLoader.instance.maxLevelUnlocked = i; 
			i++;
		}
		Debug.Log ("max= " + LevelLoader.instance.maxLevelUnlocked);


        //submit stars to kongregate api
        Application.ExternalCall("kongregate.stats.submit", "Stars", SaveManager.instance.totalStars());
        //submit highest level completed
        Application.ExternalCall("kongregate.stats.submit", "HighestLevel", SaveManager.instance.GetLastLevelCompleted());
    }

	IEnumerator DisplayWinScreen() {
		yield return new WaitForSeconds (1);
		LevelLoader.instance.SetEndCanvas (); 
		OrderUI.instance.gameobjectfields.WinScreen.gameObject.SetActive (true);
		if ((GameController.instance.currentLevel + 1) > LevelLoader.instance.maxLevelUnlocked && GameController.instance.currentLevel < LevelLoader.instance.maxLevelNumber) {
			//Next Level Locked
			OrderUI.instance.gameobjectfields.nextButton.interactable = false;
		} else {
			//Next Level Unlocked
			OrderUI.instance.gameobjectfields.nextButton.interactable = true;
		}

		if (GameController.instance.currentLevel < LevelLoader.instance.maxLevelNumber) {
			OrderUI.instance.gameobjectfields.levelsButton.gameObject.SetActive(true);
			OrderUI.instance.gameobjectfields.replayButton.gameObject.SetActive(true);
		} else {
			OrderUI.instance.gameobjectfields.levelsButton.gameObject.SetActive(false);
			OrderUI.instance.gameobjectfields.replayButton.gameObject.SetActive(false);
		}
	}

	void CreateScorePopup(int score) {
		GameObject scorePopup = Instantiate (scorePopupPrefab, transform) as GameObject;
		scorePopup.GetComponent<TextMesh> ().text = "+" + score;
		scorePopup.transform.rotation = mainCam.transform.rotation;
		// Tween to pop up, then fade out
		string tweenKey = "scorePopup_"+Time.time+"_"+name;
		Vector3 initialPos = scorePopup.transform.position + mainCam.transform.up * 0.8f;
		Vector3 endPos = initialPos + mainCam.transform.up * 1.6f;

		scorePopup.Tween (tweenKey + "scale", 0f, 1f, .5f, TweenScaleFunctions.QuinticEaseOut, (t) => {
			// fade in
			scorePopup.GetComponent<TextMesh> ().color = new Color (0, 1, 0, t.CurrentValue);
		});

		scorePopup.Tween (tweenKey, initialPos, endPos, .5f, TweenScaleFunctions.QuinticEaseOut, (t) => 
			{
				// pop up
				scorePopup.transform.position = t.CurrentValue;
			}, (t) => 
			{
				scorePopup.Tween (tweenKey, 1f, 0f, 1.1f, TweenScaleFunctions.QuinticEaseIn, (t2) => 
					{
						if (t2.CurrentProgress > 0.6f) {
							// animate total score incrementing
							ScoreUI.instance.AnimateScore (GameController.instance.score);
						}
						// fade out
						scorePopup.GetComponent<TextMesh>().color = new Color(0, 1, 0, t2.CurrentValue);
					}, (t2) =>
					{
						// remove at the end
						Destroy(scorePopup);
					});
			}
		);
	}
}
