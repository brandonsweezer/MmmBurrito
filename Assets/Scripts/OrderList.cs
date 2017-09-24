using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderList {

	//List of all types of orders in our game
	public List<Order> orders = new List<Order>();

	//retrieves a specific order
	public Order getOrders(int i){
		return orders [i];
	}

	// Use this for initialization
	public OrderList(){
		orders = new List<Order> ();
		//Order 0
		orders.Add (new Order ());
		orders[0].add("Tomato", 1);

		//Order 1
		orders.Add (new Order());
		orders[1].add("Tomato", 2);
		orders[1].add("Beans", 2);

		//Order 2
		orders.Add (new Order());
		orders [2].add ("Tomato", 1);
		orders [2].add ("Beans", 1);
		orders [2].add ("Rice", 1);

        orders.Add(new Order());
        orders[3].add("Cheese", 1);
	}
}
