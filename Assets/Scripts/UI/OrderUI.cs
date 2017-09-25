using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour {

	private string orderstring;
	public Text textInstance;

	private Dictionary<Order, int> orders;
	public GameObject gameControllerObject; 



	// Use this for initialization
	void Start () {
		orderstring = "Orders: ";
		orders = gameControllerObject.GetComponent<GameController>().orderList;

		foreach (KeyValuePair<Order, int> entry in orders) {
			orderstring += "{";
			for (int i = 0; i < entry.Value; i++) {
				orderstring += entry.Key.toString ();
			}
			orderstring += "}";

		}


		textInstance.text = orderstring;


	}

	// Update is called once per frame
	void Update() {
		Start ();
	}




}
