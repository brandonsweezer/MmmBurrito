    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControllerIsometric : MonoBehaviour {

	private Vector3 forward, right, velocity, acceleration;
	private float frc;
	private bool ismoving;
	private bool isUnwrapped;

    //Gen movement variables
	private float accelSpeed = 70f;
	private float rotationSpeedFactor = 0.5f;
	private float maxSpeed = 12f;
    private float hackedTurnrate = 0.05f;
	private float speedRestitutionFactor = 0.05f;

    //Dashing variables
    private float dashStamp;
    private bool dashing;
    private float slowdown = 10f; 
    private float maxDash = 40f;
    private float minDash = 15f;
    private float dashCooldown = .6f; //seconds
	private float dashMomentumConservationFactor = 0.2f;

    private int LOGGINGTIMER = 60;
    private int timerVar;

    // Use this for initialization
    void Start ()
    {
		forward = Vector3.Normalize(transform.forward);
		right = Quaternion.Euler(new Vector3(0, 90, 0))*forward;
		velocity = new Vector3(0, 0, 0);
		acceleration = new Vector3(0, 0, 0);
		ismoving = false;
		isUnwrapped = false;
        dashStamp = Time.time;
        dashing = false;
        timerVar = LOGGINGTIMER;
	}

	// Update is called once per frame
	void Update ()
    {
		// Do nothing if level is complete
//		if (GameController.instance.levelComplete) {
//			return;
//		}
		if (GameController.instance.gamestate!=GameController.GameState.Play) {
			return;
		}

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
        if (Input.GetKeyDown(KeyCode.T))
        {
            LoggingManager.instance.RecordEvent(3, "Trashed ingredients with T: " + GameController.instance.player.GetComponent<ObjectCatcher>().GetIngredients().ToString());
            GameController.instance.player.GetComponent<ObjectCatcher>().GetIngredients().Empty();
            //OrderUI.instance.ResetAfterDeath();
        }
        // only execute if a key is being held
        if (Input.anyKey) {
            
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Space))
            { 
                Dash();
                LoggingManager.instance.RecordEvent(4, "Dashing");
            }

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

    //Adds velocity according to your current movement direction, capped at maxDash and minDash
    void Dash()
    {
        float currtime = Time.time;
        float timeSinceDash = currtime - dashStamp;
        Debug.Log(timeSinceDash);
        if (timeSinceDash >= dashCooldown)
        {
            
            //Set dashing and timestamp variables
            dashing = true;
            dashStamp = currtime;

            //Do the dash
            Vector3 rmove = right * Input.GetAxis("Horizontal");
            Vector3 vmove = forward * Input.GetAxis("Vertical");

            Vector3 targetDirection = Vector3.Normalize(rmove + vmove);

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = (dashMomentumConservationFactor * rb.velocity) + targetDirection * maxDash;
            if (rb.velocity.magnitude > maxDash) {
                rb.velocity = targetDirection * maxDash;
            }
            if (rb.velocity.magnitude < minDash)
            {
                rb.velocity = targetDirection * minDash;
            }
        }


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

		// preliminary velocity turning
		Vector3 currentVelocityDir = rb.velocity.normalized;
		if (rb.velocity.sqrMagnitude == 0) {
			currentVelocityDir = transform.forward;
		}
		// have min velocity (i.e. can go up slopes)
		rb.velocity = rb.velocity.magnitude * Vector3.Lerp(currentVelocityDir, targetDirection, hackedTurnrate);

		// lower friction if on slope
		Vector3 groundNormal = Vector3.up;
		RaycastHit hit;
		bool floorInFront = Physics.Raycast (transform.position + rb.velocity.normalized * 0.25f, Vector3.down, out hit, 1f);
		if (floorInFront) {
			groundNormal = hit.normal.normalized;
		}
		if (Mathf.Abs(groundNormal.x) > 0.15f || Mathf.Abs(groundNormal.z) > 0.15f || !floorInFront) {
			// on slope, lower friction so we can go up the slope
			GetComponent<Collider> ().material.dynamicFriction = 0.5f;
			GetComponent<Collider> ().material.staticFriction = 0.5f;
		} else {
			GetComponent<Collider> ().material.dynamicFriction = 3f;
			GetComponent<Collider> ().material.staticFriction = 3f;
		}

		// Accelerate only if not going too fast in the x-z plane
		if (Mathf.Pow(rb.velocity.x,2) + Mathf.Pow(rb.velocity.z,2) < Mathf.Pow(maxSpeed,2)) {
			bool grounded = Physics.Raycast (transform.position, Vector3.down, 1f);
			if (grounded) {
				// if grounded, accelerate normally
				rb.AddForce (targetDirection * accelSpeed);
			} else if (rb.velocity.magnitude < maxSpeed / 3f) {
				// if not grounded and not moving fast, accelerate slowly
				rb.AddForce (targetDirection * accelSpeed / 5f);
			}
		}

	}

	void FixedUpdate() {

		Rigidbody rb = transform.GetComponent<Rigidbody>();
		// cap speed
		float currentSpeed = rb.velocity.magnitude;
		if (currentSpeed > maxSpeed) {
			// If going too fast, slow down
			rb.velocity = Vector3.Lerp(rb.velocity, rb.velocity.normalized * maxSpeed, speedRestitutionFactor);
		}
	}

	public bool getMovement() {
		return ismoving;
	}

}