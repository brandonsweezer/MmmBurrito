using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketAnimations : MonoBehaviour {

	private static Vector2 removeMovement = new Vector2 (0, 0.5f);

	private Vector2 perFrameMovement = new Vector2(0, 0);

	// Update is called once per frame
	void Update () {
		GetComponent<RectTransform> ().anchoredPosition += perFrameMovement;
	}

	public void StartRemoveAnimation() {
		Debug.Log ("start remove anim");
		perFrameMovement = removeMovement;
	}
}
