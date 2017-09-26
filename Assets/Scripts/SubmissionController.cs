using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmissionController : MonoBehaviour {

	public GameObject burritoPrefab;
	public GameObject gameControllerObject;
    private GameController gameController;
    private Dictionary<string, List<int>> burritoIngredients;

    private Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, new Vector3(1, 0, 1));

	void Start () {
		gameController = gameControllerObject.GetComponent<GameController> ();
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

		DestroyAndRespawn ();
	}

	/** Submits a burrito */
	void SubmitBurrito (GameObject burrito) {
		Debug.Log ("Submitted a burrito with contents: " + burrito.GetComponent<ObjectCatcher> ().CaughtObjectsToString ());
		// TODO: Add logic regarding ordering system
		Dictionary<Order, int> orders = gameController.orderList;
        List<Order> keys = new List<Order>(orders.Keys);
		foreach (Order key in keys) {

			if (compareBurrito (key)) {
				//MATCHES
				Debug.Log ("Matches one of the orders!");
                int score = 0;
                foreach (KeyValuePair<string, List<int>> pair in burritoIngredients){
                    foreach (int quality in pair.Value) {
                        score += quality;
                    }
                }
                Debug.Log(score*100);
                if (orders[key] == 1) {
					orders.Remove (key);
                    if (orders.Count == 0){
                        Debug.Log("All orders completed");
                    }
				} 
				else {
					gameController.orderList[key] = orders[key] - 1;
				}
			} 
			else {
				//DOES NOT MATCH
				Debug.Log("Submitted burrito does not match");
			}
		}
	}

	bool compareBurrito(Order o){
        burritoIngredients = gameController.player.GetComponent<ObjectCatcher>().getIngredients();
        Dictionary<string, int> orderIngredients = o.ingredients;
		Debug.Log (burritoIngredients.Count);
		Debug.Log (orderIngredients.Count);
		if (burritoIngredients.Count != orderIngredients.Count) {
			return false;
		}
		foreach (KeyValuePair<string, List<int>> pair in burritoIngredients) {
			int temp;
			if (!orderIngredients.TryGetValue (pair.Key, out temp) || temp != pair.Value.Count) {
                Debug.Log(pair.Key);
                Debug.Log(temp);
				return false;
			}
		}
		return true;
	}

	// Destroy burrito and spawn a new one.
	public void DestroyAndRespawn () {
		Destroy (gameController.player);
		SpawnBurrito ();
	}

	public void SpawnBurrito () {
		Vector3 spawnPosition = gameObject.transform.position;
		spawnPosition.y += 1;
		GameObject newBurrito = Instantiate (burritoPrefab, spawnPosition, spawnRotation) as GameObject;
		newBurrito.tag = "Player";
		// set reference in GameController
		gameController.player = newBurrito;
	}
}
