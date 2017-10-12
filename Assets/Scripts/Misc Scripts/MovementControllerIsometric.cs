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
    private int LOGGINGTIMER = 60;
    private int timerVar;

	// Use this for initialization
	void Start () {
		forward = Vector3.Normalize(transform.forward);
		right = Quaternion.Euler(new Vector3(0, 90, 0))*forward;
		velocity = new Vector3(0, 0, 0);
		acceleration = new Vector3(0, 0, 0);
		ismoving = false;
		isUnwrapped = false;
        timerVar = LOGGINGTIMER;
	}

	// Update is called once per frame
	void Update () {
        if (timerVar <= 0)
        {
            LoggingManager.instance.RecordEvent(0, "Coordinates: " + GameController.instance.player.transform.position.x + ", "
                + GameController.instance.player.transform.position.z);
            timerVar = LOGGINGTIMER;
        }
        else
        {
            timerVar--;
        }
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

		Vector3 targetDirection = Vector3.Normalize(rmove + vmove);
		// velocity += acceleration;

		Vector3 currentRotEuler = transform.rotation.eulerAngles;

		// Determine new rotation
		// Approach #1 -- set transform forward (using Vector3 lerping)
		transform.forward = Vector3.Lerp(transform.forward, Vector3.Normalize(rmove + vmove), rotationSpeedFactor);

		// Approach #2 -- break it down into euler angles and lerp the y rotation
		// This mostly works except for one weird case (1 direction out of 4) where it turns
		// awkwardly
		/*
		float currentYRot = currentRotEuler.y;
		float targetYRot = Vector3.Angle (targetDirection, Vector3.forward);
		float newYRot = targetYRot * rotationSpeedFactor + currentYRot * (1 - rotationSpeedFactor);
		Vector3 newRotationEuler = new Vector3(currentRotEuler.x, newYRot, currentRotEuler.z);
		transform.rotation = Quaternion.Euler (newRotationEuler);*/

		Rigidbody rb = transform.GetComponent<Rigidbody>();

		float currentSpeed = rb.velocity.magnitude;
		if (currentSpeed > maxSpeed) {
			// Cap the velocity if we're going too fast.
			float factor = maxSpeed / currentSpeed;
			rb.velocity.Scale (new Vector3 (factor, factor, factor));
		} else {
			// Accelerate otherwise.
			rb.AddForce (targetDirection * accelSpeed);
		}

	}

	public bool getMovement() {
		return ismoving;
	}

}