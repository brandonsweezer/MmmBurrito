using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulnerableToHazards : MonoBehaviour {

	private float invulnerableTimeLeft;

	void Start () {
		invulnerableTimeLeft = 0;
	}

	void Update() {
		invulnerableTimeLeft -= Time.deltaTime;
	}

	/** Handle collisions with deadly hazards */
	void OnCollisionEnter (Collision collision) {
		if (invulnerableTimeLeft > 0) {
			return;
		}

		GameObject gameObj = collision.gameObject;
		if (gameObj.tag == "DeadlyHazard" || gameObj.tag == "Chef") {
			SpawnController.instance.DestroyAndRespawn ();
			OrderUI.instance.ResetAfterDeath ();
			LoggingManager.instance.RecordEvent(5, "Died to a " + gameObj.tag);
		}
	}

	/** Makes this object invulnerable for the specified number of seconds */
	public void SetInvulnerableDuration(float duration) {
		invulnerableTimeLeft = duration;
	}
}
