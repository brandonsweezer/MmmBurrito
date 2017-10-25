using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnPlayerCollision : MonoBehaviour {

	void OnCollisionEnter (Collision collision) {
		GameObject gameObj = collision.gameObject;
		if (gameObj.tag == "Player") {
			Destroy (gameObject);
		}
	}
}
