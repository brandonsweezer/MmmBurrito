using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientIndicator : MonoBehaviour {

	public GameObject indicatorPrefab;
	public Texture2D image;

	private GameObject indicator;

	// Need this in awake for the same reason we need it in awake in the FallDecayDie script
	void Awake () {
		RaycastHit hit;
		bool raycast = ObjectSpawn.RaycastUntilTerrain(transform.position, Vector3.down, out hit);
		if (raycast) {
			indicator = Instantiate (indicatorPrefab, hit.point, Quaternion.AngleAxis (180, Vector3.up)) as GameObject;
			indicator.GetComponent<Renderer> ().material.mainTexture = image;
		}
	}
	
	public void DestroyIndicator () {
		Destroy (indicator);
	}
}
