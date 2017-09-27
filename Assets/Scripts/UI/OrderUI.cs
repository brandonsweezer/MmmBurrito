using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour {

	public GameObject canvasHUD;

	public Text levelOrderList;
	public Text currentBurrito;
	public Text submissionMessage;
	public Text winMessage;
	public Text loseMessage;

	private Dictionary<Order, int> orders;

	// Make this class a singleton
	public static OrderUI instance = null;
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (this);
		}
	}


	void Start () {
		// empty out all fields
		levelOrderList.text = "";
		currentBurrito.text = "";
		submissionMessage.text = "";
		winMessage.text = "";
		loseMessage.text = "";
		// hide the HUD in the level selection screen
		canvasHUD.SetActive (false);
	}


	// Use this for initialization
	public void UpdateUI () {
		levelOrderList.text = OrderController.instance.OrderListToString();
		if (GameController.instance.player != null) {
			currentBurrito.text = GameController.instance.player.GetComponent<ObjectCatcher> ().getTextString ();
		}
	}

	public void setWinMessage(string msg) {
		winMessage.text = msg;
	}
	public void setSubmissionMessage(string msg) {
		submissionMessage.text = msg;
	}

	// Update is called once per frame
	void FixedUpdate() {
		if (canvasHUD.activeSelf) {
			UpdateUI ();
		}
	}




}
