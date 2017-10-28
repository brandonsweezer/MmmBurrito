using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashAndStretch : MonoBehaviour {

	private float scaleChangeFactor = 0.5f;

	private float maxStretchScaleAdded = 3f; // must be >= 0
	private float maxSquashScaleAdded = -.5f; // must be <= 0
	private float speedIncreaseFactorForMaxStretch = 3f; // must be >= 0
	private float speedIncreaseFactorForMaxSquash = -5f; // must be <= 0
	private float minSpeedForStretch = 10f;

	private float lastSpeed;
	private Rigidbody rb;

	void Awake() {
		lastSpeed = 0;
		rb = GetComponent<Rigidbody> ();
	}


	void FixedUpdate () {
		float currentSpeed = rb.velocity.magnitude;

		// determine the new scale
		float scaleToAdd = 0;
		if (currentSpeed >= minSpeedForStretch) {
			lastSpeed = Mathf.Max (lastSpeed, 0.001f); // prevent a 0 lastSpeed
			float speedChangeFactor = (currentSpeed - lastSpeed) / lastSpeed;
			if (speedChangeFactor > 0) {
				float lerp = Mathf.Min (1, speedChangeFactor / speedIncreaseFactorForMaxStretch);
				scaleToAdd = Mathf.Lerp (0, maxStretchScaleAdded, lerp);
			} else {
				float lerp = Mathf.Min (1, speedChangeFactor / speedIncreaseFactorForMaxSquash);
				scaleToAdd = Mathf.Lerp (0, maxSquashScaleAdded, lerp);
			}
			scaleToAdd = Mathf.Min (maxStretchScaleAdded, Mathf.Max (maxSquashScaleAdded, scaleToAdd));
		}

		Vector3 scaleToAddVector = Vector3.forward * scaleToAdd;
		Vector3 newTargetScale = new Vector3 (1, 1, 1) + scaleToAddVector;
		transform.localScale = Vector3.Lerp(transform.localScale, newTargetScale, scaleChangeFactor);

		// remember to update the speed!
		lastSpeed = currentSpeed;
	}
}
