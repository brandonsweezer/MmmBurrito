using UnityEngine;
using System.Collections;

public class WalkingChef : MonoBehaviour{
    public Vector2[] coordinates;

    private float tolerance = 0.1f;
    private float rotationSpeedFactor = 0.2f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i<coordinates.Length; i++)
        {
            //probably set velocity and stuff up here
            Vector3 targetDirection = Vector3.Normalize(new Vector3(coordinates[i].x - transform.position.x, 0, coordinates[i].y - transform.position.y));
            transform.forward = Vector3.Lerp(transform.forward, targetDirection, rotationSpeedFactor);

            Rigidbody rb = transform.GetComponent<Rigidbody>();
            rb.velocity = targetDirection*20f;

            //don't know if I need tolerance
            while (transform.position.x > coordinates[i].x+tolerance || transform.position.x < coordinates[i].x-tolerance ||
                transform.position.z > coordinates[i].y+tolerance || transform.position.z < coordinates[i].y-tolerance)
            {
                //not in the correct spot
            }
        }
    }
}
