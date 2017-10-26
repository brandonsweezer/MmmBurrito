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

		// Change visual if invulnerable
		if (invulnerableTimeLeft <= 0) {
			StopFlashing ();
		}
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
            if(gameObj.tag == "DeadlyHazard")
            {
                LoggingManager.instance.RecordEvent(12, "Died to a " + gameObj.tag);
            }
            else
            {
                LoggingManager.instance.RecordEvent(11, "Died to a " + gameObj.tag);
            }
        }
	}

	/** Makes this object invulnerable for the specified number of seconds */
	public void SetInvulnerableDuration(float duration) {
		invulnerableTimeLeft = duration;
		if (duration > 0) {
			StartFlashing ();
		}
	}

	void StartFlashing() {
		setToFlashRecursive (transform, true);
	}
	void StopFlashing() {
		setToFlashRecursive (transform, false);
	}

	void setToFlashRecursive(Transform transf, bool active) {
		// Change this transform's flash active state
		ColorFlash flashScript = transf.gameObject.GetComponent<ColorFlash> ();
		if (flashScript != null) {
			flashScript.SetActive (active);
		}
		// Do the same for each of its children
		foreach (Transform child in transf) {
			setToFlashRecursive (child, active);
		}
	}
}
