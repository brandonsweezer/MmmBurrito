using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialPopupOnCollision : MonoBehaviour {

	public Texture imageToDisplay;
	public GameObject UIPopupPrefab;

	private GameObject popupObject;

	void Start() {
		popupObject = Instantiate (UIPopupPrefab) as GameObject;
		popupObject.transform.GetChild(0).GetComponent<Renderer> ().material.mainTexture = imageToDisplay;
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag == "Player") {
			popupObject.GetComponent<TutorialPopupManager> ().Show ();
		}
	}

	void OnCollisionExit(Collision collision) {
		if (collision.collider.tag == "Player") {
			popupObject.GetComponent<TutorialPopupManager> ().Hide ();
		}
	}

}
