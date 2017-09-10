using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public GameObject fallingObject;
	public Vector3 spawnValues;
	public float spawnInterval;

	void Start () {
		StartCoroutine (SpawnFallingObjects ());
	}

	IEnumerator SpawnFallingObjects () {
		while (true)
		{
			Vector3 spawnPosition = new Vector3 (Random.Range (0, spawnValues.x), spawnValues.y, Random.Range (0, spawnValues.z));
			Quaternion spawnRotation = Quaternion.identity;
			Instantiate (fallingObject, spawnPosition, spawnRotation);
			yield return new WaitForSeconds (spawnInterval);
		}
	}
}