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

	private Image image;

	void Awake() {
		rect = GetComponent<RectTransform> ();
		image = gameObject.GetComponent<Image> ();

		defaultPos = rect.anchoredPosition;
		GetNewKeys ();
	}

	public void GetNewKeys() {
		moveKey = "Move_" + name + Time.time;
		scaleKey = "Scale_" + name + Time.time;
		tintKey = "Tint_" + name + Time.time;
	}

	public void ResetToInitialValues() {
		rect.anchoredPosition = defaultPos;
		rect.localScale = Vector3.one;
	}

	public void SetValues(Vector2 targetPos, Vector3 scale) {
		rect.anchoredPosition = targetPos;
		rect.localScale = scale;
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
		DigitalRuby.Tween.TweenFactory.RemoveTweenKey(scaleKey, DigitalRuby.Tween.TweenStopBehavior.DoNotModify);
		DigitalRuby.Tween.TweenFactory.RemoveTweenKey(moveKey, DigitalRuby.Tween.TweenStopBehavior.DoNotModify);
	}
}
