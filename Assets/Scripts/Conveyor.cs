using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour {

    Vector3 direction;
    public float speed;

    // Use this for initialization
    void Start () {
        direction = transform.forward;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /** Handle collisions with player objects */
    void OnCollisionStay(Collision collision)
    {
        
        GameObject gameObj = collision.gameObject;
		if (gameObj.tag != "Terrain" && gameObj.tag != "SpawnArea")
        {
            System.Console.WriteLine("DETECTED");
            Rigidbody rb = gameObj.transform.GetComponent<Rigidbody>();
            rb.position += (speed * direction);
        }

    }

}
