using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject player;
	
	public Dictionary<Order, int> orderList;
	private OrderList globalOrders;

	void Start () {
		orderList = new Dictionary<Order, int> ();
		globalOrders = new OrderList ();
		//should be different for each level
		orderList.Add (globalOrders.getOrders(3), 1);
		foreach (KeyValuePair<Order, int> entry in orderList) {
			for (int i = 0; i < entry.Value; i++) {
				entry.Key.print ();
			}
		}
	}
}
