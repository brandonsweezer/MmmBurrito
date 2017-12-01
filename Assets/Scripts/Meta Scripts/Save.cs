using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save {


	public List<int> levelScores = new List<int> ();
	public List<int> levelStars = new List<int> ();
	public int lastLevelCompleted;
	public float volumeMusic;
	public float volumeEffects;

	public Save() {
		lastLevelCompleted = 0;
		for (int i = 0; i < 5; i++) {
			levelScores.Add (-1);
			levelStars.Add (-1);
		}
		volumeMusic = 1f;
		volumeEffects = 1f;
	}

}
