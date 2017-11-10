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
		if (GetComponent<ColorSetter> () == null) {
			gameObject.AddComponent<ColorSetter> ();
		}
	}

	void Update () {
		if (active) {
			float lerp = Mathf.PingPong (Time.time, period) / period;
			Color flashColor = Color.Lerp (startColor, endColor, lerp);
			GetComponent<ColorSetter> ().SetColor(flashColor);
		} else {
			GetComponent<ColorSetter> ().SetColor(Color.white);
		}
	}

	public void SetActive(bool active) {
		this.active = active;
	}
}
