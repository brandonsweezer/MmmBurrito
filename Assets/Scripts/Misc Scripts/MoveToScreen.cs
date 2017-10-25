using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToScreen : MonoBehaviour {

	// How to move.
	public enum MoveType {
		Linear,
		Smoothed
	}

	private static float smoothingEndBiasFactor = 0.4f;

	// Duration of the movement to the screen location in seconds
	private static float movementTime = 0.6f;

	// Scaling at the start and at the end of the movement
	private static Vector3 startScale = new Vector3(1.1f, 1.1f, 1.1f);
	private static Vector3 endScale = new Vector3(0.15f, 0.15f, 0.15f);

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
		Vector3 screenPos = new Vector3 (-mainCam.pixelHeight * 0.75f, 0, 1f);
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
				// Otherwise move the game object
				// 1. Determine progress based on move type
				switch (moveType) {
					case MoveType.Linear:
						break;
					case MoveType.Smoothed:
						progress = Mathf.Lerp (progress, 1, smoothingEndBiasFactor);
						break;
				}
				// 2. Move and scale
				Vector3 targetPos = mainCam.ScreenToWorldPoint (targetScreenPoint);
				targetPos += (initialPos - targetPos).normalized * distanceFromCamera;
				transform.position = Vector3.Lerp (initialPos, targetPos, progress);
				transform.localScale = Vector3.Lerp(startScale, endScale, progress);
			}
		}
	}
}
