using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSet {

	// Note: the first ingredient (in this case "Beans") will have
	// value 0.
	public enum Ingredients {
		Beans,
		Cheese,
		Lettuce,
		Meatball,
		Onion,
		Rice,
		Tomato
	};

	public static Ingredients StringToIngredient(string ingredientString) {
		return (Ingredients) Enum.Parse(typeof(Ingredients), ingredientString);
	}

	private static int numIngredientTypes = Enum.GetNames(typeof(Ingredients)).Length;

	public int[] ingredients;

	public IngredientSet(){
		ingredients = new int[numIngredientTypes];
	}

	// Sets an ingredient requirement to the order
	public void SetCount(Ingredients ingredient, int count) {
		ingredients[(int) ingredient] = count;
	}

	// Returns the number of a certain ingredient
	public int GetCount(Ingredients ingredient) {
		return ingredients[(int) ingredient];
	}

	public bool IsEmpty() {
		for (int i = 0; i < ingredients.Length; i++) {
			if (ingredients [i] != 0) {
				return false;
			}
		}
		return true;
	}

	// Clears contents of the order
	public void Clear(){
		Array.Clear (ingredients, 0, ingredients.Length);
	}

	// Formats the order as a string
	override public string ToString(){
		string result = "";
		for (int i = 0; i < ingredients.Length; i++) {
			if (ingredients[i] != 0) {
				result += string.Format ("{0} {1}, ", ingredients[i], (Ingredients)i);
			}
		}
		return result.Substring (0, result.Length - 2);
	}

	// Prints out contents of the order
	public void Print(){
		Debug.Log (ToString());
	}

	public bool Equivalent(IngredientSet other) {
		bool ingredientsAllMatch = Enumerable.SequenceEqual(this.ingredients, other.ingredients);
		return (other != null && ingredientsAllMatch);
	}
}
