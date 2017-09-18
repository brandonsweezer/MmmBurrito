using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public GameObject[] fallingObjectList;
	public Vector3 spawnValues;
	public float spawnInterval;
	public Dictionary<Order, int> orderList;

	private OrderList globalOrders;

	void Start () {
		orderList = new Dictionary<Order, int> ();
		globalOrders = new OrderList ();
		StartCoroutine (SpawnFallingObjects ());
		//should be different for each level
		orderList.Add (globalOrders.getOrders(2), 1);
		foreach (KeyValuePair<Order, int> entry in orderList) {
			for (int i = 0; i < entry.Value; i++) {
				entry.Key.print ();
			}
		}
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
