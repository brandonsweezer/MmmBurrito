using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

	private const float Y_ANGLE_MIN = 0.0f;
	private const float Y_ANGLE_MAX = 50.0f;
	private float currentY;


	public Transform target;
	public GameObject burrito; 
	public float restDistance; 
	public float restHeight; 
	public float moveDistance;
	public float moveHeight; 



	private Transform _myTransform;

	// Use this for initialization
	void Start () {
		if (target == null)
			Debug.LogWarning ("We do not have a target");

		_myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
//		currentX += Input.GetAxis ("Horizontal");
//		currentY += Input.GetAxis ("Vertical");

		currentY = Mathf.Clamp (target.position.y, Y_ANGLE_MIN, Y_ANGLE_MAX);
	}

	void LateUpdate () {
		if (!burrito.GetComponent<MovementController> ().getMovement()) {

			Vector3 restDir = new Vector3 (0, 0, target.position.z - restDistance);
			Quaternion rotation = Quaternion.Euler (currentY, target.position.y, 0);
			_myTransform.position = target.position + rotation * restDir; 
			_myTransform.LookAt (target.position);

//			_myTransform.position = new Vector3 (target.position.x, target.position.y + restHeight, target.position.z - restDistance);
//			_myTransform.LookAt (target);
			//_myTransform.eulerAngles = new Vector3(60, target.transform.eulerAngles.y, 0);

		} else {

			Vector3 moveDir = new Vector3 (0, 0, target.position.z - moveDistance);
			Quaternion rotation = Quaternion.Euler (currentY, target.position.y, 0);
			_myTransform.position = target.position + rotation * moveDir; 
			_myTransform.LookAt (target.position);

//			_myTransform.position = new Vector3 (target.position.x, target.position.y + moveHeight, target.position.z - moveDistance);
//			_myTransform.LookAt (target);
			//_myTransform.eulerAngles = new Vector3(60, target.transform.eulerAngles.y, 0);
		}



//
//
//
//		private void LateUpdate () {
//			Vector3 dir = new Vector3 (0, 0, -distance);
//			Quaternion rotation = Quaternion.Euler (currentY, target.position.y, 0);
//			_myTransform.position = target.position + rotation * dir; 
//			_myTransform.LookAt (target.position);
//		}
//			


	}
}
