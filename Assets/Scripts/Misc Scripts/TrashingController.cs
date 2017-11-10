using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashingController : MonoBehaviour {

	// (0, 0, 1) is the direction (before rotation) in which the ingredient is spawning.
	private static Vector3 ingredientSpawnAdditionalOffset = new Vector3(0, 0.8f, 0);
	private static float ingredientSpawnZOffset = 1f;
	private static Vector3 ingredientSpawnAdditionalVelocity = new Vector3(0, 10f, 0);
	private static float ingredientSpawnZVelocity = 3f;
	private static float angleSpread = 200f;

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
		float angleBetweenSpawns = angleSpread / numIngredients;
		for (int i = 0; i < numIngredients; i++) {
			IngredientSet.Ingredients ingredientType;
			int quality;
			caughtIngredients.GetNthIngredient (i, out ingredientType, out quality);

			// spawn
			Vector3 spawnDir = GetSpawnDirPartSpread(i, numIngredients);
			Vector3 spawnLocation = transform.position + spawnDir * ingredientSpawnZOffset + ingredientSpawnAdditionalOffset;
			GameObject obj = Instantiate (IngredientSet.GetPrefab(ingredientType), spawnLocation, Quaternion.identity) as GameObject;
			FallDecayDie fallScript = obj.GetComponent<FallDecayDie> ();

			// give velocity
			fallScript.DisableSlowFall ();
			fallScript.RemoveIngredientIndicator();
			obj.GetComponent<Rigidbody> ().velocity = spawnDir * ingredientSpawnZVelocity + ingredientSpawnAdditionalVelocity;

			// start scaling
			obj.GetComponent<ScaleOverTime> ().StartScaling(0.01f, 1f, 7f);

			// set quality
			fallScript.SetQualityLevel (quality);
		}

		caughtIngredients.Empty();
	}

	Vector3 GetSpawnDirFullSpread(int ingredientNum, int numIngredients) {
		float angleBetweenSpawns = 360f / numIngredients;
		return Quaternion.AngleAxis (angleBetweenSpawns * ingredientNum, Vector3.up) * Vector3.forward;
	}

	Vector3 GetSpawnDirPartSpread(int ingredientNum, int numIngredients) {
		Vector3 behindBurrito = -GameController.instance.player.GetComponent<MovementControllerIsometricNew> ().GetXZFacing();
		float angleBetweenSpawns = angleSpread / Mathf.Lerp(numIngredients, 6f, 0.5f);
		float firstIngredientAngle = -(angleBetweenSpawns * (numIngredients-1) / 2f);
		float angle = firstIngredientAngle + angleBetweenSpawns * ingredientNum;
		return Quaternion.AngleAxis (angle, Vector3.up) * behindBurrito;
	}
}
