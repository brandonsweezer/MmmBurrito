using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableRendererOnPlay : MonoBehaviour {

	void Start () {
		GetComponent<Renderer> ().enabled = false;
	}
}
