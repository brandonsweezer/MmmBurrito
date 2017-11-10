using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationManager : MonoBehaviour {
	
	private float positionMoveFactor = 0.02f;
	private RectTransform rect;
	private IEnumerator moveCoroutine;

	private Vector2 defaultPos;

	void Awake() {
		rect = GetComponent<RectTransform> ();
		defaultPos = rect.anchoredPosition;
		moveCoroutine = null;
	}

	public void ResetToDefaultPosition() {
		StopCoroutine (moveCoroutine);
		rect.anchoredPosition = defaultPos;
	}

	public void StartMoveToPosition(Vector2 targetPos, bool allowOverride = true) {
		if (moveCoroutine != null) {
			if (allowOverride) {
				StopCoroutine (moveCoroutine);
			} else {
				return;
			}
		}
		moveCoroutine = MoveToPosition(targetPos);
		StartCoroutine (moveCoroutine);
	}

	public void StartMoveToPosition(float targetX, float targetY, bool allowOverride = true) {
		StartMoveToPosition (new Vector2 (targetX, targetY), allowOverride);
	}

	IEnumerator MoveToPosition(Vector2 targetPos) {
		while (true) {
			rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, targetPos, positionMoveFactor);
			if (Vector2.Distance(rect.anchoredPosition, targetPos) < 0.1f) {
				rect.anchoredPosition = targetPos;
				StopCoroutine (moveCoroutine);
			}
			yield return new WaitForEndOfFrame();
		}
	}
}
