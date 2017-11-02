using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashingController : MonoBehaviour {

	private static Vector3 ingredientSpawnOffset = new Vector3 (0, 0, 2.15f);
	private static Vector3 ingredientSpawnVelocity = new Vector3(0, 6, 2);

	void Update () {
		if (Input.GetKeyDown(KeyCode.T)) {
            LoggingManager.instance.RecordEvent(3, "Trashed ingredients with T: " + GameController.instance.player.GetComponent<ObjectCatcher>().GetIngredients().ToString());

            ThrowOutContents();

			// Updates whether we can submit successfully or not
			GameController.instance.UpdateSubmissionValidity();

			// Update UI
			/*OrderUI.instance.ResetAfterDeath();
			OrderUI.instance.CollectionUIUpdate ();*/
			OrderUI.instance.setGeneralMessage ("Burrito Trashed");

		}
	}

	// Spawn the ingredients around the burrito and empty the contents
	void ThrowOutContents() {
		CaughtIngredientSet caughtIngredients = GameController.instance.player.GetComponent<ObjectCatcher> ().GetIngredients ();
		int numIngredients = caughtIngredients.ingredientSet.GetFullCount ();
		if (numIngredients == 0) {
			return;
		}


		// Spawn all the ingredients around the burrito
		float angleBetweenSpawns = 360 / numIngredients;
		for (int i = 0; i < numIngredients; i++) {
			IngredientSet.Ingredients ingredientType;
			int quality;
			caughtIngredients.GetNthIngredient (i, out ingredientType, out quality);

			// spawn
			Quaternion spawnRotation = Quaternion.AngleAxis (angleBetweenSpawns * i, Vector3.up);
			Vector3 spawnLocation = transform.position + spawnRotation * ingredientSpawnOffset;
			GameObject obj = Instantiate (IngredientSet.GetPrefab(ingredientType), spawnLocation, Quaternion.identity) as GameObject;
			FallDecayDie fallScript = obj.GetComponent<FallDecayDie> ();

			// give velocity
			fallScript.DisableSlowFall ();
			fallScript.RemoveIngredientIndicator();
			obj.GetComponent<Rigidbody> ().velocity = spawnRotation * ingredientSpawnVelocity;

			// set quality
			fallScript.SetQualityLevel (quality);
		}

		caughtIngredients.Empty();
	}
}
