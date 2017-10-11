using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderController : MonoBehaviour {
	
	public List<IngredientSet> orderList;

	// Make this class a singleton
	public static OrderController instance = null;
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (this);
		}
	}

	void Start () {
		orderList = new List<IngredientSet> ();
	}

	public void AddOrder(int orderIndex, int count = 1) {
		for (int i = 0; i < count; i++) {
			orderList.Add (OrderList.instance.getOrder (orderIndex));
		}
	}

	public string OrderListToString () {
		string orderString = "Orders: ";
		foreach (IngredientSet order in orderList) {
			orderString += "(" + order.ToString () + "), ";
		}
		orderString.Trim ();
		return orderString.Substring (0, orderString.Length - 2);
	}
}
