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
			Debug.Log ("SaveManager: Game loaded. Last level completed: "+save.lastLevelCompleted+" with highscore: "+GetLevelHighscore(save.lastLevelCompleted));
		}
		else {
			Debug.Log("SaveManager: No game saved, creating new save file.");
			save = new Save();
			SaveGame ();
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
		save.levelScores[levelNumber] = highscore;
	}
	public int GetLevelHighscore(int levelNumber) {
		return save.levelScores[levelNumber];
	}

	public void ProcessLevelCompletion(int levelNumber, int score) {
		bool shouldSave = false;

		if (levelNumber > GetLastLevelCompleted()) {
			SetLastLevelCompleted (levelNumber);
			shouldSave = true;
			Debug.Log ("Completed a new level!");
		}

		while (save.levelScores.Count < levelNumber+1) {
			save.levelScores.Add (-1);
		}

		if (score > GetLevelHighscore (levelNumber)) {
			SetLevelHighscore (levelNumber, score);
			shouldSave = true;
			Debug.Log ("New Highscore for level "+levelNumber+"!");
		}

		if (shouldSave) {
			Debug.Log ("save");
			SaveGame ();
		}
	}
}
