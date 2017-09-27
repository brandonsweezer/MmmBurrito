using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayAndDie : MonoBehaviour {

	public int startingQualityLevel;
	// Number of seconds it takes to decrease one quality level
	public float decayRate;

	private int qualityLevel;

	// Use this for initialization
	void Start () {
		qualityLevel = startingQualityLevel;
	}

    public int getQuality(){
        return qualityLevel;
    }

	// Start decaying after hitting something
	void OnCollisionEnter(Collision col) {
		StartCoroutine (Decay ());
	}

	IEnumerator Decay () {
		while (qualityLevel > 0) {
			yield return new WaitForSeconds (decayRate);
			qualityLevel--;
		}
		Destroy (gameObject);
	}
}
