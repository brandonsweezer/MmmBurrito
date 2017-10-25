using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulnerableToHazards : MonoBehaviour {

	private static float invulnerableColorPeriod = 0.3f;
	private static Color invulnerableStartColor = Color.red;
	private static Color invulnerableEndColor = Color.green;

	private float invulnerableTimeLeft;
	private Renderer burritoRenderer;

	void Start () {
		invulnerableTimeLeft = 0;
		burritoRenderer = gameObject.transform.GetChild (0).GetComponent<Renderer> ();
	}

	void Update() {
		invulnerableTimeLeft -= Time.deltaTime;

		// Change visual if invulnerable
		if (invulnerableTimeLeft > 0) {
			float lerp = Mathf.PingPong (Time.time, invulnerableColorPeriod) / invulnerableColorPeriod;
			Color invulnerableColor = Color.Lerp (invulnerableStartColor, invulnerableEndColor, lerp);
			burritoRenderer.material.SetColor ("_Color", invulnerableColor);
		} else {
			burritoRenderer.material.SetColor ("_Color", Color.white);
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
			LoggingManager.instance.RecordEvent(5, "Died to a " + gameObj.tag);
		}
	}

	/** Makes this object invulnerable for the specified number of seconds */
	public void SetInvulnerableDuration(float duration) {
		invulnerableTimeLeft = duration;
	}
}
