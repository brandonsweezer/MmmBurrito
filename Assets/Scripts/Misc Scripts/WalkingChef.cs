using UnityEngine;
using System.Collections;

public class WalkingChef : MonoBehaviour{
    public Vector2[] coordinates;

    public Vector3 spawnPoint;

    public int waitTime;

    private float tolerance = 0.1f;
    private float rotationSpeedFactor = 0.2f;
    private int coordPointer = 0;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTime <= 0)
        {
            if (waitTime == 0)
            {
                transform.position = spawnPoint;
                waitTime--;
            }
            else {
                //probably set velocity and stuff up here
                Vector3 targetDirection = Vector3.Normalize(new Vector3(coordinates[coordPointer].x - transform.position.x, 0, coordinates[coordPointer].y - transform.position.z));
                transform.forward = Vector3.Lerp(transform.forward, targetDirection, rotationSpeedFactor);

                Rigidbody rb = transform.GetComponent<Rigidbody>();
                rb.velocity = targetDirection * 5f;
                Debug.Log(coordinates[coordPointer]);

                //don't know if I need tolerance
                if (transform.position.x < coordinates[coordPointer].x + tolerance && transform.position.x > coordinates[coordPointer].x - tolerance &&
                    transform.position.z < coordinates[coordPointer].y + tolerance && transform.position.z > coordinates[coordPointer].y - tolerance)
                {
                    Debug.Log("hit");
                    coordPointer++;
                }
                if (coordPointer >= coordinates.Length)
                {
                    coordPointer = 0;
                }
            }
        }
        else
        {
            waitTime--;
        }
    }
}
