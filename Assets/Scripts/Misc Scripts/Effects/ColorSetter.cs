using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSetter : MonoBehaviour {

	public void SetColor(Color color) {
		changeColorRecursive (transform, color);
	}

	void changeColorRecursive(Transform transf, Color color) {
		// Change this transform's color
		Renderer renderer = transf.gameObject.GetComponent<Renderer> ();
		if (renderer != null) {
			renderer.material.SetColor ("_Color", color);
		}
		// Do the same for each of its children
		foreach (Transform child in transf) {
			changeColorRecursive (child, color);
		}
	}
}
