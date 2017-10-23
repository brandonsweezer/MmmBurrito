using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoggingCalls : MonoBehaviour {

	private static float positionLogCooldown = 1f;
	private static float lastPositionLog;
	
	// Update is called once per frame
	void Update () {
		LogPlayerPosition ();
	}

	// Logs the player's position if a player exists and enough time has elapsed since the last position logging.
	void LogPlayerPosition () {
		if (GameController.instance.player == null) {
			return;
		}
		if (Time.time - lastPositionLog > positionLogCooldown) {
			lastPositionLog = Time.time;
			Debug.Log ("logged pos");
			string positionLogMessage = "Coordinates: " + GameController.instance.player.transform.position.x + 
				", " + GameController.instance.player.transform.position.z;
			LoggingManager.instance.RecordEvent (0, positionLogMessage);
		}
	}
}
