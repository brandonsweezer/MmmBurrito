using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupAddTime : MonoBehaviour {

	public float secondsToAdd;

	void OnCollisionEnter (Collision collision) {
		GameObject gameObj = collision.gameObject;
		if (gameObj.tag == "Player") {
			GameController.instance.gameObject.GetComponent<Timer> ().AddSeconds (secondsToAdd);
            LoggingManager.instance.RecordEvent(9, "Powerup: Added Time");
        }
    }

}
