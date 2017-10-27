using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	[Tooltip("[0..1]")]
	public float looseness;
	[Tooltip("[0..1]")]
	public float biasForDefaultPosition;

	private GameObject target;
	private Vector3 offset;
	private Vector3 defaultPosition;

	void Start () {
		defaultPosition = target.transform.position;
		offset = Vector3.zero;
	}

	void setTarget (GameObject target) {
		this.target = target;
		// Set offset during the first spawn (subsequent spawns will remember this offset).
		if (offset == Vector3.zero) {
			offset = transform.position - target.transform.position;
		}
	}

	void Update () {
		if (target == null) {
			setTarget (GameController.instance.player);
		} else {
			transform.position = Vector3.Lerp (target.transform.position + offset, transform.position, looseness);

			// Bias position towards default position.
			transform.position = Vector3.Lerp (transform.position, defaultPosition, biasForDefaultPosition);
		}
	}
}
