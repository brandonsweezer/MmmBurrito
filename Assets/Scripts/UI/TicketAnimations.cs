using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketAnimations : MonoBehaviour {

	private float spawnYOffsetPerSecond = -Screen.height * 0.4f;
	private float removeYOffsetPerSecond = Screen.height * 0.25f;
	private float xPositionMoveFactor = 0.1f;
	private RectTransform rect;

	private IEnumerator xMoveCoroutine;

	void Awake() {
		rect = GetComponent<RectTransform> ();
		xMoveCoroutine = null;
	}

	public void StartRemoveAnimation() {
		StartCoroutine("RemoveAnimation");
	}

	public void StartSpawnAnimation() {
		StartCoroutine("SpawnAnimation");
	}

	public void StartMoveToXPosition(float targetX) {
		if (xMoveCoroutine != null) {
			StopCoroutine (xMoveCoroutine);
		}
		xMoveCoroutine = MoveToXPosition(targetX);
		StartCoroutine (xMoveCoroutine);
	}

	IEnumerator SpawnAnimation()
	{
		float targetY = rect.anchoredPosition.y;
		rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + rect.sizeDelta.y);
		while (true) {
			rect.anchoredPosition += new Vector2 (0, spawnYOffsetPerSecond) * Time.deltaTime;
			if (rect.anchoredPosition.y < targetY) {
				rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, targetY);
				StopCoroutine ("SpawnAnimation");
			}
			yield return new WaitForEndOfFrame();
		}
	}

	IEnumerator RemoveAnimation()
	{
		while (true) {
			rect.anchoredPosition += new Vector2 (0, removeYOffsetPerSecond) * Time.deltaTime;
			if (rect.position.y - rect.sizeDelta.y > Screen.height) {
				StopCoroutine ("RemoveAnimation");
				Destroy (gameObject);
			}
			yield return new WaitForEndOfFrame();
		}
	}

	IEnumerator MoveToXPosition(float targetX) {
		while (true) {
			rect.anchoredPosition = new Vector2(Mathf.Lerp(rect.anchoredPosition.x, targetX, xPositionMoveFactor), rect.anchoredPosition.y);
			if (Mathf.Abs(rect.anchoredPosition.x - targetX) < 0.1f) {
				rect.anchoredPosition = new Vector2(targetX, rect.anchoredPosition.y);
				StopCoroutine (xMoveCoroutine);
			}
			yield return new WaitForEndOfFrame();
		}
	}
}
