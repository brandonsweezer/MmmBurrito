using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour {

    Vector3 direction;
    public float speed;
	private float defaultSpeed = 0.2f;
    private Renderer rend;

    // Use this for initialization
    void Start () {
        direction = transform.forward.normalized;
		if (speed == 0) {
			speed = defaultSpeed;
		}
        rend = GetComponent<Renderer>();
        
	}

    private void Update() 
    {
        float offset = Time.time * speed * 5.0f;
        rend.material.mainTextureOffset = new Vector2(0, offset); 
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
