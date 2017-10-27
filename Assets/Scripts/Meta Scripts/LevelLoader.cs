using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	public GameObject canvasHUD;
	public GameObject canvasMenu;

    private int maxLevelNumber = 6;
	private int loadingLevelNumber;
	private bool inMenu;

	void Awake () {
		loadingLevelNumber = -1;
		inMenu = true;
	}

    public void GoToNextLevel()
    {
        //logging level end
        LoggingManager.instance.RecordEvent(7, "Level_" + loadingLevelNumber + " quit, timer at " + GameController.instance.gameTime);
        LoggingManager.instance.RecordLevelEnd();

        //next level
        loadingLevelNumber++;
        SceneManager.LoadScene("Level_" + loadingLevelNumber);
        LoggingManager.instance.RecordLevelStart(loadingLevelNumber, "");
    }

	public void GoToLevel(int levelNumber) {
		inMenu = false;
		loadingLevelNumber = levelNumber;
		SceneManager.LoadScene ("Level_"+levelNumber);
        LoggingManager.instance.RecordLevelStart(levelNumber, "");
    }

	public void GoToMenu () {
		inMenu = true;
        LoggingManager.instance.RecordEvent(7, "Level_" + loadingLevelNumber + " quit, timer at " + GameController.instance.gameTime);
        SceneManager.LoadScene ("LevelSelection");
        LoggingManager.instance.RecordLevelEnd();
    }

	public void ReplayLevel()
	{
		SceneManager.LoadScene("Level_" + loadingLevelNumber);
		LoggingManager.instance.RecordLevelStart(loadingLevelNumber, "");
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
			InitializeLevel (loadingLevelNumber);
			SetActiveCanvas ();
		} else {
			// Loaded a menu.
			SetActiveCanvas ();
		}
	}

	void SetActiveCanvas () {
		canvasHUD.SetActive(!inMenu);
		canvasMenu.SetActive(inMenu);
	}

	void InitializeLevel (int levelNumber) {
		SetupLevelVars (levelNumber);

		SpawnController.instance.SpawnBurrito ();

		OrderUI.instance.ResetUI();

		GetComponent<Timer> ().startTimer ();

		MovementControllerIsometricNew.UpdateViewpointRotation ();
	}

	// Setup the level variables for the specified level.
	void SetupLevelVars (int levelNumber) {
		
		GameController.instance.levelComplete = false;
		GameController.instance.score = 0;
		OrderController.instance.orderList.Clear ();
		Timer timer = GetComponent<Timer> ();
		// Updates whether we can submit successfully or not
		GameController.instance.UpdateSubmissionValidity();

		switch (levelNumber) {
		// Intro
		case 1:
			timer.TimerInit (120);
			OrderController.instance.AddOrder (
				IngredientSet.Ingredients.Tomato, 1
			);
			break;
		// 2 ingredients
		case 2:
			timer.TimerInit (60);
			OrderController.instance.AddOrder (
				IngredientSet.Ingredients.Tomato, 1,
				IngredientSet.Ingredients.Cheese, 1
			);
			break;
		// 3 ingredients
		case 3:
			timer.TimerInit (70);
			OrderController.instance.AddOrder (
				IngredientSet.Ingredients.Tomato, 2,
				IngredientSet.Ingredients.Cheese, 1,
				IngredientSet.Ingredients.Meatball, 1
			);
			break;
		// 4 ingredients + multiple orders
		case 4:
			timer.TimerInit (80);
			OrderController.instance.AddOrder (
				IngredientSet.Ingredients.Tomato, 1,
				IngredientSet.Ingredients.Cheese, 1
			);
			OrderController.instance.AddOrder (
				IngredientSet.Ingredients.Tomato, 1,
				IngredientSet.Ingredients.Meatball, 1
			);
			OrderController.instance.AddOrder (
				IngredientSet.Ingredients.Meatball, 1,
				IngredientSet.Ingredients.Lettuce, 1
			);
			break;
		case 5:
			timer.TimerInit (90);
			OrderController.instance.AddOrder (
				IngredientSet.Ingredients.Tomato, 1,
				IngredientSet.Ingredients.Cheese, 1,
				IngredientSet.Ingredients.Meatball, 1
			);
			OrderController.instance.AddOrder (
				IngredientSet.Ingredients.Lettuce, 1,
				IngredientSet.Ingredients.Cheese, 1,
				IngredientSet.Ingredients.Meatball, 1
			);
			OrderController.instance.AddOrder (
				IngredientSet.Ingredients.Tomato, 1,
				IngredientSet.Ingredients.Meatball, 1
			);
			break;
		case 6:
			timer.TimerInit (90);
			OrderController.instance.AddOrder (
				IngredientSet.Ingredients.Tomato, 1,
				IngredientSet.Ingredients.Cheese, 1,
				IngredientSet.Ingredients.Meatball, 1
			);
			break;
        case 7:
            timer.TimerInit(20);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Rice, 2
                );
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Beans, 1,
                IngredientSet.Ingredients.Cheese, 1
                );
            break;
        case 8:
            timer.TimerInit(60);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Cheese, 1,
                IngredientSet.Ingredients.Tomato, 1
                );
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Tomato, 1
                );
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Cheese, 1
                );
            break;
        case 9:
			timer.TimerInit (60);
			OrderController.instance.AddOrder (
				IngredientSet.Ingredients.Lettuce, 1,
				IngredientSet.Ingredients.Cheese, 1,
				IngredientSet.Ingredients.Meatball, 1
			);
			OrderController.instance.AddOrder (
				IngredientSet.Ingredients.Lettuce, 1,
				IngredientSet.Ingredients.Meatball, 1
			);
			OrderController.instance.AddOrder (
				IngredientSet.Ingredients.Cheese, 1,
				IngredientSet.Ingredients.Meatball, 1
			);
			break;
        case 10:
			timer.TimerInit (90);
			OrderController.instance.AddOrder (
				IngredientSet.Ingredients.Tomato, 1
			);
            OrderController.instance.AddOrder (
				IngredientSet.Ingredients.Tomato, 1
			);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Meatball, 1,
                IngredientSet.Ingredients.Cheese, 1,
                IngredientSet.Ingredients.Tomato, 1
            );
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Meatball, 1,
                IngredientSet.Ingredients.Beans, 1,
                IngredientSet.Ingredients.Tomato, 1
            );
                break;
        case 11:
			timer.TimerInit (20);
			OrderController.instance.AddOrder (
				IngredientSet.Ingredients.Lettuce
			);
			break;
		}

		Debug.Log (OrderController.instance.OrderListToString ());
	}

	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && !inMenu) {
            GoToMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Return) && !inMenu && GameController.instance.levelComplete)
        {
            if (loadingLevelNumber != maxLevelNumber)
            {
                GoToNextLevel();
            }
        }
	}


}
