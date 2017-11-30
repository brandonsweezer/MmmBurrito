using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopupOnCollision : MonoBehaviour {

	public string textToDisplay;
	public GameObject UIPopupPrefab;

	private GameObject popupObject;

	void Start() {
		popupObject = Instantiate (UIPopupPrefab) as GameObject;
		popupObject.GetComponent<TutorialPopupManager> ().SetText(textToDisplay);
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
