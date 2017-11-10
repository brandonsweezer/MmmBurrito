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
		if (File.Exists(Application.persistentDataPath + "/gamesave.save")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
			save = (Save)bf.Deserialize(file);
			file.Close();
			Debug.Log ("SaveManager: Game loaded. Last level completed: "+save.lastLevelCompleted);
		}
		else {
			Debug.Log("SaveManager: No game saved, creating new save file.");
			save = new Save();
			save.lastLevelCompleted = 0;
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

	public void SetLastLevelCompleted(int i ) {
		save.lastLevelCompleted = i;
		SaveGame ();
	}
	public int GetLastLevelCompleted() {
		return save.lastLevelCompleted;
	}
}
