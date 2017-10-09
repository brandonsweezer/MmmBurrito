using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayAndDie : MonoBehaviour {

	public int startingQualityLevel;
	// Number of seconds it takes to decrease one quality level
	public float decayRate;

	private int qualityLevel;

	// Use this for initialization
	void Start () {
		qualityLevel = startingQualityLevel;
		// only enable first child (which should be the freshest model/material)
		for (int i = 1; i < transform.childCount; i++) {
			transform.GetChild (i).gameObject.SetActive(false);
		}
	}

    public int getQuality(){
        return qualityLevel;
    }

	// Start decaying after hitting something
	void OnCollisionEnter(Collision col) {
		StartCoroutine (Decay ());
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
		Destroy (gameObject);
	}
}
