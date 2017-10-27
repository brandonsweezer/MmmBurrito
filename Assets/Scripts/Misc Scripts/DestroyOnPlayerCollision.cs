using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnPlayerCollision : MonoBehaviour {

	public bool requireInvulnerablePlayer = false;

	void OnCollisionEnter (Collision collision) {
		GameObject gameObj = collision.gameObject;
		if (gameObj.tag == "Player") {
			if (!requireInvulnerablePlayer || gameObj.GetComponent<VulnerableToHazards> ().IsInvulnerable ()) {
				Destroy (gameObject);
			}
		}
	}
}
