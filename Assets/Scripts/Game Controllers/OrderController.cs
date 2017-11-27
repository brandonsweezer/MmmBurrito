using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderController : MonoBehaviour {

	public static int maxNumActiveOrders = 3;
	public List<Order> activeOrders;
	public List<Order> inactiveOrders;

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
		activeOrders = new List<Order> ();
		inactiveOrders = new List<Order> ();
	}

	void AddOrder(Order order) {
		if (activeOrders.Count < maxNumActiveOrders) {
			activeOrders.Add (order);
		} else {
			inactiveOrders.Add (order);
		}
	}

	public void AddOrder(int orderIndex, int count = 1) {
		for (int i = 0; i < count; i++) {
			AddOrder (new Order(OrderList.instance.getOrder (orderIndex)));
		}
	}

	public void AddOrder(IngredientSet order, int count = 1) {
		for (int i = 0; i < count; i++) {
			AddOrder (new Order(order));
		}
	}

	public void AddOrder(Dictionary<IngredientSet.Ingredients, int> ingredients) {
		IngredientSet newOrder = new IngredientSet ();
		foreach (KeyValuePair<IngredientSet.Ingredients, int> kvp in ingredients) {
			newOrder.SetCount (kvp.Key, kvp.Value);
		}
		AddOrder (new Order(newOrder));
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
		AddOrder (new Order(newOrder));
	}

	public void ClearOrders() {
		inactiveOrders.Clear ();
		activeOrders.Clear ();
	}

	public string OrderListToString () {
		string orderString = "Active orders: ";
		foreach (Order order in activeOrders) {
			orderString += "(" + order.ingredientSet.ToString () + "), ";
		}
		orderString.Trim ();
		orderString += "Inactive orders: ";
		foreach (Order order in inactiveOrders) {
			orderString += "(" + order.ingredientSet.ToString () + "), ";
		}
		orderString.Trim ();
		return orderString.Substring (0, orderString.Length - 2);
	}

	public bool BurritoContentsFulfillOrder(IngredientSet orderToCompareTo){
		if (GameController.instance.player == null || GameController.instance.player.GetComponent<ObjectCatcher> ().GetIngredients () == null) {
			return false;
		}
		IngredientSet burritoIngredients = GameController.instance.player.GetComponent<ObjectCatcher> ().GetIngredients ().ingredientSet;
		return burritoIngredients.Equivalent(orderToCompareTo);
	}

	public bool BurritoContentsFulfillOrder(Order orderToCompareTo){
		return BurritoContentsFulfillOrder (orderToCompareTo.ingredientSet);
	}

	public bool BurritoContentsFailOrder(IngredientSet orderToCompareTo){
		if (GameController.instance.player == null || GameController.instance.player.GetComponent<ObjectCatcher> ().GetIngredients () == null) {
			return true;
		}
		IngredientSet burritoIngredients = GameController.instance.player.GetComponent<ObjectCatcher> ().GetIngredients ().ingredientSet;
		return burritoIngredients.FailsOrder(orderToCompareTo);
	}
	public bool BurritoContentsFailOrder(Order orderToCompareTo){
		return BurritoContentsFailOrder (orderToCompareTo.ingredientSet);
	}

	// Returns true if our burrito contents fulfill any of the current orders
	public bool CanSubmitAnOrder() {
		if (activeOrders.Count == 0) {
			return false;
		}
		foreach (Order order in activeOrders) {
			if (BurritoContentsFulfillOrder (order)) {
				return true;
			}
		}
		return false;
	}

	public void FulfillOrder(Order order) {
		if (order.uiTicket != null) {
			OrderUI.instance.SubmitOrder (order);
		} else {
			Debug.LogError ("An order did not have any uiTicket attached to it");
		}
		activeOrders.Remove (order);
		foreach (Order activeOrder in activeOrders) {
			activeOrder.uiTicket.GetComponent<TicketManager> ().MoveToCorrectXPos();
		}

		// create new ticket after a short delay
		ActivateNewOrder();
	}

	private void ActivateNewOrder() {
		if (inactiveOrders.Count > 0) {
			activeOrders.Add (inactiveOrders [0]);
			inactiveOrders.RemoveAt (0);
			OrderUI.instance.TicketInit (2);
		}
	}

	public IngredientSet GetCumulativeActiveIngredientSet() {
		if (activeOrders.Count == 0) {
			return new IngredientSet ();
		}

		IngredientSet set = activeOrders [0].ingredientSet.Clone ();
		for (int i = 1; i < activeOrders.Count; i++) {
			set.Combine (activeOrders [i].ingredientSet);
		}

		return set;
	}

	public int GetNumTotalOrders() {
		return activeOrders.Count + inactiveOrders.Count;
	}
}
