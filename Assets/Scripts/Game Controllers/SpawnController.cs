using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

	public GameObject burritoPrefab;

	// Burrito spawn location
	public GameObject spawnPoint;


	// Make this class a singleton
	public static SpawnController instance = null;
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (this);
		}
	}

	// Destroy the current burrito and spawn a new one.
	public void DestroyAndRespawn () {
		DestroyBurrito ();
		SpawnBurrito ();
		// Updates whether we can submit successfully or not
		GameController.instance.UpdateSubmissionValidity();
	}

	public void DestroyBurrito() {
		Destroy (GameController.instance.player);
	}

	// Spawns a burrito at the spawn point
	public void SpawnBurrito () {
		// find spawn point if not already defined
		if (spawnPoint == null) {
			Debug.LogError ("Invalid Level: no burrito spawn point found");
			return;
		}

		Vector3 spawnPosition = spawnPoint.transform.position + new Vector3 (0, 1, 0);
		GameObject newBurrito = Instantiate (burritoPrefab, spawnPosition, Quaternion.identity) as GameObject;
		newBurrito.tag = "Player";

		// set reference in GameController
		GameController.instance.player = newBurrito;
	}
}
