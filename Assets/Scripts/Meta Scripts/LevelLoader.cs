using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	public GameObject canvasHUD;
	public GameObject canvasHome;
	public GameObject canvasLevelSelect;
	public GameObject endHUD;
	public GameObject privacy;
	public GameObject pause;


    private int maxLevelNumber = 22;
	private int loadingLevelNumber;
	private bool inMenuHome;
	private bool inMenuLevelSelect;
	private bool play;



	void Awake () {
		loadingLevelNumber = -1;
		SetHomeCanvas();
        //Level 1
        GameController.instance.starUnlock.Add(0);
        //Level 2
        GameController.instance.starUnlock.Add(0);
        //Level 3
        GameController.instance.starUnlock.Add(0);
        //Level 4
        GameController.instance.starUnlock.Add(0);
        //Level 5
        GameController.instance.starUnlock.Add(0);
        //Level 6
        GameController.instance.starUnlock.Add(0);
        //Level 7
        GameController.instance.starUnlock.Add(0);
        //Level 8
        GameController.instance.starUnlock.Add(0);
        //Level 9
        GameController.instance.starUnlock.Add(0);
        //Level 10
        GameController.instance.starUnlock.Add(0);
        //Level 11
        GameController.instance.starUnlock.Add(0);
        //Level 12
        GameController.instance.starUnlock.Add(0);
        //Level 13
        GameController.instance.starUnlock.Add(0);
        //Level 14
        GameController.instance.starUnlock.Add(0);
        //Level 15
        GameController.instance.starUnlock.Add(0);
        //Level 16
        GameController.instance.starUnlock.Add(0);
        //Level 17
        GameController.instance.starUnlock.Add(0);
        //Level 18
        GameController.instance.starUnlock.Add(0);
        //Level 19
        GameController.instance.starUnlock.Add(0);
        //Level 20
        GameController.instance.starUnlock.Add(0);
        //Level 21
        GameController.instance.starUnlock.Add(0);
        //Level 22
        GameController.instance.starUnlock.Add(0);
    }

    public void GoToNextLevel()
    {
        //logging level end
        LoggingManager.instance.RecordLevelEnd();

        //next level
        loadingLevelNumber++;
        GameController.instance.levelEnd = false;
        //TODOX3: ADD LEVELS FOR AB TESTING HERE
        if (GameController.instance.ABValue == 2 && loadingLevelNumber == 0)
        {
            LoggingManager.instance.RecordLevelStart(loadingLevelNumber+100, "With Powerups");
            SceneManager.LoadScene("Level_A" + loadingLevelNumber);
        }
        else
        {
            LoggingManager.instance.RecordLevelStart(loadingLevelNumber, "No Powerups");
            SceneManager.LoadScene("Level_" + loadingLevelNumber);
        }
    }

	public void GoToLevel(int levelNumber) {
		//SetPlayCanvas ();
		loadingLevelNumber = levelNumber;
        GameController.instance.levelEnd = false;
        //TODO
        if (GameController.instance.ABValue == 2 && loadingLevelNumber == 0)
        {
            LoggingManager.instance.RecordLevelStart(loadingLevelNumber+100, "With Powerups");
            SceneManager.LoadScene("Level_A" + loadingLevelNumber);
        }
        else
        {
            LoggingManager.instance.RecordLevelStart(loadingLevelNumber, "No Powerups");
            SceneManager.LoadScene("Level_" + loadingLevelNumber);
        }
    }

	public void GoToMenuLevelSelect () {
		SetLevelSelectCanvas();
		GoToMenu ();
    }

	public void GoToMenuMain () {
		SetHomeCanvas();
		GoToMenu ();
	}

	public void GoToMenuPrivacy () {
		SetPrivacyCanvas();
		GoToMenu ();
	}

	private void GoToMenu() {
		if (!GameController.instance.levelEnd)
		{
			LoggingManager.instance.RecordEvent(7, "Level quit, timer at " + GameController.instance.gameTime);
		}
		LoggingManager.instance.RecordLevelEnd();
		SceneManager.LoadScene("Menu");
	}

	public void GoToPause () {
		SetPauseCanvas();
		OrderUI.instance.textfields.currentLevel.text = "Level "+GameController.instance.currentLevel;
		GameController.instance.gamestate = GameController.GameState.Pause;
	}

	public void Resume() {
		SetResumeCanvas();
		GameController.instance.gamestate = GameController.GameState.Play;
	}

	public void ReplayLevel()
	{
        LoggingManager.instance.RecordLevelEnd();
        GameController.instance.levelEnd = false;
        //TODO
        if (GameController.instance.ABValue == 2 && loadingLevelNumber == 0)
        {
            LoggingManager.instance.RecordLevelStart(loadingLevelNumber+100, "With Powerups");
            SceneManager.LoadScene("Level_A" + loadingLevelNumber);
        }
        else
        {
            LoggingManager.instance.RecordLevelStart(loadingLevelNumber, "No Powerups");
            SceneManager.LoadScene("Level_" + loadingLevelNumber);
        }
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
			SetPlayCanvas ();
			GameController.instance.gamestate = GameController.GameState.Play;
			InitializeLevel (loadingLevelNumber);
		}
		else {
			// Loaded a menu.
			//SetHomeCanvas ();
			GameController.instance.gamestate = GameController.GameState.Menu;
		}
	}

	void SetHomeCanvas () {
		canvasHUD.SetActive (false);
		canvasLevelSelect.SetActive (false);
		endHUD.SetActive (false);
		privacy.SetActive (false);
		pause.SetActive (false);
		canvasHome.SetActive (true);
	}

	void SetLevelSelectCanvas () {
		canvasHUD.SetActive (false);
		canvasHome.SetActive (false);
		privacy.SetActive (false);
		endHUD.SetActive (false);
		pause.SetActive (false);
		canvasLevelSelect.SetActive (true);
	}

	void SetPlayCanvas () {
		canvasHome.SetActive (false);
		canvasLevelSelect.SetActive (false);
		privacy.SetActive (false);
		pause.SetActive (false);
		endHUD.SetActive (true);
		canvasHUD.SetActive (true);
	}

	void SetPrivacyCanvas () {
		canvasHome.SetActive (false);
		canvasLevelSelect.SetActive (false);
		endHUD.SetActive (false);
		canvasHUD.SetActive (false);
		pause.SetActive (false);
		privacy.SetActive (true);
	}

	void SetPauseCanvas () {
		pause.SetActive (true);
	}

	void SetResumeCanvas () {
		pause.SetActive (false);
	}


	void InitializeLevel (int levelNumber) {
		GameController.instance.currentLevel = levelNumber;

		SetupLevelVars (levelNumber);

		SpawnController.instance.SpawnBurrito ();

		OrderUI.instance.ResetUI();
		OrderUI.instance.ResetScore ();

		GetComponent<Timer> ().startTimer ();

		MovementControllerIsometricNew.UpdateViewpointRotation ();
	}

	// Setup the level variables for the specified level.
	void SetupLevelVars (int levelNumber) {
		
		//GameController.instance.levelComplete = false;
		GameController.instance.gamestate = GameController.GameState.Play;

		GameController.instance.SetScore(0);
		OrderController.instance.orderList.Clear ();
		Timer timer = GetComponent<Timer> ();
		// Updates whether we can submit successfully or not
		OrderUI.instance.UpdateUIAfterInventoryChange();

        switch (levelNumber)
        {
        // Intro
        case 1:
            timer.TimerInit(60);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Tomato, 1
            );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(100);
            GameController.instance.starScore.Add(200);
            GameController.instance.starScore.Add(300);
            break;
        // 2 ingredients
        case 2:
            timer.TimerInit(60);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Tomato, 1
            );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(100);
            GameController.instance.starScore.Add(200);
            GameController.instance.starScore.Add(300);
            break;
        // 3 ingredients + dash
        case 3:
            timer.TimerInit(60);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Cheese, 1
            );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(100);
            GameController.instance.starScore.Add(200);
            GameController.instance.starScore.Add(300);
            break;
        // 4 ingredients + multiple orders + MMM
        case 4:
            timer.TimerInit(5);
            OrderController.instance.AddOrder(
				IngredientSet.Ingredients.Meatball, 1
            );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(100);
            GameController.instance.starScore.Add(200);
            GameController.instance.starScore.Add(300);
            break;
        case 5:
            timer.TimerInit(90);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Tomato, 1
            );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(100);
            GameController.instance.starScore.Add(200);
            GameController.instance.starScore.Add(300);
            break;
        case 6:
            timer.TimerInit(90);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Tomato, 1,
                IngredientSet.Ingredients.Cheese, 1,
                IngredientSet.Ingredients.Meatball, 1
            );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(100);
            GameController.instance.starScore.Add(200);
            GameController.instance.starScore.Add(300);
            break;
        case 7:
            if (GameController.instance.ABValue == 2)
            {
                timer.TimerInit(10); //FILLER CODE (EXAMPLE)
            }
            else
            {
                timer.TimerInit(20);
            }
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Rice, 2
                );
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Beans, 1,
                IngredientSet.Ingredients.Cheese, 1
                );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(100);
            GameController.instance.starScore.Add(200);
            GameController.instance.starScore.Add(300);
            break;
        case 8:
            timer.TimerInit(45);
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
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(100);
            GameController.instance.starScore.Add(200);
            GameController.instance.starScore.Add(300);
            break;
        case 9:
            timer.TimerInit(60);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Cheese, 2
            );
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Lettuce, 1,
                IngredientSet.Ingredients.Meatball, 1,
                IngredientSet.Ingredients.Cheese, 1
            );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(100);
            GameController.instance.starScore.Add(200);
            GameController.instance.starScore.Add(300);
            break;
        case 10:
            timer.TimerInit(20);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Lettuce, 1
            );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(100);
            GameController.instance.starScore.Add(200);
            GameController.instance.starScore.Add(300);
            break;
        case 11:
            timer.TimerInit(120);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Lettuce, 1,
                IngredientSet.Ingredients.Cheese, 1,
                IngredientSet.Ingredients.Meatball, 1
            );
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Lettuce, 1,
                IngredientSet.Ingredients.Meatball, 1
            );
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Cheese, 1,
                IngredientSet.Ingredients.Meatball, 1
            );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(100);
            GameController.instance.starScore.Add(200);
            GameController.instance.starScore.Add(300);
            break;
        case 12:
            timer.TimerInit(90);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Tomato, 1
            );
            OrderController.instance.AddOrder(
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
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(100);
            GameController.instance.starScore.Add(200);
            GameController.instance.starScore.Add(300);
            break;
    }

        Debug.Log (OrderController.instance.OrderListToString ());
	}

	void Update () {
//        if (Input.GetKeyDown(KeyCode.Escape) && !inMenuHome) {
//			GoToPause();
//        }
//        else if (Input.GetKeyDown(KeyCode.Return) && !inMenuHome && GameController.instance.levelComplete)
//        {
//            if (loadingLevelNumber != maxLevelNumber)
//            {
//                GoToNextLevel();
//            }
//        }
		if (Input.GetKeyDown(KeyCode.P) && GameController.instance.gamestate==GameController.GameState.Play) {
			GoToPause();
		}
		else if (Input.GetKeyDown(KeyCode.P) && GameController.instance.gamestate==GameController.GameState.Pause) {
			Resume ();
		}
		else if (Input.GetKeyDown(KeyCode.Escape) && 
			(GameController.instance.gamestate==GameController.GameState.Win || GameController.instance.gamestate==GameController.GameState.Lose)) {
			GoToMenuLevelSelect (); 
		}
		else if (Input.GetKeyDown(KeyCode.R) && 
			(GameController.instance.gamestate!=GameController.GameState.Menu || GameController.instance.gamestate!=GameController.GameState.Pause)) {
			ReplayLevel ();
		}
		else if (Input.GetKeyDown(KeyCode.Return) && GameController.instance.gamestate==GameController.GameState.Win)
		{
			if (loadingLevelNumber != maxLevelNumber)
			{
				GoToNextLevel();
			}
		}

	}


}
