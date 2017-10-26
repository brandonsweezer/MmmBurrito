using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFlash : MonoBehaviour {

	public float period = 0.3f;
	public Color startColor = Color.red;
	public Color endColor = Color.green;

	private Renderer renderer;
	private bool active;

	void Start () {
		renderer = GetComponent<Renderer> ();
	}

	void Update () {
		if (active) {
			float lerp = Mathf.PingPong (Time.time, period) / period;
			Color flashColor = Color.Lerp (startColor, endColor, lerp);
			renderer.material.SetColor ("_Color", flashColor);
		} else {
			renderer.material.SetColor ("_Color", Color.white);
		}
	}

	public void StartFlashing() {
		active = true;
	}

	public void StopFlashing() {
		active = false;
	}
}
