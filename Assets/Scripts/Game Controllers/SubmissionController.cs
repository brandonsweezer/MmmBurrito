using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SubmissionController : MonoBehaviour {

	public GameObject burritoPrefab;

	private string submissionText; 
	private string winText; 

    private CaughtIngredientSet burritoCaughtIngredients;

    private Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, new Vector3(1, 0, 1));

    private AudioSource audSrc;
    private AudioClip rightOrder;
    private AudioClip wrongOrder;


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
                audSrc.PlayOneShot(rightOrder);
				matched = true;
				Debug.Log ("Matches one of the orders!");
				OrderController.instance.FulfillOrder (order);
				if (OrderController.instance.orderList.Count != 0){
					OrderUI.instance.setGeneralMessage("Matches one of the orders!");
							//setTextString ("Matches one of the orders!");
				}
				int score = burritoCaughtIngredients.getSumOfQualities ()*50;
				OrderUI.instance.setQualityMessage("+"+score.ToString());
				Debug.Log("You just got "+score+" score!");

                LoggingManager.instance.RecordEvent(2, "Submitted ingredients: " + GameController.instance.player.GetComponent<ObjectCatcher>().GetIngredients().ToString()
                    + ". Gained score: " + score);

				GameController.instance.score += score;
				Debug.Log("Total Score: "+GameController.instance.score);

                if (OrderController.instance.orderList.Count == 0){
                    Debug.Log("All orders completed");
					OrderUI.instance.gameobjectfields.WinScreen.gameObject.SetActive (true);
					OrderUI.instance.setWinMessage("You Win! Score: "+GameController.instance.score+"\n(Press escape to return to menu)\n(Press enter to go to next level)");
					OrderUI.instance.setScore (GameController.instance.score.ToString());
                    LoggingManager.instance.RecordEvent(7, "Level quit, timer at " + GameController.instance.gameTime);
                    LoggingManager.instance.RecordEvent(8, "Won level, timer at " + GameController.instance.gameTime);
                    GameController.instance.levelEnd = true;
                    GameController.instance.levelComplete = true;
                }
				else {
					Debug.Log ("Remaining " + OrderController.instance.OrderListToString ()); // print remaining orders
				}
				GameController.instance.player.GetComponent<ObjectCatcher> ().GetIngredients ().Empty();
				//OrderUI.instance.ResetAfterDeath();
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
}
