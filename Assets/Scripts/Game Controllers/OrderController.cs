using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderController : MonoBehaviour {
	
	public OrderList globalOrders;
	public Dictionary<Order, int> orderList;

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
		orderList = new Dictionary<Order, int> ();
		globalOrders = new OrderList ();
	}

	public void AddOrder(int orderIndex, int count) {
		orderList.Add (OrderList.getOrder(orderIndex), count);
	}

	public string OrderListToString () {
		string orderString = "Orders: ";
		foreach (KeyValuePair<Order, int> entry in OrderController.instance.orderList) {
			if (entry.Value > 1) {
				orderString += entry.Value + " X ";
			} else {
				orderString += "(" + entry.Key.ToString () + "), ";
			}
		}
		orderString.Trim ();
		return orderString.Substring (0, orderString.Length - 2);
	}
}
