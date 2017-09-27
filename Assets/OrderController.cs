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
		orderList.Add (globalOrders.getOrders(orderIndex), count);
	}
}
