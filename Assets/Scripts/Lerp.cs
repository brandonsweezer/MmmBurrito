using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerp : MonoBehaviour {


	public Transform posCam1;
	public Transform posCam2;

	public Transform positionCurrent;

	public GameObject transitionCamera;

	public float speedAdjust;

	// Use this for initialization
	void Start () {
		
	}


	void ReOrient (bool reverse) {
		if (reverse) {
			positionCurrent.transform.position = Vector3.Lerp(posCam1.transform.position, posCam2.transform.position, Time.deltaTime * speedAdjust);
		}

		else {
			positionCurrent.transform.position = Vector3.Lerp(posCam2.transform.position, posCam1.transform.position, Time.deltaTime * speedAdjust);
		}
		transitionCamera.transform.position = positionCurrent.transform.position; 
	}


	// Update is called once per frame
	void Update () {
		ReOrient(true);
	}
}
