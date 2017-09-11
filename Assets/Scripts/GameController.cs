using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public GameObject[] fallingObjectList;
	public Vector3 spawnValues;
	public float spawnInterval;

	void Start () {
		StartCoroutine (SpawnFallingObjects ());
	}

	IEnumerator SpawnFallingObjects () {
		while (true) {	
			GameObject objectToSpawn = fallingObjectList[Random.Range(0, fallingObjectList.Length)];
			Vector3 spawnPosition = new Vector3 (Random.Range (0, spawnValues.x), spawnValues.y, Random.Range (0, spawnValues.z));
			Quaternion spawnRotation = Quaternion.identity;
			Instantiate (objectToSpawn, spawnPosition, spawnRotation);
			yield return new WaitForSeconds (spawnInterval);
		}
	}
}