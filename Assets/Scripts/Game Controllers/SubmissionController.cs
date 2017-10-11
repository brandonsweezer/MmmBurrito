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



	void Start () {
		setTextString ("");
		setWinString ("");
	}

	public void setTextString (string text) {
		submissionText = text;
	}

	public string getTextString () {
		return submissionText;
	}

	public void setWinString (string text) {
		winText = text;
	}

	public string getWinString () {
		return winText;
	}


	void OnCollisionEnter (Collision collision) {
		// Disregard any collisions that aren't with the burrito
		GameObject burrito = collision.gameObject;
		if (burrito.tag != "Player") {
			return;
		}

		// Prevent submission if we haven't caught any objects.
		if (burrito.GetComponent<ObjectCatcher> ().IsEmpty ()) {
			return;
		}

		SubmitBurrito (burrito);

		SpawnController.instance.DestroyAndRespawn ();
	}

	/** Submits a burrito */
	void SubmitBurrito (GameObject burrito) {
		Debug.Log ("Submitted a burrito with contents: " + burrito.GetComponent<ObjectCatcher> ().CaughtObjectsToString ());

		// Get a reference to the caught ingredients
		burritoCaughtIngredients = GameController.instance.player.GetComponent<ObjectCatcher> ().getIngredients ();

		// Logic regarding ordering system.
		List<IngredientSet> orders = OrderController.instance.orderList;
		bool matched = false;
		foreach (IngredientSet order in orders) {
			if (compareBurrito (order)) {
				//MATCHES
				matched = true;
				Debug.Log ("Matches one of the orders!");
				setTextString ("Matches one of the orders!");
				int score = burritoCaughtIngredients.getSumOfQualities ();
                Debug.Log("You just got "+score*100+" score!");
				orders.Remove (order);
                if (orders.Count == 0){
                    Debug.Log("All orders completed");
					setWinString ("All orders completed");

                    //Create GoToWinScreen instead?
                    GameController.instance.GetComponent<LevelLoader>().GoToMenu();
                }
				else {
					Debug.Log ("Remaining " + OrderController.instance.OrderListToString ()); // print remaining orders
				}
				break;
			}
		} 
		if (!matched) {
			//DOES NOT MATCH
			Debug.Log("Submitted burrito does not match");
			setTextString ("Submitted burrito does not match");
		}

		// Update UI
		OrderUI.instance.setSubmissionMessage (getTextString());
		OrderUI.instance.setWinMessage (getWinString());
	}

	bool compareBurrito(IngredientSet targetOrder){
		IngredientSet burritoIngredients = burritoCaughtIngredients.ingredientSet;
		return burritoIngredients.Equivalent(targetOrder);
	}
}
