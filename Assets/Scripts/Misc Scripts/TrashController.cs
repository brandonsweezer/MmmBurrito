using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashController : MonoBehaviour {

	private Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, new Vector3(1, 0, 1));
	public GameObject submissionPlate;

	void OnCollisionEnter (Collision collision) {
		// Disregard any collisions that aren't with the burrito
		GameObject burrito = collision.gameObject;
		if (burrito.tag != "Player") {
			return;
		}

		// Destroy burrito and spawn a new one.
		LoggingManager.instance.RecordEvent(3, "Trashed ingredients with trashcan: " + GameController.instance.player.GetComponent<ObjectCatcher>().GetIngredients().ToString());
		SpawnController.instance.DestroyAndRespawn ();
		//OrderUI.instance.ResetAfterDeath ();
	}
}
