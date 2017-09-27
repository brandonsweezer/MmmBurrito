using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	public Camera menuCamera;

	public void goToLevel(int levelNumber) {
		SceneManager.LoadScene ("Level_"+levelNumber);

		OrderController.instance.orderList.Clear ();
		switch (levelNumber) {
			case 1:
				OrderController.instance.AddOrder (1, 1);
				break;
			case 2:
				OrderController.instance.AddOrder (1, 1);
				OrderController.instance.AddOrder (1, 1);
				OrderController.instance.AddOrder (2, 1);
				break;
			case 3:
				OrderController.instance.AddOrder (2, 2);
				OrderController.instance.AddOrder (3, 1);
				break;
		}
		Debug.Log (OrderController.instance.OrderListToString ());


	}

	// http://answers.unity3d.com/questions/1174255/since-onlevelwasloaded-is-deprecated-in-540b15-wha.html
	void OnEnable()
	{
		//Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable()
	{
		//Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{	
		if (scene.name.Contains ("Level_")) {
			// Loaded a level.
			InitializeLevel ();
		} else {
			// Loaded a menu.

		}
	}

	void InitializeLevel () {
		// Initialize the game
		SpawnController.instance.SpawnBurrito ();
	}


}
