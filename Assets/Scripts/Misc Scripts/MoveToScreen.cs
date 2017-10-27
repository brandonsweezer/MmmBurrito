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
	private float movementTime = 0.6f;

	// Scaling at the start and at the end of the movement
	private static Vector3 startScale = new Vector3(1.1f, 1.1f, 1.1f);
	private static Vector3 endScale = new Vector3(0.4f, 0.4f, 0.4f);

	private Rigidbody rb;

	private bool movingToScreen;
	private bool destroyWhenComplete;
	private Vector3 initialPos;
	private Camera mainCam;
	private Vector3 targetScreenPoint;
	private float movementStartTime;
	private MoveType moveType;

	// Use this for initialization
	void Start () {
		mainCam = Camera.main;
		rb = GetComponent<Rigidbody> ();
	}

	public void StartMovingToScreenBottom(bool destroyWhenComplete, MoveType moveType = MoveType.Linear) {
		Vector3 screenPos = new Vector3 (mainCam.pixelWidth * 0.2f, 0, 3);
		StartMovingToScreen (destroyWhenComplete, moveType, screenPos);
	}

	public void StartMovingToScreenTopRight(bool destroyWhenComplete, MoveType moveType = MoveType.Linear) {
		Vector3 screenPos = new Vector3 (mainCam.pixelWidth, mainCam.pixelHeight, 3);
		StartMovingToScreen (destroyWhenComplete, moveType, screenPos);
	}

	public void StartMovingToScreen(bool destroyWhenComplete, MoveType moveType = MoveType.Smoothed, Vector3 targetScreenPoint = new Vector3()) {
		this.moveType = moveType;
		this.destroyWhenComplete = destroyWhenComplete;
		this.targetScreenPoint = targetScreenPoint;
		movementStartTime = Time.time;
		initialPos = transform.position;
		movingToScreen = true;

		// disable physics
		rb.angularVelocity = Vector3.zero;
		rb.useGravity = false;
		rb.isKinematic = false;
	}

	// WE'RE USING PIXELS INSTEAD OF WORLD UNITS WHEN SPECIFYING SCREEN POS!!!!


	void FixedUpdate() {
		rb.angularVelocity = Vector3.zero;
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
				transform.position = Vector3.Lerp (initialPos, targetPos, progress);
				transform.localScale = Vector3.Lerp(startScale, endScale, progress);
			}
		}
	}
}
