using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientIndicator : MonoBehaviour {

	public GameObject indicatorPrefab;
	public Texture2D image;

	private static Vector3 startingScale = new Vector3 (0.3f, 0.3f, 0.3f);
	private static Vector3 endingScale = new Vector3 (1f, 1f, 1f);
	private float originalDistanceFromIndicator;

	private GameObject indicator;

	// Need this in awake for the same reason we need it in awake in the FallDecayDie script
	void Awake () {
		RaycastHit hit;
		bool raycast = ObjectSpawn.RaycastUntilTerrain(transform.position, Vector3.down, out hit);
		if (raycast) {
			indicator = Instantiate (indicatorPrefab, hit.point, Quaternion.identity) as GameObject;
			indicator.transform.GetChild(0).GetComponent<Renderer> ().material.mainTexture = image;
			indicator.transform.localScale = startingScale;
			originalDistanceFromIndicator = CalculateDistanceToIndicator ();
		}
	}

	void Update() {
		if (indicator != null) {
			indicator.transform.localScale = Vector3.Lerp (endingScale, startingScale, CalculateDistanceToIndicator () / originalDistanceFromIndicator);
		}
	}
	
	public void DestroyIndicator () {
		Destroy (indicator);
	}

	float CalculateDistanceToIndicator() {
		return Vector3.Distance (transform.position, indicator.transform.position);
	}
}
