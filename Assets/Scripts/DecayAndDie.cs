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

	// Start decaying only after hitting a static object (i.e. the terrain)
	void OnCollisionEnter(Collision col) {
		if (col.gameObject.isStatic) {
			StartCoroutine (Decay ());
		}
	}

	IEnumerator Decay () {
		while (qualityLevel > 0) {
			yield return new WaitForSeconds (decayRate);
			qualityLevel--;
		}
		Destroy (gameObject);
	}
}
