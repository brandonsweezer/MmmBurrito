using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashIfSubmissionPossible : MonoBehaviour {
	
	void Update () {
		// TODO: think of a better way to do this than in update?
		if (GameController.instance.canSubmit) {
			GetComponent<ColorFlash> ().SetActive (true);
		} else {
			GetComponent<ColorFlash> ().SetActive (false);
		}
	}
}
