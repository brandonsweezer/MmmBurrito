using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager : MonoBehaviour {

	// The save file
	private Save save;

	// Make this class a singleton
	public static SaveManager instance = null;
	void Awake () {
        if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (this);
		}

		// load game
		LoadGame ();
	}

	// Loads the save file, or creates a new one if it doesn't exist.
	public void LoadGame() {
		Debug.Log ("Saving game at: "+Application.persistentDataPath);
		if (File.Exists(Application.persistentDataPath + "/gamesave.save")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
			save = (Save)bf.Deserialize(file);
			file.Close();
			Debug.Log ("SaveManager: Game loaded. Last level completed: "+save.lastLevelCompleted+" with highscore: "+GetLevelHighscore(save.lastLevelCompleted)+" and num stars: "+GetLevelStars(save.lastLevelCompleted));
		}
		else {
			Debug.Log("SaveManager: No game saved, creating new save file.");
			SaveEmptyData ();
		}
	}

	public void SaveGame() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
		bf.Serialize(file, save);
		file.Close();
		Debug.Log ("Game saved.");
	}

	public void SetLastLevelCompleted(int i) {
		save.lastLevelCompleted = i;
	}
	public int GetLastLevelCompleted() {
		return save.lastLevelCompleted;
	}

	public void SetLevelHighscore(int levelNumber, int highscore) {
		PadLevelArrays (levelNumber);
		save.levelScores[levelNumber] = highscore;
	}
	public int GetLevelHighscore(int levelNumber) {
		PadLevelArrays (levelNumber);
		return save.levelScores[levelNumber];
	}


	public void SetLevelStars(int levelNumber, int stars) {
		PadLevelArrays (levelNumber);
		save.levelStars[levelNumber] = stars;
	}
	public int GetLevelStars(int levelNumber) {
		PadLevelArrays (levelNumber);
		return save.levelStars[levelNumber];
	}

    public int totalStars()
    {
        int stars = 0;
        foreach(int i in save.levelStars)
        {
            stars += i;
        }
        return stars;
    }

	public void printStars() {
		foreach (int i in save.levelStars) {
		}
	}

	private void PadLevelArrays(int levelNumber) {
		while (save.levelScores.Count < levelNumber+1) {
			save.levelScores.Add (-1);
		}
		while (save.levelStars.Count < levelNumber+1) {
			save.levelStars.Add (-1);
		}
	}

	public void ProcessLevelCompletion(int levelNumber, int score, int numStars) {
		bool shouldSave = false;

		if (levelNumber > GetLastLevelCompleted()) {
			SetLastLevelCompleted (levelNumber);
			shouldSave = true;
			Debug.Log ("Completed a new level!");
		}

		PadLevelArrays (levelNumber);

		if (score > GetLevelHighscore (levelNumber)) {
			SetLevelHighscore (levelNumber, score);
			shouldSave = true;
			Debug.Log ("New Highscore for level "+levelNumber+"!");
		}

		if (numStars > GetLevelStars (levelNumber)) {
			SetLevelStars (levelNumber, numStars);
			shouldSave = true;
			Debug.Log ("New highest number of stars for level "+levelNumber+"!");
		}

		if (shouldSave) {
			Debug.Log ("save");
			SaveGame ();
		}
	}

	public void ClearData() {
		SaveEmptyData ();
		Debug.Log ("Cleared data!");
	}

	private void SaveEmptyData() {
		save = new Save();
		SaveGame ();
	}
}
