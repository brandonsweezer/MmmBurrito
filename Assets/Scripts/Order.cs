using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order {

	public Dictionary<string, int> ingredients;

	//prints out contents of the order
	public void print(){
		string result = "";
		foreach (KeyValuePair<string, int> entry in ingredients) {
			result += string.Format("{0} {1}(s), ", entry.Value, entry.Key);
		}
		result = result.Substring (0, result.Length - 2);
		Debug.Log (result);
	}

	//clears contents of the order
	public void clear(){
		ingredients.Clear();
	}

	//adds ingredients to the order
	public void add(string s, int i){
		ingredients.Add(s, i);
	}

	public Order(){
		ingredients = new Dictionary<string, int> ();
	}
}
