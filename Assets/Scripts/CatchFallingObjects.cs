using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchFallingObjects : MonoBehaviour {

	public bool canCatch;

	void Start () {
		canCatch = true;
	}

	void OnCollisionEnter (Collision collision) {
		if (!canCatch) {
			return;
		}

		GameObject gameObj = collision.gameObject;
		if (gameObj.tag == "FallingObject") {
			Destroy (gameObj);
		}

		/*
		// Create an effect at the location of contact.
		ContactPoint contact = collision.contacts[0];
		// Rotate the object so that the y-axis faces along the normal of the surface
		Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
		Vector3 pos = contact.point;
		Instantiate(explosionPrefab, pos, rot);
		*/
	}
}
