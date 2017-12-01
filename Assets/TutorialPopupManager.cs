using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;

public class TutorialPopupManager : MonoBehaviour {

	private Camera cam;
	private Renderer renderer;
	private GameObject image;

	private Vector3 popupScale = Vector3.one;

	private float spawnAnimationDuration = 0.25f;

	public bool showing;

	void Awake() {
		cam = Camera.main;
		image = transform.GetChild (0).gameObject;
		renderer = image.GetComponent<Renderer> ();
	}

	void Start() {
		HideImmediate ();
		showing = false;
	}

	void Update() {
		if (GameController.instance.gamestate != GameController.GameState.Play) {
			Hide ();
		}

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
		transform.position = playerPos + cam.transform.up * 2.25f;
		transform.position += (cam.transform.position - transform.position).normalized * 10f;
	}

	public void Show() {
		if (!showing) {
			showing = true;
			renderer.enabled = true;
			StopAnimation ();
			SpawnAnimation ();
		}
	}

	public void Hide() {
		if (showing) {
			showing = false;
			StopAnimation ();
			DespawnAnimation ();
		}
	}

	void SpawnAnimation() {
		Debug.Log ("scale: " + popupScale);
		gameObject.Tween (name+"scale", Vector3.one * 0.1f, popupScale, spawnAnimationDuration, TweenScaleFunctions.QuadraticEaseIn, (t) => 
			{
				gameObject.transform.localScale = t.CurrentValue;
			}
		);
		/*image.Tween (name+"pos", new Vector3(0, -1.5f, 0), Vector3.zero, spawnAnimationDuration, TweenScaleFunctions.QuadraticEaseOut, (t) => 
			{
				image.transform.localPosition = t.CurrentValue;
			}
		);*/
	}

	void DespawnAnimation() {
		gameObject.Tween (name+"scale", popupScale, Vector3.zero, spawnAnimationDuration, TweenScaleFunctions.QuadraticEaseIn, (t) => 
			{
				gameObject.transform.localScale = t.CurrentValue;
			}, (t) => {
				HideImmediate();
			}
		);
	}

	void StopAnimation() {
		DigitalRuby.Tween.TweenFactory.RemoveTweenKey(name+"scale", DigitalRuby.Tween.TweenStopBehavior.Complete);
		DigitalRuby.Tween.TweenFactory.RemoveTweenKey(name+"pos", DigitalRuby.Tween.TweenStopBehavior.Complete);
	}

	void HideImmediate() {
		renderer.enabled = false;
	}

	public void SetPopupScale(float s) {
		popupScale.Set (s, s, s);
	}
}
