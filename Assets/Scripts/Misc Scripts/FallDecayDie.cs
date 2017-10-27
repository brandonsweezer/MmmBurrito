using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDecayDie : MonoBehaviour {

	private bool decaying;

	public int startingQualityLevel = 3;

	// Number of seconds it takes to decrease one quality level
	public float decayRate;

	// Fall speed vars
	private static float slowFallSpeed = 7f;
	private static float fastFallSpeed = 25f;
	private static float playerMaxSpeedForFastFall = 3f;

	private int qualityLevel;
	private bool slowFalling;
	private Rigidbody rb;

	// Must be Awake, not Start, since we'll be instantiating prefabs with this script on them that
	// will have subsequent calls that we don't want to override with this function's code (ex: 
	// instantiating an ingredient of a particular quality level)
	void Awake () {
		decaying = false;
		qualityLevel = startingQualityLevel;

		// Only enable first child (which should be the freshest model/material).
		for (int i = 1; i < transform.childCount; i++) {
			transform.GetChild (i).gameObject.SetActive(false);
		}

		// Disable gravity for constant fall speed.
		rb = GetComponent<Rigidbody>();
		rb.useGravity = false;
		slowFalling = true;
	}

    public int getQuality(){
        return qualityLevel;
    }

	void FixedUpdate() {
		if (slowFalling) {
			float fallSpeed = slowFallSpeed;
			// Increase the fall speed if the burrito is on the indicator and barely moving.
			if (GetComponent<IngredientIndicator> () != null && GetComponent<IngredientIndicator> ().indicator != null) {
				Vector3 playerPos = GameController.instance.player.transform.position;
				Vector3 indicatorPos = GetComponent<IngredientIndicator> ().indicator.transform.position;
				float playerSpeed = GameController.instance.player.GetComponent<Rigidbody> ().velocity.magnitude;
				if (Vector3.Distance (playerPos, indicatorPos) <= TiledFloor.tileHeight/2 * 1.414f && playerSpeed <= playerMaxSpeedForFastFall) {
					fallSpeed = fastFallSpeed;
				}
			}

			rb.velocity = new Vector3 (rb.velocity.x, -fallSpeed, rb.velocity.z);
		}
	}

	// Start decaying after hitting something
	void OnCollisionEnter(Collision col) {
		DisableSlowFall ();
	}

	// Sets the quality level to a particular value, and enables the corresponding child model
	public void SetQualityLevel(int newQualityLevel) {
		qualityLevel = newQualityLevel;

		// update displayed model
		if (qualityLevel <= 0 || transform.childCount == 1) {
			return;
		}
		foreach (Transform child in transform) {
			child.gameObject.SetActive (false);
		}
		int newChildIndex = (int)Mathf.Min (transform.childCount-1, startingQualityLevel - qualityLevel);
		Transform childToActivate = transform.GetChild(newChildIndex);
		childToActivate.gameObject.SetActive (true);
	}

	IEnumerator Decay () {
        GameController.instance.objects.Add(gameObject);
        while (qualityLevel > 0) {
			yield return new WaitForSeconds (decayRate);
			SetQualityLevel(qualityLevel-1);
		}
        GameController.instance.objects.RemoveAt(0);
		Destroy (gameObject);
	}

	public void DisableSlowFall() {
		slowFalling = false;

		if (!decaying) {
			decaying = true;
			StartCoroutine (Decay ());
			RemoveIngredientIndicator ();
		}

		// re-enable gravity
		GetComponent<Rigidbody>().useGravity = true;
	}

	public void RemoveIngredientIndicator() {
		if (GetComponent<IngredientIndicator> () != null) {
			GetComponent<IngredientIndicator> ().DestroyIndicator ();
		}
	}
}