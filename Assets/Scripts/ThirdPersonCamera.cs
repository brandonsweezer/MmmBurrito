using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

	private const float Y_ANGLE_MIN = 0.0f;
	private const float Y_ANGLE_MAX = 50.0f;

	public GameObject gameControllerObject; 
	public float restDistance; 
	public float restHeight; 
	public float moveDistance;
	public float moveHeight; 
	public float speed;

	public Transform camTransform;
	private Camera cam;

	private float currentX = 0.0f;
	private float currentY = 0.0f;

	// Use this for initialization
	private void Start () {
		camTransform = transform;
		cam = Camera.main;
	}







	// Update is called once per frame
	private void Update () {
		currentX += Input.GetAxis ("Horizontal");
		currentY += Input.GetAxis ("Vertical");

		currentY = Mathf.Clamp (currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
	}

	private void LateUpdate () {
		GameObject burrito = gameControllerObject.GetComponent<GameController> ().player;
		Transform target = burrito.transform;
		if (!burrito.GetComponent<MovementController> ().getMovement()) {
			Vector3 dir = new Vector3 (0, restHeight, -restDistance);
			Quaternion rotation = Quaternion.Euler (0, speed*currentX, 0); //currentY?
			camTransform.position = target.position + rotation * dir; 
			camTransform.LookAt (target.position);
		}
		else{
			Vector3 dir = new Vector3 (0, moveHeight, -moveDistance);
			Quaternion rotation = Quaternion.Euler (0, speed*currentX, 0);
			camTransform.position = target.position + rotation * dir; 
			camTransform.LookAt (target.position);
		}
	}
}
