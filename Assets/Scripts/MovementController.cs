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
        forward = Vector3.Normalize(transform.forward);
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
            forward = Quaternion.Euler(new Vector3(0, -90, 0)) * transform.right;
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

        acceleration = Vector3.Normalize(vmove + rmove);
        velocity += acceleration;

        //transform.forward = Vector3.Normalize(rmove + vmove);
        
        float startangle = transform.rotation.eulerAngles.y;
        float endangle = transform.rotation.eulerAngles.y + (Input.GetAxis("Horizontal"))*90;
        float turnrate = .05f;
        float newyrot = startangle * (1 - turnrate) + endangle * turnrate;
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, newyrot, transform.rotation.eulerAngles.z));

        Rigidbody rb = transform.GetComponent<Rigidbody>();
        rb.AddForce(acceleration * 15);
        //rb.MoveRotation(Quaternion.Euler(new Vector3(0, 90, 0)));
        
        

    }

	public bool getMovement() {
		return ismoving;
	}

}
