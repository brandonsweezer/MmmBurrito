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
	public Text orderTotalDisplay;


	public Image ticket1;
	public Image ticket2;
	public Image ticket3;

	private int orderTotal;

	private Dictionary<Order, int> orders;
	private int orderCount;

	// Make this class a singleton
	public static OrderUI instance = null;
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (this);
		}

	}

	public void TicketInit (Image orderTicket){
		GridLayoutGroup tick = orderTicket.GetComponent<GridLayoutGroup> ();
		if (orderTotal == 1) {
			tick.cellSize = new Vector2 (150, 60);
			Debug.Log ("one order");
		}
		else if (orderTotal == 2) {
			tick.cellSize = new Vector2 (75, 60);
		}

		else if (orderTotal <= 4) {
			tick.cellSize = new Vector2 (75, 30);
		}

		else {
			tick.cellSize = new Vector2 (75, 20);
		}

//		Dictionary<Order, int>.KeyCollection key = orders.Keys;
//		foreach (Order keyvalue in key)
			}


	public void ResetUIFields () {
		levelOrderList.text = "";
		currentBurrito.text = "";
		submissionMessage.text = "";
		winMessage.text = "";
		loseMessage.text = "";
		orderTotalDisplay.text = "";
	}


	// Use this for initialization
	public void UpdateUI () {
		orders = OrderController.instance.orderList;
		levelOrderList.text = OrderController.instance.OrderListToString();
		orderTotal = orders.Count;
		orderTotalDisplay.text = orders.Count.ToString();
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
			TicketInit(ticket1);
		}
	}

}
