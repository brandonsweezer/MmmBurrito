using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCatcher : MonoBehaviour {

	public bool canCatch;
	private CaughtIngredientSet caughtIngredients;


	private string currentBurritoText; 

	void Start () {
		canCatch = true;
		caughtIngredients = new CaughtIngredientSet();
		setTextString("");
	}

	public void setTextString (string text) {
		currentBurritoText = text;
	}

	public string getTextString () {
		return currentBurritoText;
	}

	/** Returns true if this ObjectCatcher is empty */
	public bool IsEmpty () {
		return caughtIngredients.IsEmpty();
	}


	/** Handle collisions with falling objects */
	void OnCollisionEnter (Collision collision) {
        if (!canCatch)
        {
            // return; // disabled until we can visually show the burrito's wrap-state
        }

		GameObject gameObj = collision.gameObject;
		if (gameObj.tag == "FallingObject") { 
            CatchObject (gameObj);
            LoggingManager.instance.RecordEvent(6, "Caught ingredient - " + gameObj.name);
			Destroy (gameObj);
		}
	}

	/** Catches an object by updating the caught values for the [caughtObjects] dictionary */
	void CatchObject (GameObject gameObj) {
		// Catch object
		string objectName = gameObj.name.Replace ("(Clone)", "");
		IngredientSet.Ingredients ingredientType = IngredientSet.StringToIngredient (objectName);
		int quality = gameObj.GetComponent<DecayAndDie> ().getQuality ();
		caughtIngredients.CatchIngredient (ingredientType, quality);

		// Print out
		Debug.Log (string.Format("Caught a {0}, burrito now contains:\n{1}", objectName, CaughtObjectsToString ()));
		setTextString(string.Format("Burrito contents: {0}", CaughtObjectsToString ()));
	}

	/** Returns the content of the [caughtObjects] dictionary as a string */
	public string CaughtObjectsToString () {
		return caughtIngredients.ToString ();
	}

	public CaughtIngredientSet getIngredients(){
		return caughtIngredients;
	}
}
