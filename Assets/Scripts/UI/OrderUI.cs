using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour {

	private string orderString;
	public Text levelOrderList;


	private string burritoString;
	public Text currentBurrito;

	private string submissionString;
	public Text submissionMessage;

	private string winString;
	public Text winMessage;

	private Dictionary<Order, int> orders;
	public GameObject gameControllerObject; 


	public GameObject subPlate;
	private GameObject burrito;





	// Use this for initialization
	void Start () {
		orderString = "Orders: ";
		orders = OrderController.instance.orderList;
		burrito = GameController.instance.player;



		burritoString = burrito.GetComponent<ObjectCatcher>().getTextString();
		submissionString = subPlate.GetComponent<SubmissionController>().getTextString();
		winString = subPlate.GetComponent<SubmissionController>().getWinString();

		foreach (KeyValuePair<Order, int> entry in orders) {
			orderString += "{";
			for (int i = 0; i < entry.Value; i++) {
				orderString += entry.Key.toString ();
			}
			orderString += "}";

		}

		levelOrderList.text = orderString;
		currentBurrito.text = burritoString;
		submissionMessage.text = submissionString;
		winMessage.text = winString;

	}

	// Update is called once per frame
	void FixedUpdate() {
		Start ();

	}




}
