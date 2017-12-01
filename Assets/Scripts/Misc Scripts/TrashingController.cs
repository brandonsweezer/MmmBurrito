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

	public static TrashingController instance = null;
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (this);
		}
	}


	void Update () {
//		if (GameController.instance.levelComplete) {
//			return;
//		}
		if (GameController.instance.gamestate !=GameController.GameState.Play) {
			return;
		}

		if (Input.GetKeyDown(KeyCode.T)) {
			ThrowOutLast();
		}
	}

	public void ThrowOutLast() {
		LoggingManager.instance.RecordEvent(3, "Threw out last ingredient. Ingredients were: " + GameController.instance.player.GetComponent<ObjectCatcher>().GetIngredients().ToString());

		Undo ();

		SoundController.instance.audSrc.PlayOneShot(SoundController.instance.trash, SoundController.instance.SoundEffectVolume.value);

        // Update UI
        OrderUI.instance.setGeneralMessage ("Tossed Ingredient");
	}

	void Undo() {
		GameObject player = GameController.instance.player;
		CaughtIngredientSet caughtIngredients = player.GetComponent<ObjectCatcher> ().GetIngredients ();
		int numIngredients = caughtIngredients.ingredientSet.GetFullCount ();
		if (numIngredients == 0) {
			return;
		}

		IngredientSet.Ingredients ingredientType;
		int quality;
		caughtIngredients.Undo (out ingredientType, out quality);

		// spawn
		Vector3 spawnDir = GetSpawnDirRandom(70f);
		Vector3 spawnLocation = player.transform.position + spawnDir * ingredientSpawnZOffset + ingredientSpawnAdditionalOffset;
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

	// Spawn the ingredients around the burrito and empty the contents
	public void ThrowOutContents() {
		GameObject player = GameController.instance.player;
		CaughtIngredientSet caughtIngredients = player.GetComponent<ObjectCatcher> ().GetIngredients ();
		int numIngredients = caughtIngredients.ingredientSet.GetFullCount ();
		if (numIngredients == 0) {
			return;
		}


		// Spawn all the ingredients around the burrito
		for (int i = 0; i < numIngredients; i++) {
			IngredientSet.Ingredients ingredientType;
			int quality;
			caughtIngredients.GetNthIngredient (i, out ingredientType, out quality);

			// spawn
			Vector3 spawnDir = GetSpawnDirFullSpread(i, numIngredients);
			Vector3 spawnLocation = player.transform.position + spawnDir * ingredientSpawnZOffset + ingredientSpawnAdditionalOffset;
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

	Vector3 GetSpawnDirRandom(float angleSpread) {
		Vector3 behindBurrito = -GameController.instance.player.GetComponent<MovementControllerIsometricNew> ().GetXZFacing();
		float angle = (float) RandomGenerator.RandomNumberBetween(-angleSpread/2, angleSpread/2);
		return Quaternion.AngleAxis (angle, Vector3.up) * behindBurrito;
	}
}
