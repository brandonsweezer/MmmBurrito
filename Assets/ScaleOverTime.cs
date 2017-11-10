using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A class to animate uniform scaling (using standard lerp easing).
public class ScaleOverTime : MonoBehaviour {

	private bool active;
	private Vector3 targetScale = new Vector3();
	private float scaleSpeedFactor;

	void Update () {
		// Change scale if active.
		if (active) {
			Vector3 newScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeedFactor * Time.deltaTime);
			transform.localScale = newScale;
			if (targetScale.x - transform.localScale.x < 0.1f) {
				active = false;
				transform.localScale = targetScale;
			}
		}
	}

	// See specification for the other StartScaling().
	public void StartScaling(float targetScale, float scaleSpeedFactor) {
		this.targetScale.Set (targetScale, targetScale, targetScale);
		this.scaleSpeedFactor = scaleSpeedFactor;
		active = true;
	}

	// Scales from startingScale to targetScale using the scaleSpeedFactor. 
	// - All scales are in local scale.
	// - scaleSpeedFactor should be positive, but can be greater than 1. A smaller scaleSpeedFactor is a slower change.
	public void StartScaling(float startingScale, float targetScale, float scaleSpeedFactor) {
		transform.localScale = new Vector3 (startingScale, startingScale, startingScale);
		StartScaling (targetScale, scaleSpeedFactor);
	}
}
