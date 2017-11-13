using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save {


	public List<int> levelScores = new List<int> ();
	public int lastLevelCompleted;

	public Save() {
		lastLevelCompleted = 0;
		for (int i = 0; i < 5; i++) {
			levelScores.Add (-1);
		}
	}

}
