using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    AudioClip pickup;
    AudioClip fall;
    AudioClip dash;
    AudioClip dashAlt;
    AudioClip bump;
    AudioClip trash;
    
    AudioSource audSrc;


	// Use this for initialization
	void Start () {
        pickup = (AudioClip) Resources.Load("Sound/pickup");
        fall = (AudioClip) Resources.Load("Sound/fall");
        bump = (AudioClip) Resources.Load("Sound/bump");
        trash = (AudioClip) Resources.Load("Sound/trash");

        audSrc = gameObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        string colTag = collision.gameObject.tag;
        if (colTag == "Terrain")
        {
            

        }
        if (colTag == "Trash")
        {
            audSrc.PlayOneShot(trash);

        }
        else if (colTag == "FallingObject")
        {
            audSrc.PlayOneShot(pickup);

        }

    }
}
