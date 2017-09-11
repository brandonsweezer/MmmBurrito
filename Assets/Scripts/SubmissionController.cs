using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmissionController : MonoBehaviour {

	public GameObject burritoPrefab;
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
	}
}
