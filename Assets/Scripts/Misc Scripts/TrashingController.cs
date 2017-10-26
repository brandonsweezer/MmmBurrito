using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashingController : MonoBehaviour {

	void Update () {
		if (Input.GetKeyDown(KeyCode.T))
		{
			LoggingManager.instance.RecordEvent(3, "Trashed ingredients with T: " + GameController.instance.player.GetComponent<ObjectCatcher>().getIngredients().ToString());
			GameController.instance.player.GetComponent<ObjectCatcher>().getIngredients().Empty();
			OrderUI.instance.ResetAfterDeath();
			OrderUI.instance.CollectionUIUpdate ();
			OrderUI.instance.setGeneralMessage ("Burrito Trashed");
		}
	}
}
