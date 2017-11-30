using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopupManager : MonoBehaviour {

	private Camera cam;
	private Renderer renderer;

	void Awake() {
		cam = Camera.main;
		renderer = transform.GetChild (0).GetComponent<Renderer> ();
	}

	void Start() {
		Hide ();
	}

	void Update() {
		PositionAndFaceCamera ();
	}

	void PositionAndFaceCamera() {
		// find player for parent
		if (GameController.instance.player == null) {
			return;
		}

		transform.parent = GameController.instance.player.transform;

		// face camera
		transform.forward = cam.transform.forward;
		transform.up = cam.transform.up;

		// position
		Vector3 playerPos = transform.parent.transform.position;
		transform.position = playerPos + cam.transform.up * 3.75f;
		transform.position += (cam.transform.position - transform.position).normalized * 7f;
	}

	public void Show() {
		renderer.enabled = true;
	}

	public void Hide() {
		renderer.enabled = false;
	}
}
