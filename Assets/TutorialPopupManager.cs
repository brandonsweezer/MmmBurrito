using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DigitalRuby.Tween;

public class TutorialPopupManager : MonoBehaviour {

	private Camera cam;
	private Renderer[] renderers = new Renderer[2];
	private TextMeshPro textMeshPro;

	private GameObject container;
	private GameObject textObj;
	private GameObject boxObj;

	private Vector3 textBoxPadding = new Vector3(1f, 1f, 0.1f);
	private float spawnAnimationDuration = 0.2f;

	void Awake() {
		cam = Camera.main;
		container = transform.GetChild (0).gameObject;
		textObj = transform.GetChild (0).GetChild (0).gameObject;
		boxObj = transform.GetChild (0).GetChild (1).gameObject;
		renderers[0] = textObj.GetComponent<Renderer> ();
		renderers[1] = boxObj.GetComponent<Renderer> ();
		textMeshPro = textObj.GetComponent<TextMeshPro> ();
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
		transform.position = playerPos + cam.transform.up * 4.25f;
		transform.position += (cam.transform.position - transform.position).normalized * 7f;
	}

	public void Show() {
		foreach (Renderer r in renderers) {
			r.enabled = true;
		}
		boxObj.transform.localScale = textMeshPro.textBounds.size + textBoxPadding;
		boxObj.transform.localPosition = new Vector3(0, -boxObj.transform.localScale.y/2, 0);

		SpawnAnimation ();
	}

	public void Hide() {
		foreach (Renderer r in renderers) {
			r.enabled = false;
		}
	}

	public void SetText(string textToDisplay) {
		textMeshPro.text = textToDisplay;
	}

	void SpawnAnimation() {
		container.Tween (name+"scale"+Time.time, Vector3.zero, Vector3.one, spawnAnimationDuration, TweenScaleFunctions.QuadraticEaseIn, (t) => 
			{
				container.transform.localScale = t.CurrentValue;
			}
		);
		container.Tween (name+"pos"+Time.time, new Vector3(0, -5, 0), Vector3.zero, spawnAnimationDuration, TweenScaleFunctions.QuadraticEaseIn, (t) => 
			{
				container.transform.localPosition = t.CurrentValue;
			}
		);
	}
}
