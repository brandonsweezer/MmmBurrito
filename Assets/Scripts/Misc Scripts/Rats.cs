﻿using UnityEngine;
using System.Collections;

public class Rats : MonoBehaviour
{ 
    public int spawnTimer;
    public Vector3 spawnPoint;
    public float tolerance = 5f;

    private int currentTimer;
    private float rotationSpeedFactor = 0.2f;
    private GameObject target = null;
    
    private float tolerance2 = 0.2f;

    //if false, return home
    private bool chase;

    // Use this for initialization
    void Start()
    {
        currentTimer = spawnTimer*60;
        chase = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.instance.levelComplete)
        {
            return;
        }
        if (currentTimer < 0)
        {
            if (chase)
            {
                if (target == null || !target.Equals(GameController.instance.player))
                {
                    //check if player is nearby
                    if (transform.position.x - GameController.instance.player.transform.position.x < tolerance &&
                        transform.position.x - GameController.instance.player.transform.position.x > tolerance * -1 &&
                        (transform.position.z - GameController.instance.player.transform.position.z < tolerance &&
                        transform.position.z - GameController.instance.player.transform.position.z > tolerance * -1))
                    {
                        target = GameController.instance.player;
                    }
                }
                if (target == null)
                {
                    //check for ingredients
                    foreach (GameObject o in GameController.instance.objects)
                    {
                        if (transform.position.x - o.transform.position.x < tolerance &&
                        transform.position.x - o.transform.position.x > tolerance * -1 &&
                        (transform.position.z - o.transform.position.z < tolerance &&
                        transform.position.z - o.transform.position.z > tolerance * -1))
                        {
                            target = o;
                            break;
                        }
                    }
                }
            }
            if (target != null)
            {
                //do pathfinding
                Vector3 targetDirection = Vector3.Normalize(new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z-transform.position.z));
                transform.forward = Vector3.Lerp(transform.forward, targetDirection, rotationSpeedFactor);

                Rigidbody rb = transform.GetComponent<Rigidbody>();
                rb.velocity = targetDirection * 5f;

                if (transform.position.x < spawnPoint.x + tolerance2 && transform.position.x > spawnPoint.x - tolerance2 &&
                    transform.position.z < spawnPoint.z + tolerance2 && transform.position.z > spawnPoint.z - tolerance2 && !chase)
                {
                    transform.position = new Vector3(1000, 0, 1000);
                    rb.velocity = new Vector3();
                    target = null;
                    chase = true;
                    currentTimer = spawnTimer*60;
                }
            }
        }
        else if(currentTimer == 0)
        {
            //spawnRat
            transform.position = spawnPoint;
            currentTimer = -1;
        }
        else
        {
            currentTimer--;
        }
    }

    void OnCollisionEnter (Collision collision)
    {
        GameObject gameObj = collision.gameObject;
        if(gameObj.tag == "Player")
        {
            SpawnController.instance.DestroyAndRespawn();
            chase = false;
            target = new GameObject();
            target.transform.position = spawnPoint;
            LoggingManager.instance.RecordEvent(13, "Died to a rat");
        }
        else if(gameObj.tag == "FallingObject")
        {
            gameObj.GetComponent<Shrink>().StartShrink();
            chase = false;
            target = new GameObject();
            target.transform.position = spawnPoint;
            LoggingManager.instance.RecordEvent(14, "Rat stole a " + gameObj.name);
        }
    }
}
