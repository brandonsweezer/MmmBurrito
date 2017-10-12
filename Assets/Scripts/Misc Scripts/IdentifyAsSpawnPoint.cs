using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentifyAsSpawnPoint : MonoBehaviour {

	void Awake() {
		SpawnController.instance.spawnPoint = gameObject;
	}
}
.