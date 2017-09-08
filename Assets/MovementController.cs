using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {

    private Vector3 forward;
    private Vector3 right;
    private Vector3 velocity;
    private Vector3 acceleration;

    // Use this for initialization
    void Start () {
        forward = Vector3.Normalize(Camera.main.transform.forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0))*forward;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKey) // only execute if a key is being pressed
            Move();
    }
    void Move()
    {
        Vector3 rmove = right * Time.deltaTime * Input.GetAxis("Horizontal");
        Vector3 vmove = forward * Time.deltaTime * Input.GetAxis("Vertical");
        Vector3 heading = Vector3.Normalize(rmove + vmove);

        

    }

}
