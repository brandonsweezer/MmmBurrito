using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulnerableToHazards : MonoBehaviour {

	public GameObject submissionPlate;

	/** Handle collisions with deadly hazards */
	void OnCollisionEnter (Collision collision) {
		GameObject gameObj = collision.gameObject;
		if (gameObj.tag == "DeadlyHazard" || gameObj.tag == "Chef") {
			SpawnController.instance.DestroyAndRespawn ();
			OrderUI.instance.TicketUpdate ();
            LoggingManager.instance.RecordEvent(5, "Died to a " + gameObj.tag);
		}
	}
}
