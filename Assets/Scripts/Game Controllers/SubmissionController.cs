﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DigitalRuby.Tween;


public class SubmissionController : MonoBehaviour {

	public GameObject scorePopupPrefab;

	private string submissionText; 
	private string winText; 

    private CaughtIngredientSet burritoCaughtIngredients;

    private Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, new Vector3(1, 0, 1));

    private AudioSource audSrc;
    private AudioClip rightOrder;
    private AudioClip wrongOrder;

	private Camera mainCam;

	void Awake() {
		mainCam = Camera.main;
	}

	void Start () {
		//setTextString ("");
		//setWinString ("");

        audSrc = gameObject.GetComponent<AudioSource>();
        rightOrder = Resources.Load<AudioClip>("Sound/submit(right)");
        wrongOrder = Resources.Load<AudioClip>("Sound/submit(wrong)");
    }

//	public void setTextString (string text) {
//		submissionText = text;
//	}
//
//	public string getTextString () {
//		return submissionText;
//	}
//
//	public void setWinString (string text) {
//		winText = text;
//	}
//
//	public string getWinString () {
//		return winText;
//	}


	void OnTriggerEnter (Collider other) {
		// Disregard any collisions that aren't with the burrito
		GameObject burrito = other.gameObject;
		if (burrito.tag != "Player") {
			return;
		}

		// Prevent submission if we haven't caught any objects.
		if (burrito.GetComponent<ObjectCatcher> ().IsEmpty ()) {
			return;
		}

		SubmitBurrito (burrito);

		if (GameController.instance.levelComplete) {
			SpawnController.instance.DestroyBurrito ();
		}
	}

	/** Submits a burrito */
	void SubmitBurrito (GameObject burrito) {
		Debug.Log ("Submitted a burrito with contents: " + burrito.GetComponent<ObjectCatcher> ().CaughtObjectsToString ());

		// Get a reference to the caught ingredients
		burritoCaughtIngredients = GameController.instance.player.GetComponent<ObjectCatcher> ().GetIngredients ();

		// Logic regarding ordering system.
		List<Order> orders = OrderController.instance.orderList;
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
            audSrc.PlayOneShot(wrongOrder);
			Debug.Log("Submitted burrito does not match");
			OrderUI.instance.setGeneralMessage ("Complete an order first! (Press T to empty)");
					//setTextString ("Invalid Burrito Submission");
            LoggingManager.instance.RecordEvent(1, "Submitted ingredients: " + GameController.instance.player.GetComponent<ObjectCatcher>().GetIngredients().ToString()
            + ". Did not match.");
        }

		// Update UI
		//OrderUI.instance.setMessageHUDMessage (getTextString());
		//OrderUI.instance.setWinMessage (getWinString());
	}

	void ProcessSuccessfulOrderSubmission(Order order) {
		audSrc.PlayOneShot(rightOrder);
		Debug.Log ("Matches one of the orders!");

		// Add the score
		int score = burritoCaughtIngredients.getSumOfQualities ()*50;
		GameController.instance.score += score;
		CreateScorePopup (score);
		Debug.Log("You just got "+score+" score!");
		Debug.Log("Total Score: "+GameController.instance.score);

		// Log the successful submission
		LoggingManager.instance.RecordEvent(2, "Submitted ingredients: " + GameController.instance.player.GetComponent<ObjectCatcher>().GetIngredients().ToString()
			+ ". Gained score: " + score);

		// Fulfill the order and empty burrito
		OrderController.instance.FulfillOrder (order);
		GameController.instance.player.GetComponent<ObjectCatcher> ().GetIngredients ().Empty();

		// Determine if we won the level
		if (OrderController.instance.orderList.Count != 0){
			OrderUI.instance.setGeneralMessage("Matches one of the orders!");
			Debug.Log ("Remaining " + OrderController.instance.OrderListToString ()); // print remaining orders
		} else {
			ProcessLevelWin ();
		}
	}

	void ProcessLevelWin() {
		Debug.Log ("All orders completed");
		OrderUI.instance.gameobjectfields.WinScreen.gameObject.SetActive (true);
		OrderUI.instance.setWinMessage ("You Win! Score: " + GameController.instance.score + "\n(Press escape to return to menu)\n(Press enter to go to next level)");
		OrderUI.instance.setScore (GameController.instance.score.ToString ());
		LoggingManager.instance.RecordEvent (7, "Level quit, timer at " + GameController.instance.gameTime);
		LoggingManager.instance.RecordEvent (8, "Won level, timer at " + GameController.instance.gameTime);
		GameController.instance.levelEnd = true;
		GameController.instance.levelComplete = true;
		// save level
		SaveManager.instance.ProcessLevelCompletion(GameController.instance.currentLevel, GameController.instance.score);
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
