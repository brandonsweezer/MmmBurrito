using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour {
	public GameObject[] fallingObjectList;
	public float spawnInterval;

	private static float spawnHeight = 10f;

	private float[] spawnRangeX;
	private float[] spawnRangeZ;

	void Start () {
		float xScale = transform.localScale.x;
		float zScale = transform.localScale.z;
		spawnRangeX = new float[2]{transform.position.x - xScale/2, transform.position.x + xScale/2};
		spawnRangeZ = new float[2]{transform.position.z - zScale/2, transform.position.z + zScale/2};
		StartCoroutine (SpawnFallingObjects ());
	}

	IEnumerator SpawnFallingObjects () {
		while (true) {
			GameObject objectToSpawn = fallingObjectList[Random.Range(0, fallingObjectList.Length)];
			Vector3 spawnPosition = new Vector3 (Random.Range (spawnRangeX[0], spawnRangeX[1]), spawnHeight, Random.Range (spawnRangeZ[0], spawnRangeZ[1]));
			Quaternion spawnRotation = Quaternion.identity;
			GameObject obj = Instantiate (objectToSpawn, spawnPosition, spawnRotation) as GameObject;
			yield return new WaitForSeconds (spawnInterval);
		}
	}
}
