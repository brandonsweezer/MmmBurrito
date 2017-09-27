using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderList {

	// List of all types of orders in our game.
	public static List<Order> orders = new List<Order>();

	// Retrieves a specific order.
	public static Order getOrder(int i){
		return orders [i];
	}

	// Setup all the orders in the constructor.
	public OrderList(){
		orders = new List<Order> ();

		// Order 0
		orders.Add (new Order ());
		orders[orders.Count-1].add("Tomato", 1);

		// Order 1
		orders.Add(new Order());
		orders[orders.Count-1].add("Cheese", 1);

		// Order 2
		orders.Add (new Order());
		orders[orders.Count-1].add("Tomato", 2);
		orders[orders.Count-1].add("Beans", 2);

		// Order 3
		orders.Add (new Order());
		orders [orders.Count-1].add ("Tomato", 1);
		orders [orders.Count-1].add ("Beans", 1);
		orders [orders.Count-1].add ("Rice", 1);
	}
}
