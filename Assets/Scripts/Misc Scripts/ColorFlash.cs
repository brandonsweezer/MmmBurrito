using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFlash : MonoBehaviour {

	public float period = 0.3f;
	public Color startColor = Color.red;
	public Color endColor = Color.green;

	public bool onByDefault = false;

	private bool active;

	void Start() {
		active = onByDefault;
	}

	void Update () {
		if (active) {
			float lerp = Mathf.PingPong (Time.time, period) / period;
			Color flashColor = Color.Lerp (startColor, endColor, lerp);
			changeColorRecursive (transform, flashColor);
		} else {
			changeColorRecursive (transform, Color.white);
		}
	}

	public void SetActive(bool active) {
		this.active = active;
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
