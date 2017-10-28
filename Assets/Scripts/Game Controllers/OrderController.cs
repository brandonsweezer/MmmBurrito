using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderController : MonoBehaviour {
	
	public List<Order> orderList;

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
		orderList = new List<Order> ();
	}

	public void AddOrder(int orderIndex, int count = 1) {
		for (int i = 0; i < count; i++) {
			orderList.Add (new Order(OrderList.instance.getOrder (orderIndex)));
		}
	}

	public void AddOrder(IngredientSet order, int count = 1) {
		for (int i = 0; i < count; i++) {
			orderList.Add (new Order(order));
		}
	}

	public void AddOrder(Dictionary<IngredientSet.Ingredients, int> ingredients) {
		IngredientSet newOrder = new IngredientSet ();
		foreach (KeyValuePair<IngredientSet.Ingredients, int> kvp in ingredients) {
			newOrder.SetCount (kvp.Key, kvp.Value);
		}
		orderList.Add (new Order(newOrder));
	}

	// Must be alternating parameter types between IngredientSet.Ingredients and ints
	public void AddOrder(params object[] parameters) {
		IngredientSet newOrder = new IngredientSet ();
		if (parameters.Length < 2) {
			return;
		}
		for (int i = 0; i < parameters.Length; i+=2) {
			newOrder.SetCount ((IngredientSet.Ingredients) parameters[i], (int) parameters[i+1]);
		}
		orderList.Add (new Order(newOrder));
	}

	public string OrderListToString () {
		string orderString = "Orders: ";
		foreach (Order order in orderList) {
			orderString += "(" + order.ingredientSet.ToString () + "), ";
		}
		orderString.Trim ();
		return orderString.Substring (0, orderString.Length - 2);
	}

	public bool BurritoContentsFulfillOrder(IngredientSet orderToCompareTo){
		if (GameController.instance.player == null || GameController.instance.player.GetComponent<ObjectCatcher> ().getIngredients () == null) {
			return false;
		}
		IngredientSet burritoIngredients = GameController.instance.player.GetComponent<ObjectCatcher> ().getIngredients ().ingredientSet;
		return burritoIngredients.Equivalent(orderToCompareTo);
	}

	public bool BurritoContentsFulfillOrder(Order orderToCompareTo){
		return BurritoContentsFulfillOrder (orderToCompareTo.ingredientSet);
	}

	// Returns true if our burrito contents fulfill any of the current orders
	public bool CanSubmitAnOrder() {
		if (orderList.Count == 0) {
			return false;
		}
		foreach (Order order in orderList) {
			if (BurritoContentsFulfillOrder (order)) {
				return true;
			}
		}
		return false;
	}

	public void FulfillOrder(Order order) {
		Debug.Log ("order is: " + order.ToString());
		Debug.Log ("fulfill the order");
		if (order.uiTicket != null) {
			order.uiTicket.GetComponent<TicketAnimations> ().StartRemoveAnimation ();
		} else {
			Debug.LogError ("An order did not have any uiTicket attached to it");
		}
		orderList.Remove (order);
	}
}
