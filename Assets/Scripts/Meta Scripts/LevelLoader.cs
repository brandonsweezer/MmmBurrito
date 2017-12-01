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
	public GameObject canvasInstructionsMain;
	public GameObject canvasInstructionsPause;
	public GameObject canvasLevelStart;

	public GameObject[] HUDToHideOnLevelStart;
	public Text levelNumberText;
	IEnumerator levelStartDelayRoutine;


	public int maxLevelNumber = 24;
	public int maxLevelUnlocked = 1; 
	public Button[] levelSet; 
	private int currentStars;
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
        GameController.instance.starUnlock.Add(1);
        //Level 3
        GameController.instance.starUnlock.Add(2);
        //Level 4
        GameController.instance.starUnlock.Add(5);
        //Level 5
        GameController.instance.starUnlock.Add(7);
        //Level 6
        GameController.instance.starUnlock.Add(9);
        //Level 7
        GameController.instance.starUnlock.Add(12);
        //Level 8
        GameController.instance.starUnlock.Add(15);
        //Level 9
        GameController.instance.starUnlock.Add(17);
        //Level 10
        GameController.instance.starUnlock.Add(19);
        //Level 11
        GameController.instance.starUnlock.Add(20);
        //Level 12
        GameController.instance.starUnlock.Add(22);
        //Level 13
        GameController.instance.starUnlock.Add(24);
        //Level 14
        GameController.instance.starUnlock.Add(26);
        //Level 15
        GameController.instance.starUnlock.Add(28);
        //Level 16
        GameController.instance.starUnlock.Add(31);
        //Level 17
        GameController.instance.starUnlock.Add(33);
        //Level 18
        GameController.instance.starUnlock.Add(35);
        //Level 19
        GameController.instance.starUnlock.Add(36);
        //Level 20
        GameController.instance.starUnlock.Add(38);
        //Level 21
        GameController.instance.starUnlock.Add(40);
        //Level 22
        GameController.instance.starUnlock.Add(45);
		//Level 23
		GameController.instance.starUnlock.Add(50);
		//Level 24
		GameController.instance.starUnlock.Add(55);

		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (this);
		}
    }

    public void GoToNextLevel()
	{
		if (loadingLevelNumber == maxLevelNumber) {
			OrderUI.instance.gameobjectfields.WinScreen.gameObject.SetActive (false);
			OrderUI.instance.gameobjectfields.GameCompleteScreen.gameObject.SetActive (true);
			Text summary = OrderUI.instance.gameobjectfields.GameCompleteScreen.transform.GetChild (0).GetChild (0).GetComponent<Text> ();
			summary.text = SaveManager.instance.totalStars ().ToString () + "/" + (maxLevelNumber * 3f).ToString (); 
			return;
		}

        //logging level end
        LoggingManager.instance.RecordLevelEnd();

        //next level
		loadingLevelNumber = loadingLevelNumber+1;
        GameController.instance.levelEnd = false;
        LoggingManager.instance.RecordLevelStart(loadingLevelNumber, "");
        SceneManager.LoadScene("Level_" + loadingLevelNumber);

        /*SoundController.instance.audSrc.Stop();
        SoundController.instance.audSrc.clip = SoundController.instance.music;
        SoundController.instance.audSrc.Play();*/
    }

	public void GoToLevel(int levelNumber) {
		//SetPlayCanvas ();
		loadingLevelNumber = levelNumber;
        GameController.instance.levelEnd = false;
        LoggingManager.instance.RecordLevelStart(loadingLevelNumber, "");
        SceneManager.LoadScene("Level_" + loadingLevelNumber);

        /*SoundController.instance.audSrc.Stop();
        SoundController.instance.audSrc.clip = SoundController.instance.music;
        SoundController.instance.audSrc.Play();*/
    }


	public void PlayLatestLevel  () {
		int levelNum = (int)Mathf.Min (maxLevelNumber, SaveManager.instance.GetLastLevelCompleted () + 1);
			int totalStars= 0;
		for (int i=SaveManager.instance.GetLastLevelCompleted(); i>0; i --) {
				totalStars+=SaveManager.instance.GetLevelStars (i);
			}
		if (totalStars < GameController.instance.starUnlock [levelNum-1]) {
			levelNum -= 1; 
		}


		Debug.Log ("next level is " + levelNum);
		if (levelNum == 1) {
			GoToInstructionsMain ();
		} else {
			GoToLevel (levelNum);
		}
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
            LoggingManager.instance.RecordLevelEnd();
        }
		SceneManager.LoadScene("Menu");
	}

	public void GoToPause () {
		SetPauseCanvas();
		OrderUI.instance.textfields.currentLevel.text = "Level "+GameController.instance.currentLevel;
		GameController.instance.gamestate = GameController.GameState.Pause;
	}

	public void GoToInstructionsMain () {
		SetInstructionCanvasMain();
		StartCoroutine (DisplayInstructions());
	}

	IEnumerator DisplayInstructions() {
		yield return new WaitForSeconds (.01f);
		GameController.instance.gamestate = GameController.GameState.GameStart;
	}

	public void GoToInstructionsPause () {
		SetInstructionCanvasPause();
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


		currentStars = 80;


		for (int i=1; i<=maxLevelNumber; i ++) {
			//Reset Empty Star Sprites
			canvasLevelSelect.transform.GetChild (2 + i).GetChild(0).GetChild(1).GetChild(0).GetComponent<Image> ().sprite = OrderUI.instance.gameobjectfields.EmptyStar;
			canvasLevelSelect.transform.GetChild (2 + i).GetChild(0).GetChild(1).GetChild(0).GetComponent<Image> ().color = Color.black;
			canvasLevelSelect.transform.GetChild (2 + i).GetChild(0).GetChild(1).GetChild(1).GetComponent<Image> ().sprite = OrderUI.instance.gameobjectfields.EmptyStar;
			canvasLevelSelect.transform.GetChild (2 + i).GetChild(0).GetChild(1).GetChild(1).GetComponent<Image> ().color = Color.black;
			canvasLevelSelect.transform.GetChild (2 + i).GetChild(0).GetChild(1).GetChild(2).GetComponent<Image> ().sprite = OrderUI.instance.gameobjectfields.EmptyStar;
			canvasLevelSelect.transform.GetChild (2 + i).GetChild(0).GetChild(1).GetChild(2).GetComponent<Image> ().color = Color.black;

			//Calculates total stars
			int stars=SaveManager.instance.GetLevelStars (i);
			int tempStar;
			if (stars == -1) {
				tempStar = 0;
			} else {
				tempStar = stars;
			}
			currentStars += tempStar; 

			canvasLevelSelect.transform.GetChild (27).GetChild (0).GetComponent<Text> ().text = currentStars.ToString ();

			//Determines if a level is unlocked or not
			Button lvl = (Button)levelSet.GetValue (i-1); 
			//Locked
			if (currentStars < GameController.instance.starUnlock [i-1]) {
				lvl.interactable = false; 
				canvasLevelSelect.transform.GetChild (2 + i).GetChild (0).gameObject.SetActive (false);
				canvasLevelSelect.transform.GetChild (2 + i).GetChild (1).gameObject.SetActive (true);
				canvasLevelSelect.transform.GetChild (2 + i).GetChild (1).GetChild (0).GetComponent<Text> ().text = (GameController.instance.starUnlock [i] - currentStars).ToString(); 

			//Unlocked
			} else {
				lvl.interactable = true; 
				canvasLevelSelect.transform.GetChild (2 + i).GetChild (0).gameObject.SetActive (true);
				canvasLevelSelect.transform.GetChild (2 + i).GetChild (1).gameObject.SetActive (false);

			}

			//Sets the correct number of filled sprites
			if (stars >= 1) {
				canvasLevelSelect.transform.GetChild (2 + i).GetChild(0).GetChild(1).GetChild(0).GetComponent<Image> ().sprite = OrderUI.instance.gameobjectfields.FilledStar;
				canvasLevelSelect.transform.GetChild (2 + i).GetChild(0).GetChild(1).GetChild(0).GetComponent<Image> ().color = Color.white;
			}
			if (stars >= 2) {
				canvasLevelSelect.transform.GetChild (2 + i).GetChild(0).GetChild(1).GetChild(1).GetComponent<Image> ().sprite = OrderUI.instance.gameobjectfields.FilledStar;
				canvasLevelSelect.transform.GetChild (2 + i).GetChild(0).GetChild(1).GetChild(1).GetComponent<Image> ().color = Color.white;
			}
			if (stars >= 3) {
				canvasLevelSelect.transform.GetChild (2 + i).GetChild(0).GetChild(1).GetChild(2).GetComponent<Image> ().sprite = OrderUI.instance.gameobjectfields.FilledStar;
				canvasLevelSelect.transform.GetChild (2 + i).GetChild(0).GetChild(1).GetChild(2).GetComponent<Image> ().color = Color.white;
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
			InitializeLevel (loadingLevelNumber);
		}
		else {
			// Loaded a menu.
			GameController.instance.gamestate = GameController.GameState.Menu;
		}
	}

	//Tunrns off all layers
	void SetAllToFalse() {
		canvasHUD.SetActive (false);
		canvasLevelSelect.SetActive (false);
		canvasLevelEnd.SetActive (false);
		canvasPrivacy.SetActive (false);
		canvasPause.SetActive (false);
		canvasSetting.SetActive (false);
		canvasInstructionsMain.SetActive (false);
		canvasInstructionsPause.SetActive (false);
		canvasHome.SetActive (false);
		canvasLevelStart.SetActive (false);
	}

	void SetHomeCanvas () {
		SetAllToFalse ();
		canvasHome.SetActive (true);
	}

	void SetLevelSelectCanvas () {
		SetAllToFalse ();
		canvasLevelSelect.SetActive (true);
	}

	void SetPlayCanvas () {
		SetAllToFalse ();
		canvasLevelEnd.SetActive (true);
		canvasHUD.SetActive (true);
	}

	void SetPrivacyCanvas () {
		SetAllToFalse ();
		canvasPrivacy.SetActive (true);
	}

	void SetSettingCanvas () {
		SetAllToFalse ();
		canvasSetting.SetActive (true);
	}

	void SetInstructionCanvasMain () {
		SetAllToFalse ();
		canvasInstructionsMain.SetActive (true);
	}

	void SetInstructionCanvasPause () {
		canvasInstructionsPause.SetActive (true);
	}

	void SetPauseCanvas () {
		canvasPause.SetActive (true);
		canvasInstructionsPause.SetActive (false);
	}

	void SetResumeCanvas () {
		canvasPause.SetActive (false);
	}

	public void SetEndCanvas () {
		canvasHUD.SetActive (false);
	}

	public void OpenLevelStartCanvas () {
		canvasLevelStart.SetActive (true);
		LevelStartHideHUD ();
	}

	public void CloseLevelStartCanvas () {
		canvasLevelStart.SetActive (false);
		LevelStartShowHUD ();
	}

	void LevelStartHideHUD() {
		foreach (GameObject g in HUDToHideOnLevelStart) {
			g.SetActive (false);
		}
	}
	void LevelStartShowHUD() {
		foreach (GameObject g in HUDToHideOnLevelStart) {
			g.SetActive (true);
		}
	}



	void InitializeLevel (int levelNumber) {
		GameController.instance.gamestate = GameController.GameState.LevelStart;
		GameController.instance.currentLevel = levelNumber;

		SpawnController.instance.SpawnBurrito ();

		OrderUI.instance.ResetUI();
		OrderUI.instance.ResetScore();
		OrderUI.instance.DeleteTweeningObjects ();

		// reset final scoring screen stars
		OrderUI.instance.gameobjectfields.WinScreen.transform.GetChild (0).GetComponent<Image> ().sprite = OrderUI.instance.gameobjectfields.EmptyStar;
		OrderUI.instance.gameobjectfields.WinScreen.transform.GetChild (1).GetComponent<Image> ().sprite = OrderUI.instance.gameobjectfields.EmptyStar;
		OrderUI.instance.gameobjectfields.WinScreen.transform.GetChild (2).GetComponent<Image> ().sprite = OrderUI.instance.gameobjectfields.EmptyStar;

		//OrderUI.instance.gameobjectfields.GameCompleteScreen.transform.GetChild (1).GetComponent<Image> ().sprite = OrderUI.instance.gameobjectfields.EmptyStar;
		//OrderUI.instance.gameobjectfields.GameCompleteScreen.transform.GetChild (2).GetComponent<Image> ().sprite = OrderUI.instance.gameobjectfields.EmptyStar;
		//OrderUI.instance.gameobjectfields.GameCompleteScreen.transform.GetChild (3).GetComponent<Image> ().sprite = OrderUI.instance.gameobjectfields.EmptyStar;

		MovementControllerIsometricNew.UpdateViewpointRotation ();

		DisableTimerByLevel ();

		SetupLevelVars (levelNumber);

		// level start screen
		OpenLevelStartCanvas ();
		levelNumberText.text = "LEVEL " + levelNumber;
		if (levelStartDelayRoutine != null) {
			StopCoroutine (levelStartDelayRoutine);
		}
		levelStartDelayRoutine = BeginLevelAfterDelay ();
		StartCoroutine (levelStartDelayRoutine);
	}

	IEnumerator BeginLevelAfterDelay() {
		yield return new WaitForEndOfFrame ();
		// Timer.instance.SignalTimeLeft (timerLevelStartPos, timerLevelStartScale, 3, 0, 0.5f);
		if (GameController.instance.currentLevel > 3) {
			Timer.instance.LevelStartSpawnAnimation ();
		}
		yield return new WaitForSeconds(3);
		BeginLevel ();
	}

	void BeginLevel() {
		GameController.instance.gamestate = GameController.GameState.Play;
		CloseLevelStartCanvas();

		GameController.instance.gamestate = GameController.GameState.Play;
		// Updates whether we can submit successfully or not
		OrderUI.instance.UpdateUIAfterInventoryChange();
	}

	void DisableTimerByLevel() {
		// disable timer for levels that don't use it
		if (GameController.instance.currentLevel >= 4) {
			OrderUI.instance.textfields.timeRemainingText.gameObject.SetActive (true);
			OrderUI.instance.textfields.timeRemaining.gameObject.SetActive (true);
			Timer.instance.TimerObject.SetActive (true);
			GetComponent<Timer> ().startTimer ();
		} else {
			OrderUI.instance.textfields.timeRemainingText.gameObject.SetActive (false);
			OrderUI.instance.textfields.timeRemaining.gameObject.SetActive (false);
			Timer.instance.TimerObject.SetActive (false);
		}
	}

	// Setup the level variables for the specified level.
	void SetupLevelVars (int levelNumber) {

		GameController.instance.SetScore(0);
		OrderController.instance.ClearOrders ();
		Timer timer = GetComponent<Timer> ();

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
			timer.TimerInit(60);
			OrderController.instance.AddOrder(
				IngredientSet.Ingredients.Lettuce, 1
			);
			GameController.instance.starScore.Clear();
			GameController.instance.starScore.Add(25);
			GameController.instance.starScore.Add(75);
			GameController.instance.starScore.Add(100);
			break;
        case 7:
                timer.TimerInit(30);
            OrderController.instance.AddOrder(
                IngredientSet.Ingredients.Cheese, 1,
                IngredientSet.Ingredients.Lettuce, 1
            );
            GameController.instance.starScore.Clear();
            GameController.instance.starScore.Add(100);
            GameController.instance.starScore.Add(150);
            GameController.instance.starScore.Add(200);
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
        case 12:
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
        case 13:

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
		case 14:
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
                GameController.instance.starScore.Add(500);
                GameController.instance.starScore.Add(600);
                break;
            case 15:
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
            case 16:
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
            case 23:
                timer.TimerInit(90);
                OrderController.instance.AddOrder(
                    IngredientSet.Ingredients.Beans, 1);
                OrderController.instance.AddOrder(
                    IngredientSet.Ingredients.Cheese, 1);
                OrderController.instance.AddOrder(
                    IngredientSet.Ingredients.Lettuce, 1);
                OrderController.instance.AddOrder(
                    IngredientSet.Ingredients.Tomato, 1);
                OrderController.instance.AddOrder(
                    IngredientSet.Ingredients.Meatball, 1);
                OrderController.instance.AddOrder(
                    IngredientSet.Ingredients.Beans, 1,
                    IngredientSet.Ingredients.Cheese, 1,
                    IngredientSet.Ingredients.Lettuce, 1,
                    IngredientSet.Ingredients.Tomato, 1,
                    IngredientSet.Ingredients.Meatball, 1);
                GameController.instance.starScore.Clear();
                GameController.instance.starScore.Add(500);
                GameController.instance.starScore.Add(750);
                GameController.instance.starScore.Add(950);
                break;
            case 24:
//        	timer.TimerInit(90);
//        	OrderController.instance.AddOrder(
//        		IngredientSet.Ingredients.Cheese, 1,
//        		IngredientSet.Ingredients.Beans, 1,
//        		IngredientSet.Ingredients.Meatball, 1,
//        		IngredientSet.Ingredients.Tomato, 1,
//        		IngredientSet.Ingredients.Lettuce, 1,
//        		IngredientSet.Ingredients.Rice, 1);
//        	GameController.instance.starScore.Clear();
//        	GameController.instance.starScore.Add(300);
//        	GameController.instance.starScore.Add(450);
//        	GameController.instance.starScore.Add(550);
//            break;

			timer.TimerInit(60);
			OrderController.instance.AddOrder(
				IngredientSet.Ingredients.Tomato, 1
			);
			GameController.instance.starScore.Clear();
			GameController.instance.starScore.Add(25);
			GameController.instance.starScore.Add(75);
			GameController.instance.starScore.Add(100);
			break;
        }
            

        Debug.Log (OrderController.instance.OrderListToString ());
	}

	void Update () {
        if (GameController.instance.first)
        {
            GameController.instance.first = false;
            LoggingManager.instance.Initialize(094, 4, true);
            LoggingManager.instance.RecordPageLoad();


            //resubmit total stars on game load
            Application.ExternalCall("kongregate.stats.submit", "Stars", SaveManager.instance.totalStars());
            //submit highest level completed
            Application.ExternalCall("kongregate.stats.submit", "HighestLevel", SaveManager.instance.GetLastLevelCompleted());
        }

		if (Input.anyKeyDown && GameController.instance.gamestate == GameController.GameState.GameStart) {
			GoToLevel (1);
		}
		else if ((Input.GetKeyDown(KeyCode.P)|| Input.GetKeyDown(KeyCode.Escape)) && GameController.instance.gamestate==GameController.GameState.Play) {
			GoToPause();
		}
		else if ((Input.GetKeyDown(KeyCode.P)||Input.GetKeyDown(KeyCode.Escape)|| Input.GetKeyDown(KeyCode.Return))  && GameController.instance.gamestate==GameController.GameState.Pause) {
			if (canvasInstructionsPause.activeSelf == true) {
				GoToPause ();
			}
			else {
				Resume ();
			}
		}
		else if (Input.GetKeyDown(KeyCode.P)||Input.GetKeyDown(KeyCode.Escape) && GameController.instance.gamestate==GameController.GameState.LevelStart) {
			GoToMenuLevelSelect ();
		}
		else if (Input.GetKeyDown(KeyCode.Escape) && 
			(GameController.instance.gamestate==GameController.GameState.Win || GameController.instance.gamestate==GameController.GameState.Lose)) {
			GoToMenuLevelSelect (); 
		}
		else if (Input.GetKeyDown(KeyCode.R) && 
			((GameController.instance.gamestate==GameController.GameState.Pause) || (GameController.instance.gamestate==GameController.GameState.Win) || (GameController.instance.gamestate==GameController.GameState.Lose))) {
			ReplayLevel ();
		}
		else if (Input.GetKeyDown(KeyCode.Return) && GameController.instance.gamestate==GameController.GameState.Win && ((maxLevelUnlocked >= ((loadingLevelNumber+1))) || (loadingLevelNumber==maxLevelNumber)) && OrderUI.instance.gameobjectfields.GameCompleteScreen.gameObject.activeInHierarchy == false)
		{
			GoToNextLevel ();
		}
		else if (Input.anyKeyDown && OrderUI.instance.gameobjectfields.GameCompleteScreen.gameObject.activeInHierarchy == true) {
			if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1) || Input.GetMouseButtonDown (2)) {
				return; //Do Nothing
			} else {
				GoToMenuLevelSelect ();
			}
		}

	}


}
