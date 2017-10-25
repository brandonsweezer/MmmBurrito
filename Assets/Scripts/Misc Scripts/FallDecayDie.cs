using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDecayDie : MonoBehaviour {

	private bool decaying;

	public int startingQualityLevel;

	// Number of seconds it takes to decrease one quality level
	public float decayRate;

	// Fall speed
	private static float slowFallSpeed = 7f;

	private int qualityLevel;
	private bool slowFalling;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
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
			rb.velocity = new Vector3 (rb.velocity.x, -slowFallSpeed, rb.velocity.z);
		}
	}

	// Start decaying after hitting something
	void OnCollisionEnter(Collision col) {
		if (!decaying) {
			decaying = true;
			StartCoroutine (Decay ());
			if (GetComponent<IngredientIndicator> () != null) {
				GetComponent<IngredientIndicator> ().DestroyIndicator ();
			}
		}

		// re-enable gravity
		GetComponent<Rigidbody>().useGravity = true;
		slowFalling = false;
	}

	// Sets the quality level to a particular value, and enables the corresponding child model
	void SetQualityLevel(int newQualityLevel) {
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
		while (qualityLevel > 0) {
			yield return new WaitForSeconds (decayRate);
			SetQualityLevel(qualityLevel-1);
		}
        GameController.instance.objects.RemoveAt(0);
		Destroy (gameObject);
	}
}
