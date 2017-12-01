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

	void OnCollisionStay(Collision collision) {
		OnTriggerStay (collision.collider);
	}

	void OnTriggerStay(Collider col) {
		if (col.tag == "Player") {
			popupObject.GetComponent<TutorialPopupManager> ().Show ();
		}
	}

	void OnCollisionExit(Collision collision) {
		OnTriggerExit (collision.collider);
	}

	void OnTriggerExit(Collider col) {
		if (col.tag == "Player") {
			popupObject.GetComponent<TutorialPopupManager> ().Hide ();
		}
	}

}
