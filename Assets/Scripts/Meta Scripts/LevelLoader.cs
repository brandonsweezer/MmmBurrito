using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	public GameObject canvasHUD;
	public GameObject canvasHome;
	public GameObject canvasLevelSelect;
	public GameObject canvasLevelEnd;
	public GameObject canvasPrivacy;
	public GameObject canvasPause;
	public GameObject canvasSetting;


    private int maxLevelNumber = 22;
	private int loadingLevelNumber;
	private bool inMenuHome;
	private bool inMenuLevelSelect;
	private bool play;


	// Make this class a singleton
	public static LevelLoader instance = null;

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

		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (this);
		}
    }

    public void GoToNextLevel()
    {
        //logging level end
        LoggingManager.instance.RecordLevelEnd();

        //next level
        loadingLevelNumber++;
        GameController.instance.levelEnd = false;
        LoggingManager.instance.RecordLevelStart(loadingLevelNumber, "");
        SceneManager.LoadScene("Level_" + loadingLevelNumber);
    }

	public void GoToLevel(int levelNumber) {
        //SetPlayCanvas ();
        SoundController.instance.audSrc.Stop();
        SoundController.instance.audSrc.clip = SoundController.instance.music;
        SoundController.instance.audSrc.loop = true;
        SoundController.instance.audSrc.Play();

        loadingLevelNumber = levelNumber;
        GameController.instance.levelEnd = false;
        LoggingManager.instance.RecordLevelStart(loadingLevelNumber, "");
        SceneManager.LoadScene("Level_" + loadingLevelNumber);
    }


	public void PlayLatestLevel  () {
		int levelNum = (int)Mathf.Min (maxLevelNumber, SaveManager.instance.GetLastLevelCompleted () + 1);
		Debug.Log ("next level is " + levelNum);
		GoToLevel (levelNum);
	}

	public void GoToMenuLevelSelect () {
		SaveManager.instance.printStars ();
		SetLevelSelectCanvas();
		GoToMenu ();
		FillStars ();
    }

	public void GoToMenuMain () {
		SetHomeCanvas();
		GoToMenu ();
        SoundController.instance.audSrc.Stop(); 
        SoundController.instance.audSrc.clip = SoundController.instance.music;
        SoundController.instance.audSrc.loop = true;
        SoundController.instance.audSrc.Play();
    }

	public void GoToMenuPrivacy () {
		SetPrivacyCanvas();
		GoToMenu ();
	}

	public void GoToMenuSetting () {
		SetSettingCanvas();
		GoToMenu ();
	}

	private void GoToMenu() {
		if (!GameController.instance.levelEnd)
		{
			LoggingManager.instance.RecordEvent(7, "Level quit, timer at " + GameController.instance.gameTime);
		}
		LoggingManager.instance.RecordLevelEnd();
		SceneManager.LoadScene("Menu");
		SoundController.instance.audSrc.Stop();
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
        LoggingManager.instance.RecordLevelStart(loadingLevelNumber, "");
        SceneManager.LoadScene("Level_" + loadingLevelNumber);
    }

	public void FillStars () {

		for (int i=0; i<maxLevelNumber; i ++) {
			canvasLevelSelect.transform.GetChild (3 + i).GetChild(1).GetChild(1).GetComponent<Image> ().sprite = OrderUI.instance.gameobjectfields.EmptyStar;
			canvasLevelSelect.transform.GetChild (3 + i).GetChild(1).GetChild (1).GetComponent<Image> ().color = Color.black;
			canvasLevelSelect.transform.GetChild (3 + i).GetChild(1).GetChild(0).GetComponent<Image> ().sprite = OrderUI.instance.gameobjectfields.EmptyStar;
			canvasLevelSelect.transform.GetChild (3 + i).GetChild(1).GetChild (0).GetComponent<Image> ().color = Color.black;
			canvasLevelSelect.transform.GetChild (3 + i).GetChild(1).GetChild(2).GetComponent<Image> ().sprite = OrderUI.instance.gameobjectfields.EmptyStar;
			canvasLevelSelect.transform.GetChild (3 + i).GetChild(1).GetChild (2).GetComponent<Image> ().color = Color.black;

			int stars=SaveManager.instance.GetLevelStars (i);

			if (stars >= 1) {
				canvasLevelSelect.transform.GetChild (2 + i).GetChild(1).GetChild(1).GetComponent<Image> ().sprite = OrderUI.instance.gameobjectfields.FilledStar;
				canvasLevelSelect.transform.GetChild (2 + i).GetChild(1).GetChild (1).GetComponent<Image> ().color = Color.white;
			}
			if (stars >= 2) {
				canvasLevelSelect.transform.GetChild (2 + i).GetChild(1).GetChild(0).GetComponent<Image> ().sprite = OrderUI.instance.gameobjectfields.FilledStar;
				canvasLevelSelect.transform.GetChild (2 + i).GetChild(1).GetChild (0).GetComponent<Image> ().color = Color.white;
			}
			if (stars >= 3) {
				canvasLevelSelect.transform.GetChild (2 + i).GetChild(1).GetChild(2).GetComponent<Image> ().sprite = OrderUI.instance.gameobjectfields.FilledStar;
				canvasLevelSelect.transform.GetChild (2 + i).GetChild(1).GetChild (2).GetComponent<Image> ().color = Color.white;
			}
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
			GameController.instance.gamestate = GameController.GameState.Menu;
		}
	}

	void SetHomeCanvas () {
		canvasHUD.SetActive (false);
		canvasLevelSelect.SetActive (false);
		canvasLevelEnd.SetActive (false);
		canvasPrivacy.SetActive (false);
		canvasPause.SetActive (false);
		canvasSetting.SetActive (false);
		canvasHome.SetActive (true);
	}

	void SetLevelSelectCanvas () {
		canvasHUD.SetActive (false);
		canvasHome.SetActive (false);
		canvasPrivacy.SetActive (false);
		canvasLevelEnd.SetActive (false);
		canvasPause.SetActive (false);
		canvasSetting.SetActive (false);
		canvasLevelSelect.SetActive (true);
	}

	void SetPlayCanvas () {
		canvasHome.SetActive (false);
		canvasLevelSelect.SetActive (false);
		canvasPrivacy.SetActive (false);
		canvasPause.SetActive (false);
		canvasSetting.SetActive (false);
		canvasLevelEnd.SetActive (true);
		canvasHUD.SetActive (true);
	}

	void SetPrivacyCanvas () {
		canvasHome.SetActive (false);
		canvasLevelSelect.SetActive (false);
		canvasLevelEnd.SetActive (false);
		canvasHUD.SetActive (false);
		canvasPause.SetActive (false);
		canvasSetting.SetActive (false);
		canvasPrivacy.SetActive (true);
	}

	void SetSettingCanvas () {
		canvasHome.SetActive (false);
		canvasLevelSelect.SetActive (false);
		canvasLevelEnd.SetActive (false);
		canvasHUD.SetActive (false);
		canvasPause.SetActive (false);
		canvasPrivacy.SetActive (false);
		canvasSetting.SetActive (true);
	}

	void SetPauseCanvas () {
		canvasPause.SetActive (true);
	}

	void SetResumeCanvas () {
		canvasPause.SetActive (false);
	}

	public void SetEndCanvas () {
		canvasHUD.SetActive (false);
	}




	void InitializeLevel (int levelNumber) {
		GameController.instance.currentLevel = levelNumber;

		SetupLevelVars (levelNumber);

		SpawnController.instance.SpawnBurrito ();

		OrderUI.instance.ResetUI();
		OrderUI.instance.ResetScore();

		// reset final scoring screen stars
		OrderUI.instance.gameobjectfields.WinScreen.transform.GetChild (0).GetComponent<Image> ().sprite = OrderUI.instance.gameobjectfields.EmptyStar;
		OrderUI.instance.gameobjectfields.WinScreen.transform.GetChild (1).GetComponent<Image> ().sprite = OrderUI.instance.gameobjectfields.EmptyStar;
		OrderUI.instance.gameobjectfields.WinScreen.transform.GetChild (2).GetComponent<Image> ().sprite = OrderUI.instance.gameobjectfields.EmptyStar;

		OrderUI.instance.DeleteTweeningObjects ();

		// disable timer for levels that don't use it
		if (levelNumber >= 4) {
			OrderUI.instance.textfields.timeRemaining.gameObject.SetActive (true);
			Timer.instance.TimerObject.SetActive (true);
			GetComponent<Timer> ().startTimer ();
		} else {
			OrderUI.instance.textfields.timeRemaining.gameObject.SetActive (false);
			Timer.instance.TimerObject.SetActive (false);
		}

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
            GameController.instance.starScore.Add(25);
            GameController.instance.starScore.Add(75);
            GameController.instance.starScore.Add(100);
            break;
        // 2 ingredients
        case 2:
            timer.TimerInit(60);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Tomato, 1
            );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(25);
            GameController.instance.starScore.Add(75);
            GameController.instance.starScore.Add(100);
            break;
        // 3 ingredients + dash
        case 3:
            timer.TimerInit(60);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Cheese, 1
            );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(25);
            GameController.instance.starScore.Add(75);
            GameController.instance.starScore.Add(100);
            break;
        // 4 ingredients + multiple orders + MMM
        case 4:
            timer.TimerInit(5);
            OrderController.instance.AddOrder(
				IngredientSet.Ingredients.Meatball, 1
            );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(25);
            GameController.instance.starScore.Add(75);
            GameController.instance.starScore.Add(100);
            break;
        case 5:
            timer.TimerInit(90);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Tomato, 1
            );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(25);
            GameController.instance.starScore.Add(75);
            GameController.instance.starScore.Add(100);
            break;
		case 6:
			timer.TimerInit(90);
			OrderController.instance.AddOrder(
				IngredientSet.Ingredients.Lettuce, 1
			);
			GameController.instance.starScore.Clear();
			GameController.instance.starScore.Add(25);
			GameController.instance.starScore.Add(75);
			GameController.instance.starScore.Add(100);
			break;
        case 7:
                timer.TimerInit(10);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Cheese, 1
            );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(25);
            GameController.instance.starScore.Add(75);
            GameController.instance.starScore.Add(100);
            break;
        case 8:
            timer.TimerInit(40);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Rice, 1
            );
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Tomato, 1
            );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(100);
            GameController.instance.starScore.Add(150);
            GameController.instance.starScore.Add(200);
            break;

        case 9:
            timer.TimerInit(45);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Lettuce, 1,
                IngredientSet.Ingredients.Cheese, 1,
                IngredientSet.Ingredients.Tomato, 1
            );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(150);
            GameController.instance.starScore.Add(250);
            GameController.instance.starScore.Add(300);
            break;

        case 10:
			timer.TimerInit(60);
			OrderController.instance.AddOrder(
				IngredientSet.Ingredients.Rice, 1,
				IngredientSet.Ingredients.Meatball, 1
			);
            OrderController.instance.AddOrder(
				IngredientSet.Ingredients.Lettuce, 1,
				IngredientSet.Ingredients.Tomato, 1
            );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(200);
            GameController.instance.starScore.Add(300);
            GameController.instance.starScore.Add(400);
            break;
        case 11:
            timer.TimerInit(60);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Lettuce, 1,
                IngredientSet.Ingredients.Cheese, 1
            );
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Tomato, 2
            );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(200);
            GameController.instance.starScore.Add(300);
            GameController.instance.starScore.Add(400);
            break;
        case 12:
            timer.TimerInit(90);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Lettuce, 1
            );
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Tomato, 1
            );
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Tomato, 1,
                IngredientSet.Ingredients.Cheese, 1,
                IngredientSet.Ingredients.Lettuce, 1
            );
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Cheese, 1,
                IngredientSet.Ingredients.Cheese, 1,
                IngredientSet.Ingredients.Cheese, 1
            );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(400);
            GameController.instance.starScore.Add(700);
            GameController.instance.starScore.Add(800);
            break;

        case 13:
            timer.TimerInit(90);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Cheese, 1,
                IngredientSet.Ingredients.Lettuce, 1
            );
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Tomato, 1,
                IngredientSet.Ingredients.Lettuce, 1
            );
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Cheese, 1,
                IngredientSet.Ingredients.Tomato, 1,
                IngredientSet.Ingredients.Lettuce, 1
            );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(300);
            GameController.instance.starScore.Add(400);
            GameController.instance.starScore.Add(600);
            break;

		case 14:
			timer.TimerInit(12);
			OrderController.instance.AddOrder(
				IngredientSet.Ingredients.Tomato, 2,
				IngredientSet.Ingredients.Cheese, 1,
				IngredientSet.Ingredients.Lettuce, 1
			);
			GameController.instance.starScore.Clear();
			GameController.instance.starScore.Add(200);
			GameController.instance.starScore.Add(300);
			GameController.instance.starScore.Add(400);
			break;
        case 15:
            timer.TimerInit(45);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Tomato, 1,
                IngredientSet.Ingredients.Lettuce, 1
            );
            OrderController.instance.AddOrder(
            IngredientSet.Ingredients.Cheese, 1,
            IngredientSet.Ingredients.Meatball, 1
            );
                GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(200);
            GameController.instance.starScore.Add(300);
            GameController.instance.starScore.Add(400);
            break;

            case 16:
            timer.TimerInit(90);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Meatball, 1,
                IngredientSet.Ingredients.Cheese, 1,
                IngredientSet.Ingredients.Tomato, 1
            );
                OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Lettuce, 1,
                IngredientSet.Ingredients.Meatball, 1
            );
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Lettuce, 1,
                IngredientSet.Ingredients.Lettuce, 1,
                IngredientSet.Ingredients.Tomato, 1
            );
                GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(400);
            GameController.instance.starScore.Add(550);
            GameController.instance.starScore.Add(700);
            break;

            case 17:
                timer.TimerInit(90);
                OrderController.instance.AddOrder(
                    IngredientSet.Ingredients.Meatball, 1,
                    IngredientSet.Ingredients.Cheese, 1
                );
                OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Meatball, 1,
                IngredientSet.Ingredients.Lettuce, 1,
                IngredientSet.Ingredients.Tomato, 1
            );
                OrderController.instance.AddOrder(
                    IngredientSet.Ingredients.Cheese, 1,
                    IngredientSet.Ingredients.Tomato, 1,
                    IngredientSet.Ingredients.Lettuce, 1
                );
                GameController.instance.starScore.Clear();
                GameController.instance.starScore.Add(200);
                GameController.instance.starScore.Add(500);
                GameController.instance.starScore.Add(700);
                break;
        case 18:
            timer.TimerInit(90);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Lettuce, 1,
                IngredientSet.Ingredients.Meatball, 1
            );
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Cheese, 1,
                IngredientSet.Ingredients.Meatball, 1
            );
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Tomato, 1,
                IngredientSet.Ingredients.Lettuce, 1
            );
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Cheese, 1,
                IngredientSet.Ingredients.Tomato, 1,
                IngredientSet.Ingredients.Lettuce, 1,
                IngredientSet.Ingredients.Meatball, 1
            );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(500);
            GameController.instance.starScore.Add(700);
            GameController.instance.starScore.Add(900);
            break;
        case 19:
                timer.TimerInit(45);
                OrderController.instance.AddOrder(
                    IngredientSet.Ingredients.Cheese, 1,
                    IngredientSet.Ingredients.Lettuce, 1);
                OrderController.instance.AddOrder(
                    IngredientSet.Ingredients.Cheese, 1);
                GameController.instance.starScore.Clear();
                GameController.instance.starScore.Add(200);
                GameController.instance.starScore.Add(250);
                GameController.instance.starScore.Add(300);
                break;
        case 20:
                timer.TimerInit(90);
                OrderController.instance.AddOrder(
                    IngredientSet.Ingredients.Meatball, 1,
                    IngredientSet.Ingredients.Lettuce, 1,
                    IngredientSet.Ingredients.Tomato, 1);
                GameController.instance.starScore.Clear();
                GameController.instance.starScore.Add(200);
                GameController.instance.starScore.Add(250);
                GameController.instance.starScore.Add(300);
                break;
        case 21:
            timer.TimerInit(90);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Cheese, 1,
                IngredientSet.Ingredients.Beans, 1,
                IngredientSet.Ingredients.Meatball, 1);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Tomato, 2,
                IngredientSet.Ingredients.Meatball, 1);
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(350);
            GameController.instance.starScore.Add(400);
            GameController.instance.starScore.Add(500);
            break;
        case 22:
        	timer.TimerInit(90);
        	OrderController.instance.AddOrder(
        		IngredientSet.Ingredients.Cheese, 1,
        		IngredientSet.Ingredients.Beans, 1,
        		IngredientSet.Ingredients.Meatball, 1,
        		IngredientSet.Ingredients.Tomato, 1,
        		IngredientSet.Ingredients.Lettuce, 1,
        		IngredientSet.Ingredients.Rice, 1);
        	GameController.instance.starScore.Clear();
        	GameController.instance.starScore.Add(300);
        	GameController.instance.starScore.Add(450);
        	GameController.instance.starScore.Add(550);
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
		if ((Input.GetKeyDown(KeyCode.P)||Input.GetKeyDown(KeyCode.Escape)) && GameController.instance.gamestate==GameController.GameState.Play) {
			GoToPause();
		}
		else if (Input.GetKeyDown(KeyCode.P)||Input.GetKeyDown(KeyCode.Escape) && GameController.instance.gamestate==GameController.GameState.Pause) {
			Resume ();
		}
		else if (Input.GetKeyDown(KeyCode.Escape) && 
			(GameController.instance.gamestate==GameController.GameState.Win || GameController.instance.gamestate==GameController.GameState.Lose)) {
			GoToMenuLevelSelect (); 
		}
		else if (Input.GetKeyDown(KeyCode.R) && 
			(GameController.instance.gamestate==GameController.GameState.Pause)) {
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
