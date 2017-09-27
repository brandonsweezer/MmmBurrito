using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControllerIsometric : MonoBehaviour {

	private Vector3 forward, right, velocity, acceleration;
	private float frc;
	private bool ismoving;
	private bool isUnwrapped;

	private float accelSpeed = 20f;
	private float rotationSpeedFactor = 0.2f;
	private float maxSpeed = 10f;
	private float slowDownFactor = 0.962f;

	private Vector3 targetDirection;

	// Use this for initialization
	void Start () {
		forward = Vector3.Normalize(transform.forward);
		right = Quaternion.Euler(new Vector3(0, 90, 0))*forward;
		velocity = new Vector3(0, 0, 0);
		acceleration = new Vector3(0, 0, 0);
		ismoving = false;
		isUnwrapped = false;
	}

	// Update is called once per frame
	void Update () {
		// only execute if a key is being pressed
		if (Input.anyKey) {
			Move ();
			// Wrap up burrito
			isUnwrapped = false;
		} else {
			ismoving = false;
			// Unwrap burrito
			isUnwrapped = true;
		}
		// Enable/disable catching falling objects based on wrapped state
		GetComponent<ObjectCatcher> ().canCatch = isUnwrapped;  
	}

	void Move()
	{
		if (ismoving == false)
		{
			ismoving = true;
		}

		Vector3 rmove = right * Input.GetAxis("Horizontal");
		Vector3 vmove = forward * Input.GetAxis("Vertical");

		targetDirection = Vector3.Normalize(rmove + vmove);
	}

	void FixedUpdate() {
		if (ismoving) {
			// Determine new rotation.
			// (Found a real easy way to set the forward so we rotate in both the X and Y axis)
			Vector3 newRight = Vector3.Cross (Vector3.up, targetDirection);
			transform.forward = Vector3.ProjectOnPlane (transform.forward, newRight);

			// Velocity changes.
			Rigidbody rb = transform.GetComponent<Rigidbody> ();
			float currentSpeed = rb.velocity.magnitude;
			if (currentSpeed > maxSpeed) {
				// Cap the velocity if we're going too fast.
				float factor = maxSpeed / currentSpeed;
				rb.velocity.Scale (new Vector3 (factor, factor, factor));
			} else {
				// Accelerate otherwise.
				rb.AddForce (targetDirection * accelSpeed);
			}
		} else {
			SlowDown ();
		}
	}

	void SlowDown() {
		Rigidbody rb = transform.GetComponent<Rigidbody>();
		rb.velocity = new Vector3(rb.velocity.x*slowDownFactor, rb.velocity.y*slowDownFactor, rb.velocity.z*slowDownFactor);
	}

	public bool getMovement() {
		return ismoving;
	}

}