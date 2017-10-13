using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderList {

	// This class is a singleton
	private static OrderList _instance;

	// List of all types of orders in our game.
	public List<IngredientSet> orders = new List<IngredientSet>();

	// Setup all the orders in the constructor.
	private OrderList(){
		orders = new List<IngredientSet> ();
		IngredientSet newOrder;

		// Order 0
		newOrder = new IngredientSet();
		newOrder.SetCount (IngredientSet.Ingredients.Tomato, 1);
		orders.Add (newOrder);

		// Order 1
		newOrder = new IngredientSet();
		newOrder.SetCount (IngredientSet.Ingredients.Cheese, 1);
		orders.Add (newOrder);

		// Order 2
		newOrder = new IngredientSet();
		newOrder.SetCount (IngredientSet.Ingredients.Tomato, 2);
		newOrder.SetCount (IngredientSet.Ingredients.Beans, 2);
		newOrder.SetCount (IngredientSet.Ingredients.Rice, 3);
		orders.Add (newOrder);

		// Order 3
		newOrder = new IngredientSet();
		newOrder.SetCount (IngredientSet.Ingredients.Tomato, 1);
		newOrder.SetCount (IngredientSet.Ingredients.Cheese, 1);
		newOrder.SetCount (IngredientSet.Ingredients.Rice, 1);
		newOrder.SetCount (IngredientSet.Ingredients.Meatball, 1);
		newOrder.SetCount (IngredientSet.Ingredients.Lettuce, 1);
		orders.Add (newOrder);

		// Order 4
		newOrder = new IngredientSet();
		newOrder.SetCount (IngredientSet.Ingredients.Beans, 1);
		newOrder.SetCount (IngredientSet.Ingredients.Lettuce, 1);
		newOrder.SetCount (IngredientSet.Ingredients.Cheese, 1);
		orders.Add (newOrder);
	}

	public static OrderList instance {
		get {
			if (_instance == null) {
				_instance = new OrderList ();
			}
			return _instance;
		}
	}

	// Retrieves a specific order.
	public IngredientSet getOrder(int i){
		return orders [i];
	}
}
