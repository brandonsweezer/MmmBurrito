using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCatcher : MonoBehaviour {

	public bool canCatch;
	private Dictionary<string, int> caughtObjects;

	void Start () {
		canCatch = true;
		caughtObjects = new Dictionary<string, int> ();
	}

	void OnCollisionEnter (Collision collision) {
		if (!canCatch) {
			return;
		}

		GameObject gameObj = collision.gameObject;
		if (gameObj.tag == "FallingObject") {
			CatchObject (gameObj);
			Destroy (gameObj);
		}

		/*
		// Create an effect at the location of contact.
		ContactPoint contact = collision.contacts[0];
		// Rotate the object so that the y-axis faces along the normal of the surface
		Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
		Vector3 pos = contact.point;
		Instantiate(explosionPrefab, pos, rot);
		*/
	}

	/** Catches an object by updating the caught values for the [caughtObjects] dictionary */
	void CatchObject (GameObject gameObj) {
		string objectName = gameObj.name.Replace ("(Clone)", "");
		// Increment the count for this object type, or set it to 1 if non-existent in dictionary
		int currentCount;
		caughtObjects.TryGetValue (objectName, out currentCount);
		caughtObjects [objectName] = currentCount + 1;

		// Print out for now
		Debug.Log (string.Format("Caught a {0}, burrito now contains:\n{1}", objectName, CaughtObjectsToString ()));
	}

	/** Returns the content of the [caughtObjects] dictionary as a string */
	string CaughtObjectsToString () {
		string result = "";
		foreach (KeyValuePair<string, int> kvp in caughtObjects) {
			Debug.Log (kvp.Key);
			result += string.Format("{0} {1}(s), ", kvp.Value, kvp.Key);
		}
		result = result.Substring (0, result.Length - 2);
		return result;
	}
}
