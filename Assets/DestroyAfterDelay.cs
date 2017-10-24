using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour {

	public float destroyDelay;

	void Start () {
		StartCoroutine (DestroyObject ());
	}

	IEnumerator DestroyObject () {
		yield return new WaitForSeconds(destroyDelay);
		Destroy(gameObject);
	}
}
