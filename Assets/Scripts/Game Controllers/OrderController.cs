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

	public void AddOrder(IngredientSet order, int count = 1) {
		for (int i = 0; i < count; i++) {
			orderList.Add (order);
		}
	}

	public void AddOrder(Dictionary<IngredientSet.Ingredients, int> ingredients) {
		IngredientSet newOrder = new IngredientSet ();
		foreach (KeyValuePair<IngredientSet.Ingredients, int> kvp in ingredients) {
			newOrder.SetCount (kvp.Key, kvp.Value);
		}
		orderList.Add (newOrder);
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
		orderList.Add (newOrder);
	}

	public string OrderListToString () {
		string orderString = "Orders: ";
		foreach (IngredientSet order in orderList) {
			orderString += "(" + order.ToString () + "), ";
		}
		orderString.Trim ();
		return orderString.Substring (0, orderString.Length - 2);
	}

	public bool BurritoContentsFulfillOrder(IngredientSet orderToCompareTo){
		if (GameController.instance.player == null) {
			return false;
		}
		IngredientSet burritoIngredients = GameController.instance.player.GetComponent<ObjectCatcher> ().getIngredients ().ingredientSet;
		return burritoIngredients.Equivalent(orderToCompareTo);
	}

	// Returns true if our burrito contents fulfill any of the current orders
	public bool CanSubmitAnOrder() {
		if (orderList.Count == 0) {
			return false;
		}
		foreach (IngredientSet order in orderList) {
			if (BurritoContentsFulfillOrder (order)) {
				return true;
			}
		}
		return false;
	}
}
