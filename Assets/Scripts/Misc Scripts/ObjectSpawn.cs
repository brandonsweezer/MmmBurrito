using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour {
	public GameObject[] fallingObjectList;
	public float spawnInterval;

	private static float spawnYOffset = 10f;
	private static float maxSpawnHeight = 200f;

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

			// Determine Position
			Vector3 spawnPosition = new Vector3 (Random.Range (spawnRangeX[0], spawnRangeX[1]), maxSpawnHeight, Random.Range (spawnRangeZ[0], spawnRangeZ[1]));
			// Snap to tiled position
			spawnPosition.x = Mathf.Round(spawnPosition.x / TiledFloor.tileWidth) * TiledFloor.tileWidth;
			spawnPosition.z = Mathf.Round(spawnPosition.x / TiledFloor.tileHeight) * TiledFloor.tileHeight;
			// Offset position a certain distance above ground below that position
			RaycastHit hit;
			bool raycastResult = Physics.Raycast (spawnPosition, Vector3.down, out hit, maxSpawnHeight);
			if (!raycastResult) {
				// Debug.LogError ("Oops! An object spawn region is hovering over the void! Spawning object at height "+maxSpawnHeight);
			} else {
				spawnPosition.y = hit.point.y + spawnYOffset;
			}

			// Spawn the object
			GameObject objectToSpawn = fallingObjectList[Random.Range(0, fallingObjectList.Length)];
			Quaternion spawnRotation = Quaternion.identity;
			GameObject obj = Instantiate (objectToSpawn, spawnPosition, spawnRotation) as GameObject;

			LoggingManager.instance.RecordEvent(1, obj.name+ "Spawned");
			yield return new WaitForSeconds (spawnInterval);
		}
	}
}
