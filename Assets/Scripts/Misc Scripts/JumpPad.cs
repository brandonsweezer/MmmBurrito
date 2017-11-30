using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {

    public float force;
    private int bouncetime;
    private int cooldown;

	// Use this for initialization
	void Start () {
        bouncetime = 0;
        cooldown = 50;
	}

    // Update is called once per frame
    void Update() {
        
	}

    private void FixedUpdate()
    {
        bouncetime -= 1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (bouncetime <= 0)
        {
            string tg = collision.gameObject.tag;
            if (tg == "Player" || tg == "Ingredient")
            {
                bouncetime = cooldown;
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                //rb.position += transform.forward * force;
                rb.velocity += transform.up * force;
                Animator a = GetComponent<Animator>();
                Debug.Log("wtf");
                a.Play("Armature|ArmatureAction");
                

            }
        }
    }

}
