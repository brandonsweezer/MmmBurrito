using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour {

    Vector3 direction;
    public float speed;
	private float defaultSpeed = 0.2f;

    // Use this for initialization
    void Start () {
        direction = transform.forward;
		if (speed == 0) {
			speed = defaultSpeed;
		}
	}

    /** Handle collisions with player objects */
    void OnCollisionStay(Collision collision)
    {
        GameObject gameObj = collision.gameObject;
		if (gameObj.tag != "Terrain" && gameObj.tag != "SpawnArea")
        {
            Rigidbody rb = gameObj.transform.GetComponent<Rigidbody>();
            rb.position += (speed * direction);
        }

    }

}
