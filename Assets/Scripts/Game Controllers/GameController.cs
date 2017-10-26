using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject player;

	public bool levelComplete;
	public int score;

    public int gameTime;

	public bool canSubmit; 

    public List<GameObject> objects;

	// Make this class a singleton
	public static GameController instance = null;
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (this);
		}
	}
		
}
