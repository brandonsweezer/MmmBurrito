using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCamera : MonoBehaviour {

	// Note: could make these two variables public, but for the sake of consistency we won't
	// [Tooltip("[0..1]")]
	private float looseness = 0.8f;
	[Tooltip("[0..1]")]
	public  float biasForDefaultPosition = 0.7f;

	private GameObject target;
	private Vector3 defaultPosition;
	private Vector3 xzPlaneOffset; // used in moving the camera to loosely follow the player

	void Start () {
		defaultPosition = transform.position;

		// determine hypothenuseToFloor
		Plane hPlane = new Plane(Vector3.up, Vector3.zero);
		float distance = 0; 
		Ray camRay = new Ray (transform.position, transform.forward);
		if (hPlane.Raycast(camRay, out distance)) {
			Vector3 hitPoint = camRay.GetPoint(distance);
			xzPlaneOffset = new Vector3(transform.position.x, 0, transform.position.z) - hitPoint;
		}
	}

	void Update () {
		if (target == null) {
			target = GameController.instance.player;
		} else {
			Vector3 pointAboveTarget = new Vector3 (target.transform.position.x, transform.position.y, target.transform.position.z) + xzPlaneOffset;
			Vector3 newPosition = Vector3.Lerp (pointAboveTarget, defaultPosition, biasForDefaultPosition);
			transform.position = Vector3.Lerp (newPosition, transform.position, looseness);
		}
	}
}
