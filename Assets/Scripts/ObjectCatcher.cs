using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCatcher : MonoBehaviour {

	public bool canCatch;
	private Dictionary<string, List<int>> caughtObjects;

	void Start () {
		canCatch = true;
		caughtObjects = new Dictionary<string, List<int>> ();
	}
		
	/** Returns the number of caught objects, summing up across all object types */
	public int getNumCaughtObjects () {
		if (isEmpty ()) {
			return 0;
		} else {
			int numCaughtObjects = 0;
			foreach (KeyValuePair<string, List<int>> kvp in caughtObjects) {
				numCaughtObjects += kvp.Value.Count;
			}
			return numCaughtObjects;
		}
	}

	/** Returns true if this ObjectCatcher is empty */
	public bool isEmpty () {
		return caughtObjects.Count == 0;
	}


	/** Handle collisions with falling objects */
	void OnCollisionEnter (Collision collision) {
        if (!canCatch)
        {
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
        if (caughtObjects.ContainsKey(objectName)){
            caughtObjects[objectName].Add(gameObj.GetComponent<DecayAndDie>().getQuality());
        }
        else{
            caughtObjects.Add(objectName, new List<int>(new int[] {gameObj.GetComponent<DecayAndDie>().getQuality()}));
        }

		// Print out for now
		Debug.Log (string.Format("Caught a {0}, burrito now contains:\n{1}", objectName, CaughtObjectsToString ()));
	}

	/** Returns the content of the [caughtObjects] dictionary as a string */
	public string CaughtObjectsToString () {
		string result = "";
		foreach (KeyValuePair<string, List<int>> kvp in caughtObjects) {
			result += string.Format("{0} {1}(s), ", kvp.Value.Count, kvp.Key);
		}
		result = result.Substring (0, result.Length - 2);
		return result;
	}

	public Dictionary<string, List<int>> getIngredients(){
		return caughtObjects;
	}
}
