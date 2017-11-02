using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order {
	public IngredientSet ingredientSet;
	public GameObject uiTicket;
	public Order(IngredientSet ingredientSet) {
		this.ingredientSet = ingredientSet;
	}

	public string ToString() {
		return "Order with contents: "+ingredientSet.ToString()+"; does it have a UI ticket? "+(uiTicket!= null);
	}
}
