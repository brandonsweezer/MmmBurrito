using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.Tween;
using System;

public class UIAnimationManager : MonoBehaviour {

	private string moveKey;
	private string scaleKey;
	private string tintKey;

	private RectTransform rect;
	private Vector2 defaultPos;
	private Color defaultColor;

	private Image image;

	void Awake() {
		rect = GetComponent<RectTransform> ();
		image = gameObject.GetComponent<Image> ();

		UpdateDefaultValues ();
		GetNewKeys ();
	}

	void UpdateDefaultValues() {
		defaultPos = rect.anchoredPosition;
		defaultColor = image.color;
	}

	public void GetNewKeys() {
		moveKey = "Move_" + name + Time.time + gameObject.GetHashCode();
		scaleKey = "Scale_" + name + Time.time + gameObject.GetHashCode();
		tintKey = "Tint_" + name + Time.time + gameObject.GetHashCode();
	}

	public void ResetToInitialValues() {
		rect.anchoredPosition = defaultPos;
		rect.localScale = Vector3.one;
		image.color = defaultColor;
	}

	public void SetValues(Vector2 targetPos, Vector3 scale) {
		rect.anchoredPosition = targetPos;
		rect.localScale = scale;
	}

	public void SetPos(Vector2 targetPos) {
		rect.anchoredPosition = targetPos;
	}

	public void SetColor(Color targetColor) {
		image.color = targetColor;
	}

	public void TweenToInitialValues(float duration = 1f, Action callback = null) {
		Move (defaultPos, duration, callback);
		Scale (defaultPos, duration, null);
	}

	// MOVE FUNCTIONS
	public void Move(Vector2 targetPos, float duration = 1f, Action callback = null) {
		gameObject.Tween (moveKey, rect.anchoredPosition, targetPos, duration, TweenScaleFunctions.QuadraticEaseInOut, (t) => 
			{
				rect.anchoredPosition = t.CurrentValue;
			}, (t) => { if (callback != null) { callback(); } }
		);
	}
	public void MoveToPosAndBack(Vector2 targetPos, float delay, float tweenDuration1, float tweenDuration2, Action finalCallback = null) {
		Vector2 startPos = rect.anchoredPosition;
		Action callback = () => {
			ExecuteAfterDelay (delay, () => { 
				Move (startPos, tweenDuration2, finalCallback); 
			});
		};
		Move(targetPos, tweenDuration1, callback);
	}

	// SCALE FUNCTIONS
	public void Scale(Vector3 targetScale, float duration = 1f, Action callback = null) {
		gameObject.Tween (scaleKey, rect.localScale, targetScale, duration, TweenScaleFunctions.QuadraticEaseInOut, (t) => 
			{
				rect.localScale = t.CurrentValue;
			}, (t) => { if (callback != null) { callback(); } }
		);
	}
	public void ScaleToValueAndBack(Vector3 targetScale, float delay, float tweenDuration1, float tweenDuration2, Action finalCallback = null) {
		Vector3 startScale = rect.localScale;
		Action callback = () => {
			ExecuteAfterDelay (delay, () => { 
				Scale (startScale, tweenDuration2, finalCallback); 
			});
		};
		Scale(targetScale, tweenDuration1, callback);
	}


	// COLOR FUNCTIONS
	public void Tint(Color targetColor, float duration = 1f, Action callback = null) {
		gameObject.Tween (tintKey, image.color, targetColor, duration, TweenScaleFunctions.QuadraticEaseInOut, (t) => 
			{
				image.color = t.CurrentValue;
			}, (t) => { if (callback != null) { callback(); } }
		);
	}
	public void TintToColorAndBack(Color targetColor, float delay, float tweenDuration1, float tweenDuration2, Action finalCallback = null) {
		Vector2 startPos = rect.anchoredPosition;
		Action callback = () => {
			ExecuteAfterDelay (delay, () => { 
				Tint (Color.white, tweenDuration2, finalCallback); 
			});
		};
		Tint(targetColor, tweenDuration1, callback);
	}

	public void ExecuteAfterDelay(float delay, Action callback) {
		IEnumerator coroutine = ExecuteAfterDelayRoutine(delay, callback);
		StartCoroutine(coroutine);
	}

	IEnumerator ExecuteAfterDelayRoutine(float delay, Action callback) {
		yield return new WaitForSeconds(delay);
		callback();
	}

	public void StopAllAnimations() {
		DigitalRuby.Tween.TweenFactory.RemoveTweenKey(scaleKey, DigitalRuby.Tween.TweenStopBehavior.Complete);
		DigitalRuby.Tween.TweenFactory.RemoveTweenKey(moveKey, DigitalRuby.Tween.TweenStopBehavior.Complete);
		DigitalRuby.Tween.TweenFactory.RemoveTweenKey(tintKey, DigitalRuby.Tween.TweenStopBehavior.Complete);
	}

	public void LevelStartSpawnAnimation(Vector2 position, float scale = 1f, float duration = 0.5f, float callbackDelay = 2.5f) {
		StopAllAnimations ();
		UpdateDefaultValues ();
		// spawning values
		Vector2 startPos = new Vector2 (position.x, position.y + rect.sizeDelta.y);
		Color startColor = defaultColor;
		startColor.a = 0;
		SetPos (startPos);
		SetColor (startColor);
		// animate to spawn end pos, then back to default pos
		Action callbackPos = () => ExecuteAfterDelay(callbackDelay, () => Move (defaultPos));
		Action callbackScale = () => ExecuteAfterDelay(callbackDelay, () => Scale (Vector3.one));
		Move (position, duration, callbackPos);
		Scale (new Vector3(scale, scale, scale), duration, callbackScale);
		Tint (defaultColor, duration);
	}
}
