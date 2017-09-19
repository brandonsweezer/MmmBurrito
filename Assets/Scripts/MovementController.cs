using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {

    private Vector3 forward, right, velocity, acceleration;
    private float accelspeed, frc;
	private bool ismoving;
	private bool isUnwrapped;

    // Use this for initialization
    void Start () {
        print(transform.rotation);
        forward = Vector3.Normalize(new Vector3(45,0,45));
        right = Quaternion.Euler(new Vector3(0, 90, 0))*forward;
        velocity = new Vector3(0, 0, 0);
        acceleration = new Vector3(0, 0, 0);
        accelspeed = .2f;
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
            velocity = new Vector3(0, 0, 0);
            ismoving = true;
        }
           
        Vector3 rmove = right * Time.deltaTime * Input.GetAxis("Horizontal");
        Vector3 vmove = forward * Time.deltaTime * Input.GetAxis("Vertical");

        acceleration = Vector3.Normalize(rmove + vmove);
        velocity += acceleration;

        transform.forward = Vector3.Normalize(rmove + vmove);
        transform.rotation *= Quaternion.Euler(new Vector3(1, 1, 90));

        Rigidbody rb = transform.GetComponent<Rigidbody>();
        rb.AddForce(acceleration * 15);
        //rb.MoveRotation(Quaternion.Euler(new Vector3(0, 90, 0)));
        
        

    }

	public bool getMovement() {
		return ismoving; 
	}

}
