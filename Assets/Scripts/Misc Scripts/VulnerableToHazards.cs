using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulnerableToHazards : MonoBehaviour {

	public GameObject submissionPlate;

	/** Handle collisions with deadly hazards */
	void OnCollisionEnter (Collision collision) {
		GameObject gameObj = collision.gameObject;
		if (gameObj.tag == "DeadlyHazard") {
			SpawnController.instance.DestroyAndRespawn ();
		}
	}
}
