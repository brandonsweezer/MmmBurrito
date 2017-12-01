using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {

	private int JUMP_COOLDOWN = 24;

    public float force;
    private int cooldown;
    private bool on;

	// Use this for initialization
	void Start () {
        cooldown = 0;
        on = true;
        Animator a = GetComponent<Animator>();
        a.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (cooldown > 0)
        {
            cooldown -= 1;
        }
        if (cooldown <= 0)
        {
            on = true;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (on)
        {
            string tag = col.gameObject.tag;
            if (tag == "Player")
            {
                Vector3 direction = transform.forward * force;
                GameController.instance.player.GetComponent<MovementControllerIsometricNew>().Bounce(direction);
                on = false;
                cooldown = JUMP_COOLDOWN;
                Animator a = GetComponent<Animator>();
                a.enabled = true;
                a.Play(0, -1, 0);
				SoundController.instance.RequestSpringFX ();
            }
        }
    }
}
