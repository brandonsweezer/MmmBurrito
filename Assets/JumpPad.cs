using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {

    public float force;
    private int cooldown;
    private bool on;

	// Use this for initialization
	void Start () {
        cooldown = 0;
        on = true;
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

    private void OnCollisionEnter(Collision col)
    {
        if (on)
        {
            string tag = col.gameObject.tag;
            if (tag == "Player")
            {
                Vector3 direction = transform.forward * force;
                GameController.instance.player.GetComponent<MovementControllerIsometricNew>().Bounce(direction);
                on = false;
                cooldown = 48;
                Animator a = GetComponent<Animator>();
                a.Play(0, -1, 0);
                var sc = SoundController.instance;
                sc.audSrc.PlayOneShot(sc.spring, sc.SoundEffectVolume.value);
            }
        }
    }
}
