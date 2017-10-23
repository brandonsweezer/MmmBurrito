using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToScreen : MonoBehaviour {

	// How to move.
	public enum MoveType {
		Linear,
		Smoothed
	}

	// Duration of the movement to the screen location in seconds
	private static float movementTime = 0.75f;

	private bool movingToScreen;
	private bool destroyWhenComplete;
	private Vector3 initialPos;
	private Camera mainCam;
	private Vector3 targetScreenPoint;
	private float distanceFromCamera;
	private float movementStartTime;
	private MoveType moveType;

	// Use this for initialization
	void Start () {
		mainCam = Camera.main;
	}

	public void StartMovingToScreenBottom(bool destroyWhenComplete, MoveType moveType = MoveType.Smoothed) {
		Vector3 screenPos = new Vector3 (mainCam.pixelWidth / 2.0f, -mainCam.pixelHeight / 2.0f, 1f);
		StartMovingToScreen (destroyWhenComplete, moveType, screenPos, 0.2f);
	}

	public void StartMovingToScreen(bool destroyWhenComplete, MoveType moveType = MoveType.Smoothed, Vector3 targetScreenPoint = new Vector3(), float distanceFromCamera = 0) {
		this.moveType = moveType;
		this.destroyWhenComplete = destroyWhenComplete;
		this.targetScreenPoint = targetScreenPoint;
		this.distanceFromCamera = distanceFromCamera;
		movementStartTime = Time.time;
		initialPos = transform.position;
		movingToScreen = true;

		// cancel rotation of the game object
		GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
	}


	void FixedUpdate() {
		if (movingToScreen) {
			float progress = (Time.time - movementStartTime) / (movementTime);
			if (progress > 1) {
				movingToScreen = false;
				// Destroy game object if it has reached the target position.
				if (destroyWhenComplete) {
					Destroy (gameObject);
				}
			} else {
				// Otherwise move the game object towards the target position (updated to reflect current camera pos).
				Vector3 targetPos = mainCam.ScreenToWorldPoint (targetScreenPoint);
				targetPos += (initialPos - targetPos).normalized * distanceFromCamera;
				switch (moveType) {
				case MoveType.Linear:
					transform.position = Vector3.Lerp (initialPos, targetPos, progress);
					break;
				case MoveType.Smoothed:
					transform.position = Vector3.Lerp (initialPos, targetPos, progress);
					transform.position = Vector3.Lerp (transform.position, targetPos, 0.4f);
					break;
				}
			}
		}
	}
}
