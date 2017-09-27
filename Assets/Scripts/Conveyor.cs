using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour {

    Vector3 direction;
    float speed;

    // Use this for initialization
    void Start () {
        direction = transform.forward;
        speed = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /** Handle collisions with player objects */
    void OnCollisionStay(Collision collision)
    {
        
        GameObject gameObj = collision.gameObject;
        if (gameObj.tag != "Terrain")
        {
            System.Console.WriteLine("DETECTED");
            Rigidbody rb = gameObj.transform.GetComponent<Rigidbody>();
            rb.position += (speed * direction);
        }

    }

}
