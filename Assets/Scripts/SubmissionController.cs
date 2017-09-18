using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmissionController : MonoBehaviour {

	public GameObject burritoPrefab;
	public GameObject gameController;
	private Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, new Vector3(1, 0, 1));

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

		// Destroy burrito and spawn a new one.
		Destroy (burrito);
		Vector3 spawnPosition = gameObject.transform.position;
		spawnPosition.y += 1;
		GameObject newBurrito = Instantiate (burritoPrefab, spawnPosition, spawnRotation) as GameObject;
		newBurrito.tag = "Player";
	}

	/** Submits a burrito */
	void SubmitBurrito (GameObject burrito) {
		Debug.Log ("Submitted a burrito with contents: " + burrito.GetComponent<ObjectCatcher> ().CaughtObjectsToString ());
		// TODO: Add logic regarding ordering system
		Dictionary<Order, int> orders = gameController.GetComponent<GameController>().orderList;
		foreach (KeyValuePair<Order, int> kvp in orders) {

			if (compareBurrito (kvp.Key)) {
				//MATCHES
				Debug.Log ("Matches one of the orders!");
				if (kvp.Value == 1) {
					orders.Remove (kvp.Key);
					// TODO: Check, did we win??
				} 
				else {
					gameController.GetComponent<GameController>().orderList[kvp.Key] = kvp.Value - 1;
				}
			} 
			else {
				//DOES NOT MATCH
				Debug.Log("Submitted burrito does not match");
			}
		}
	}

	bool compareBurrito(Order o){
		Dictionary<string, int> burritoIngredients = burritoPrefab.GetComponent<ObjectCatcher> ().getIngredients ();
		Dictionary<string, int> orderIngredients = o.ingredients;
		Debug.Log (burritoIngredients.Count);
		Debug.Log (orderIngredients.Count);
		if (burritoIngredients.Count != orderIngredients.Count) {
			return false;
		}
		foreach (KeyValuePair<string, int> pair in burritoIngredients) {
			int temp;
			if (!orderIngredients.TryGetValue (pair.Key, out temp) || temp != pair.Value) {
				return false;
			}
		}
		return true;
	}
}
