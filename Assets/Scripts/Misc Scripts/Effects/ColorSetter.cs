using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSetter : MonoBehaviour {

	public void SetColor(Color color) {
		changeColorRecursive (transform, color);
	}

	public void TintColor(Color color, float t) {
		tintColorRecursive (transform, color, t);
	}

	void changeColorRecursive(Transform transf, Color color) {
		// Change this transform's color
		Renderer renderer = transf.gameObject.GetComponent<Renderer> ();
		if (renderer != null) {
			foreach (Material mat in renderer.materials) {
				mat.color = color;
			}
		}
		// Do the same for each of its children
		foreach (Transform child in transf) {
			changeColorRecursive (child, color);
		}
	}

	void tintColorRecursive(Transform transf, Color color, float t) {
		// Change this transform's color
		Renderer renderer = transf.gameObject.GetComponent<Renderer> ();
		if (renderer != null) {
			foreach (Material mat in renderer.materials) {
				mat.color = Color.Lerp (mat.color, color, t);
			}
		}
		// Do the same for each of its children
		foreach (Transform child in transf) {
			tintColorRecursive (child, color, t);
		}
	}
}
