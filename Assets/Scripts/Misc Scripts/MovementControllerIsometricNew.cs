using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControllerIsometricNew : MonoBehaviour {

    private int deathTimer = 0;
	// Speed vars
	private static float maxSpeed = 13f;
	private static float dashSpeed = 32f;
	private static float dashBoostDistance = 0f;
	private static float dashCooldown = 0.8f; // dash cooldown in seconds.
	private static float dashSlowDownFactor = 0.1f;

	// Control responsiveness vars
	private static float velocityChangeRate;
	private static float velocityChangeRateInAir = 0.15f;
	private static float velocityChangeRateOnGround = 0.2f;
	private static float additionalTurnRateLerp = 0.2f;
	// How closely going straight up the ramp we must be to snap to ramp and get the speed boost.
	// Higher value means we need to go closer to straight up the ramp.
	// Going 45 degrees up the ramp is about 0.707.
	private static float velocityFractionInRampDirectionForSnap = .75f;

	// Dashing boost on ramps vars
	private static float dashSpeedOnRamp = 20f;
	private static float dashDuration = 0.4f; // in seconds
	private static float rampDetectionDistance = 1.5f;
	private static float rampBiasAngle = 10; // After what angle from a flat ground are we considering the ground to be a ramp.
	private static float speedUpRampIncreaseFactor = 0.3f;

	// bouncing/jump pad vars
	private static float bounceInputStun = 0.1f;
	private float timeOfLastBounce = 0f;

	private Vector3 lastVelocity = new Vector3();

	// Rotation of 45 degrees for isommetric view
	private static Quaternion viewpointRotation = Quaternion.AngleAxis (-45, Vector3.up);

	// grounded vars
	private static float maxFloorAngleForGrounding = 40f; // Maximum floor angle for a collision with it to be considered grounding.
	private bool grounded;

	// Misc. vars
	Rigidbody rb;
	private float horizontalMoveInput;
	private float verticalMoveInput;
	private bool dashInput;
	private float timeOfLastDash;
	private Vector3 xzFacing;
    private bool unfolded = false;
	private Animator animator;

	private Collider foldedCollisionBox;
	private Collider unfoldedCollisionBox;

	// Dash particle system
	public GameObject dashParticleSystem;
	private Vector3 dashParticleSpawnOffset = new Vector3(0, 0, 0);

    //Audio vars
    AudioSource audSrc;

    void Awake () {
		rb = GetComponent<Rigidbody> ();
		animator = GetComponent<Animator>();

		foldedCollisionBox = GetComponent<CapsuleCollider> ();
		unfoldedCollisionBox = GetComponent<MeshCollider> ();

		timeOfLastDash = 0f;
		xzFacing = Vector3.forward;
		velocityChangeRate = velocityChangeRateOnGround;

        audSrc = SoundController.instance.audSrc;
    }

    private void Start()
    {
        GetComponent<Animator>().enabled = false;
    }

    // Update the rotation of our movement input to match our camera angle
    // (i.e. pressing the "up" arrow key moves the burrito up on the screen).
    public static void UpdateViewpointRotation() {
		viewpointRotation = Quaternion.FromToRotation (Vector3.forward, Vector3.ProjectOnPlane(Camera.main.transform.up, Vector3.up));
	}

	// Start decaying after hitting something
	void OnCollisionStay(Collision col) {
		Vector3 floorNormal = col.contacts [0].normal;
		if ((col.gameObject.tag == "Terrain" || col.gameObject.tag == "SpawnArea") && Vector3.Angle(floorNormal, Vector3.up) <= maxFloorAngleForGrounding) {
			SetGrounded (true);
		}
	}

	void OnCollisionExit(Collision col) {
		if (col.gameObject.tag == "Terrain" || col.gameObject.tag == "SpawnArea") {
			SetGrounded (false);
		}
	}

	void SetGrounded(bool grounded) {
		this.grounded = grounded;
		if (grounded) {
			velocityChangeRate = velocityChangeRateOnGround;
		} else {
			velocityChangeRate = velocityChangeRateInAir;
		}
	}

    public void Bounce(Vector3 force)
	{
		Vector3 v = force * 7f;
		if (v.y == 0) {
			v.y = 4f;
		}
		rb.velocity = v;
		transform.position += new Vector3 (0, 1, 0);
		timeOfLastBounce = Time.time;
    }

	void Update () {
        
		horizontalMoveInput = 0;
		verticalMoveInput = 0;

		// Only read input if we're still playing the level
//		if (GameController.instance.levelComplete) {
//			return;
//		}
		if (GameController.instance.gamestate!=GameController.GameState.Play) {
			return;
		}
        if (GameController.instance.dead)
        {
            deathTimer++;
            transform.Rotate(new Vector3(0, 20f, 0));
            transform.localScale -= Vector3.one * 0.003f *deathTimer;
            gameObject.GetComponent<Rigidbody>().Sleep();
            gameObject.GetComponent<Rigidbody>().detectCollisions = false;
            if (deathTimer >= 60)
            {
                deathTimer = 0;
                GameController.instance.dead = false;
                SpawnController.instance.DestroyAndRespawn();
            }
            return;
        }

        // Detect move input.
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
		if (Input.GetKey (KeyCode.Space) || Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
			dashInput = true;
		}

		// enable/disable dash particles
		if (!IsDashing ()) {
			GetComponent<SmokeTrail> ().Disable ();
		}
	}

	void FixedUpdate () {
		if (GameController.instance.gamestate != GameController.GameState.Play) {
			if (!rb.IsSleeping ()) {
				if (lastVelocity == Vector3.zero) {
					lastVelocity = rb.velocity;
				}
				rb.Sleep ();
			}
			return;
		}
		if (rb.IsSleeping()) {
			rb.WakeUp ();
			rb.velocity = lastVelocity;
			lastVelocity = Vector3.zero;
			return;
		}

		UpdateXZFacing ();

		rb.angularVelocity = Vector3.zero;

		float currentXZSpeed = Vector3.ProjectOnPlane(rb.velocity, Vector3.up).magnitude;
		if (getMovement()) {
			// Move manually using the input and no friction
			ToggleFriction(false);
			ManualMove();
		} else {
			// Let friction slow us down
			ToggleFriction(true);
		}

		// If in the air, slow down artifically when not pressing buttons
		if (!grounded) {
			ToggleFriction (false);

			if (!getMovement ()) {
				Vector3 newXZVelocity = new Vector3 (rb.velocity.x, 0, rb.velocity.z) * 0.94f;
				rb.velocity = new Vector3 (newXZVelocity.x, rb.velocity.y, newXZVelocity.z);
			}

			Vector3 newXZVelocity2 = new Vector3 (rb.velocity.x, 0, rb.velocity.z).normalized * currentXZSpeed;
			Vector3 newVelocity2 = new Vector3 (newXZVelocity2.x, rb.velocity.y, newXZVelocity2.z);
			rb.velocity = Vector3.Lerp (rb.velocity, newVelocity2, 0.5f);
		}

		IncreaseSpeedDashingUpRamp ();

		HandleFolding ();
	}

	void HandleFolding() {
		animator.enabled = true;

		if (!getMovement()) {
			if (!unfolded) {
				unfolded = true;
				UpdateCollisionBox ();
				animator.SetTrigger("Unwrap");
			}
		}
		else if (unfolded) {
			unfolded = false;
			UpdateCollisionBox ();
			animator.SetTrigger("Roll");
		}

		if (!unfolded) {
			UpdateAnimationDirection ();
		}
	}

	void UpdateCollisionBox() {
		if (unfolded) {
			unfoldedCollisionBox.enabled = true;
			foldedCollisionBox.enabled = false;
		} else {
			unfoldedCollisionBox.enabled = false;
			foldedCollisionBox.enabled = true;
		}
	}

	void UpdateAnimationDirection() {
		Vector3 XZmove = new Vector3 (rb.velocity.x, 0, rb.velocity.z);
		if (Vector3.Dot(XZmove, transform.forward) < 0) {
			animator.SetFloat("Direction", -1.0f);
		}
		if (Vector3.Dot(XZmove, transform.forward) > 0)
		{
			animator.SetFloat("Direction", 1.0f);
		}
	}

	void UpdateXZFacing() {
		Vector3 XZmove = new Vector3 (rb.velocity.x, 0, rb.velocity.z);
		if (XZmove.magnitude > 0.05f) {
			xzFacing = transform.forward;
			if (Vector3.Dot (xzFacing, XZmove) < 0) {
				xzFacing *= -1;
			}
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
		targetDirection = viewpointRotation * targetDirection;
		if (targetDirection == Vector3.zero) {
			targetDirection = xzFacing;
		}

		// Simple dash
		if (dashInput && CanDash()) {
			Dash(targetDirection);
			return;
		}

		// Else set velocity using some calculations
		Vector3 currentXZVelocity = Vector3.ProjectOnPlane(rb.velocity, Vector3.up);
		float currentXZSpeed = currentXZVelocity.magnitude;
		float currentYSpeed = rb.velocity.y;
		if (currentXZSpeed > maxSpeed) {
			// if going over the max speed, we decrease the speed slower than we turn
			Vector3 newXZDirection = Vector3.Lerp (currentXZVelocity.normalized, targetDirection, velocityChangeRate);
			float newSpeed = Mathf.Lerp (currentXZSpeed, maxSpeed, dashSlowDownFactor);
			rb.velocity = newXZDirection * newSpeed;
		} else {
			// else we can just take care of it all in one
			rb.velocity = Vector3.Lerp (currentXZVelocity, targetDirection * maxSpeed, velocityChangeRate);
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

		// Restore y velocity
		rb.velocity = new Vector3(rb.velocity.x, currentYSpeed, rb.velocity.z);
	}

	// Dashes by boosting the burrito forward a bit and giving it a big velocity.
	void Dash (Vector3 targetDirection) {
		audSrc.PlayOneShot(SoundController.instance.dash,SoundController.instance.SoundEffectVolume.value);

		rb.velocity = targetDirection * dashSpeed;
		transform.position = transform.position + targetDirection * dashBoostDistance;
		timeOfLastDash = Time.time;

		LoggingManager.instance.RecordEvent(4, "Dashing");

		// dash particles
		transform.forward = targetDirection;
		Instantiate (dashParticleSystem, transform.position + dashParticleSpawnOffset, transform.rotation);
		GetComponent<SmokeTrail> ().Enable ();
	}

	// Increases the speed up the ramp and snap to the ramp's direction if player is trying to dash up ramp.
	void IncreaseSpeedDashingUpRamp() {
		RaycastHit hit;
		if (IsDashing() && Physics.Raycast (transform.position, Vector3.down, out hit, rampDetectionDistance)) {
			if (Vector3.Angle (hit.normal, Vector3.up) >= rampBiasAngle) {
				// Dashing on a ramp, check if we need to snap to and boost up the ramp
				GameObject ramp = hit.collider.gameObject;
				Vector3 rampDirection = ramp.transform.forward;
				Vector3 currentXZVelocity = Vector3.ProjectOnPlane (rb.velocity, Vector3.up);
				float speedInRampDirection = Vector3.Dot(rampDirection, currentXZVelocity);
				float velocityFractionInRampDirection = speedInRampDirection / currentXZVelocity.magnitude;
				if (velocityFractionInRampDirection >= velocityFractionInRampDirectionForSnap) {
					// Snap and boost up the ramp.
					Vector3 boostDirection = Vector3.Cross(ramp.transform.right, hit.normal);
					rb.velocity = boostDirection * dashSpeedOnRamp;
					transform.forward = rampDirection;
					timeOfLastDash = Time.time;
				}
			}
		}
	}

	// Returns true if we're trying to move with the input
	public bool getMovement() {
		if (InBounceInputStun ()) {
			return false;
		}
		return horizontalMoveInput != 0 || verticalMoveInput != 0 || dashInput;
	}

	bool CanDash() {
		return grounded && (Time.time - timeOfLastDash) > dashCooldown;
	}

	bool IsDashing() {
		return (Time.time - timeOfLastDash < dashDuration);
	}

	bool InBounceInputStun() {
		return (timeOfLastBounce + bounceInputStun) > Time.time;
	}

	public Vector3 GetXZFacing() {
		return xzFacing;
	}

}
