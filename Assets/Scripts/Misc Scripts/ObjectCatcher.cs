using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCatcher : MonoBehaviour {

	public bool canCatch;
	private CaughtIngredientSet caughtIngredients;


	private string currentBurritoText; 
	private bool newIngredient=false; 
	private IngredientSet.Ingredients ingredientType;
	private int ingredientQuality;


	void Start () {
		canCatch = true;
		caughtIngredients = new CaughtIngredientSet();
		//SetTextString("");
	}

//	public void SetTextString (string text) {
//		currentBurritoText = text;
//	}
//
//	public string GetTextString () {
//		return currentBurritoText;
//	}

	public void SetnewIngredient(bool boolean) {
		newIngredient = boolean;
	}

	public bool GetnewIngredient() {
		return newIngredient;
	}

	public IngredientSet.Ingredients GetIngredientType() {
		return ingredientType;
	}

	public int GetIngredientQuality() {
		return ingredientQuality;
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
			SetnewIngredient (true);
			// Make sure burrito is not full
			if (caughtIngredients.ingredientSet.GetFullCount () >= 6) {
				//Debug.Log ("Can't catch, burrito is full!");
				//SetTextString("Cannot catch anymore. Burrito Full");
				return;
			}

			CatchObject (gameObj);
			LoggingManager.instance.RecordEvent (6, "Caught ingredient - " + gameObj.name);
			if (gameObj.GetComponent<MoveToScreen> () != null) {
				gameObj.GetComponent<MoveToScreen> ().StartMovingToScreenBottom (true);
			} else {
				Destroy (gameObj);
			}
		}
	}

	/** Catches an object by updating the caught values for the [caughtObjects] dictionary */
	void CatchObject (GameObject gameObj) {
        //remove from GameController
        GameController.instance.objects.RemoveAt(GameController.instance.objects.IndexOf(gameObj));
        // Catch object
        string objectName = gameObj.name.Replace ("(Clone)", "");
		ingredientType = IngredientSet.StringToIngredient (objectName);
		ingredientQuality = gameObj.GetComponent<FallDecayDie> ().getQuality ();
		caughtIngredients.CatchIngredient (ingredientType, ingredientQuality);

		// Updates whether we can submit successfully or not
		GameController.instance.UpdateSubmissionValidity();

		// Print out
		Debug.Log (string.Format("Caught a {0}, burrito now contains:\n{1}", objectName, CaughtObjectsToString ()));
		//SetTextString(string.Format("Burrito contents: {0}", CaughtObjectsToString ()));
	}

	/** Returns the content of the [caughtObjects] dictionary as a string */
	public string CaughtObjectsToString () {
		return caughtIngredients.ToString ();
	}

	public CaughtIngredientSet getIngredients(){
		return caughtIngredients;
	}
}
