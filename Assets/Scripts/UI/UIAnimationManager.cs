﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;
using System;

public class UIAnimationManager : MonoBehaviour {

	private string moveKey;
	private string scaleKey;

	private RectTransform rect;
	private Vector2 defaultPos;

	void Awake() {
		rect = GetComponent<RectTransform> ();
		defaultPos = rect.anchoredPosition;
		moveKey = "Move_" + name;
		scaleKey = "Scale_" + name;
	}

	// MOVE FUNCTIONS
	public void Move(Vector2 targetPos, float duration = 1f, Action callback = null) {
		gameObject.Tween (moveKey, rect.anchoredPosition, targetPos, duration, TweenScaleFunctions.QuadraticEaseInOut, (t) => 
			{
				rect.anchoredPosition = t.CurrentValue;
			}, (t) => { if (callback != null) { callback(); } }
		);
	}
	public void MoveToPosAndBack(Vector2 targetPos, float delay, float tweenDuration1, float tweenDuration2) {
		Vector2 startPos = rect.anchoredPosition;
		Action callback = () => {
			ExecuteAfterDelay (delay, () => { 
				Move (startPos, tweenDuration2); 
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
	public void ScaleToValueAndBack(Vector3 targetScale, float delay, float tweenDuration1, float tweenDuration2) {
		Vector3 startScale = rect.localScale;
		Action callback = () => {
			ExecuteAfterDelay (delay, () => { 
				Scale (startScale, tweenDuration2); 
			});
		};
		Scale(targetScale, tweenDuration1, callback);
	}

	public void ExecuteAfterDelay(float delay, Action callback) {
		IEnumerator coroutine = ExecuteAfterDelayRoutine(delay, callback);
		StartCoroutine(coroutine);
	}

	IEnumerator ExecuteAfterDelayRoutine(float delay, Action callback) {
		yield return new WaitForSeconds(delay);
		callback();
	}
}