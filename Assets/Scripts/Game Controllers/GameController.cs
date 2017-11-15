using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject player;

	public bool levelComplete;

	public enum GameState {
		Menu,
		Play,
		Win,
		Lose,
		Pause
	};

	public int currentLevel;
	public int score;

    public int gameTime;

	public bool canSubmit;

    public bool levelEnd;

    public List<GameObject> objects;

    public bool dead;

    public int ABValue;

    //score needed for certain amount of stars
    public List<int> starScore;

    //number of total stars needed to unlock a level
    public List<int> starUnlock;

	public GameState gamestate = GameState.Menu;

    public bool first = true;

    // Make this class a singleton
    public static GameController instance = null;
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (this);
		}
	}

	public void UpdateSubmissionValidity() {
		canSubmit = OrderController.instance.CanSubmitAnOrder ();
	}

	public void SetScore(int score) {
		// Update score display.
		if (score == 0) {
			ScoreUI.instance.SetScoreDisplay (0);
		}
		// Update actual score.
		this.score = score;
	}

	public int GetScore() {
		return score;
	}
		
}
