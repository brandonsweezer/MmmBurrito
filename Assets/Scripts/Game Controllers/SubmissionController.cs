using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SubmissionController : MonoBehaviour {

	public GameObject burritoPrefab;

	private string submissionText; 
	private string winText; 

    private Dictionary<string, List<int>> burritoIngredients;

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
		if (burrito.GetComponent<ObjectCatcher> ().isEmpty ()) {
			return;
		}

		SubmitBurrito (burrito);

		SpawnController.instance.DestroyAndRespawn ();
	}

	/** Submits a burrito */
	void SubmitBurrito (GameObject burrito) {
		Debug.Log ("Submitted a burrito with contents: " + burrito.GetComponent<ObjectCatcher> ().CaughtObjectsToString ());

		// Logic regarding ordering system.
		Dictionary<Order, int> orders = OrderController.instance.orderList;
        List<Order> keys = new List<Order>(orders.Keys);
		bool matched = false;
		foreach (Order key in keys) {
			if (compareBurrito (key)) {
				//MATCHES
				matched = true;
				Debug.Log ("Matches one of the orders!");
				setTextString ("Matches one of the orders!");
                int score = 0;
                foreach (KeyValuePair<string, List<int>> pair in burritoIngredients){
                    foreach (int quality in pair.Value) {
                        score += quality;
                    }
                }
                Debug.Log("You just got "+score*100+" score!");
                if (orders[key] == 1) {
					orders.Remove (key);
                    if (orders.Count == 0){
                        Debug.Log("All orders completed");
						setWinString ("All orders completed");

                        //Create GoToWinScreen instead?
                        GameController.instance.GetComponent<LevelLoader>().GoToMenu();
                    }
                } 
				else {
					OrderController.instance.orderList[key] = orders[key] - 1;
				}

				Debug.Log ("Remaining " + OrderController.instance.OrderListToString ()); // print remaining orders
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

	bool compareBurrito(Order o){
		burritoIngredients = GameController.instance.player.GetComponent<ObjectCatcher>().getIngredients();
		Dictionary<string, int> orderIngredients = o.ingredients;
		if (burritoIngredients.Count != orderIngredients.Count) {
			return false;
		}
		foreach (KeyValuePair<string, List<int>> pair in burritoIngredients) {
			int temp;
			if (!orderIngredients.TryGetValue (pair.Key, out temp) || temp != pair.Value.Count) {
                // Debug.Log(pair.Key);
                // Debug.Log(temp);
				return false;
			}
		}
		return true;
	}
}
