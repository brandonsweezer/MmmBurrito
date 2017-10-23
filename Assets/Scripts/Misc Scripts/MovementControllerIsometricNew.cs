using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControllerIsometricNew : MonoBehaviour {

	// Speed vars
	private static float maxSpeed = 13f;
	private static float dashSpeed = 38f;
	private static float dashBoostDistance = 0.7f;
	private static float dashCooldown = 0.8f; // dash cooldown in seconds.
	private static float dashSlowDownFactor = 0.1f;

	// Control responsiveness vars
	private static float velocityChangeRate = 0.2f;
	private static float additionalTurnRateLerp = 0.2f;

	// Misc. vars
	Rigidbody rb;
	private float horizontalMoveInput;
	private float verticalMoveInput;
	private bool dashInput;
	private float timeOfLastDash;

	// Dash particle system
	public GameObject dashParticleSystem;
	private Vector3 dashParticleSpawnOffset = new Vector3(0, 0, 0);


	void Awake () {
		rb = GetComponent<Rigidbody> ();
	}
	

	void Update () {
		// Detect move input.

		horizontalMoveInput = 0;
		verticalMoveInput = 0;
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			horizontalMoveInput += 1;
		}
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			horizontalMoveInput -= 1;
		}
		if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
			verticalMoveInput += 1;
		}
		if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
			verticalMoveInput -= 1;
		}

		dashInput = false;
		if (Input.GetKey (KeyCode.Space) || Input.GetKey (KeyCode.LeftShift)) {
			dashInput = true;
		}
	}

	void FixedUpdate () {
		rb.angularVelocity = Vector3.zero;
		if (getMovement()) {
			// Move manually using the input and no friction
			ToggleFriction(false);
			ManualMove();
		} else {
			// Let friction slow us down
			ToggleFriction(true);
		}
	}

	// Turns friction on or off
	void ToggleFriction(bool on) {
		if (on) {
			GetComponent<Collider> ().material.dynamicFriction = 0.8f;
			GetComponent<Collider> ().material.staticFriction = 0.8f;
		} else {
			GetComponent<Collider> ().material.dynamicFriction = 0f;
			GetComponent<Collider> ().material.staticFriction = 0f;
		}
	}

	// Set the velocity and facing based on the input alone, with no consideration for friction and forces
	void ManualMove() {
		Vector3 targetDirection = ((horizontalMoveInput * Vector3.right) + (verticalMoveInput * Vector3.forward)).normalized;

		// Set velocity
		if (dashInput && (Time.time - timeOfLastDash) > dashCooldown) {
			Dash(targetDirection);
		} else {
			float currentSpeed = rb.velocity.magnitude;
			if (currentSpeed > maxSpeed) {
				// if going over the max speed, we decrease the speed slower than we turn
				Vector3 newDirection = Vector3.Lerp (rb.velocity.normalized, targetDirection, velocityChangeRate);
				float newSpeed = Mathf.Lerp (currentSpeed, maxSpeed, dashSlowDownFactor);
				rb.velocity = newDirection * newSpeed;
			} else {
				// else we can just take care of it all in one
				rb.velocity = Vector3.Lerp (rb.velocity, targetDirection * maxSpeed, velocityChangeRate);
			}

			// Set the rotation based on the velocity
			if (rb.velocity != Vector3.zero) {
				Vector3 targetFacing = rb.velocity;
				// Always pick the fastest way to turn
				// (ex: moving backwards vs forwards in quick succession should do no rotation)
				if (Vector3.Angle (transform.forward, rb.velocity) > 90f) {
					targetFacing = -targetFacing;
				}
				// Lerp angles (on top of using the lerped velocity)
				transform.forward = Vector3.Lerp (transform.forward, targetFacing, additionalTurnRateLerp);
			}
		}
	}

	// Dashes by boosting the burrito forward a bit and giving it a big velocity.
	void Dash (Vector3 targetDirection) {
		rb.velocity = targetDirection * dashSpeed;
		transform.position = transform.position + targetDirection * dashBoostDistance;
		timeOfLastDash = Time.time;

		LoggingManager.instance.RecordEvent(4, "Dashing");

		// dash particles
		transform.forward = targetDirection;
		Instantiate (dashParticleSystem, transform.position + dashParticleSpawnOffset, transform.rotation);
	}

	// Returns true if we're trying to move with the input
	public bool getMovement() {
		return horizontalMoveInput != 0 || verticalMoveInput != 0;
	}

}
