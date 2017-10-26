using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupMakeInvulnerable : MonoBehaviour {

	public float invulnerabilityDuration;

	void OnCollisionEnter (Collision collision) {
		GameObject gameObj = collision.gameObject;
		if (gameObj.tag == "Player") {
			gameObj.GetComponent<VulnerableToHazards> ().SetInvulnerableDuration (invulnerabilityDuration);
            LoggingManager.instance.RecordEvent(10, "Powerup: Invulnerable");
        }
    }

}
