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

	// Converts an input like "tomato" to Ingredients.Tomato
	public static Ingredients StringToIngredient(string ingredientString) {
		return (Ingredients) Enum.Parse(typeof(Ingredients), ingredientString);
	}

	// the count of each of the different ingredient types (from the Ingredients enum defined above)
	private static int numIngredientTypes = Enum.GetNames(typeof(Ingredients)).Length;

	// The number of each ingredient type in this ingredient set
	public int[] ingredients;

	public IngredientSet(){
		ingredients = new int[numIngredientTypes];
	}

	// Sets the count of a specific ingredient in this ingredient set
	public void SetCount(Ingredients ingredient, int count) {
		ingredients[(int) ingredient] = count;
	}

	// Returns the count of a specific ingredient in this ingredient set
	public int GetCount(Ingredients ingredient) {
		return ingredients[(int) ingredient];
	}

	// Returns true if there are no ingredients in this ingredient set
	public bool IsEmpty() {
		for (int i = 0; i < ingredients.Length; i++) {
			if (ingredients [i] != 0) {
				return false;
			}
		}
		return true;
	}

	// Clears contents of the ingredient set
	public void Clear(){
		Array.Clear (ingredients, 0, ingredients.Length);
	}

	// Formats the ingredient set as a string
	override public string ToString(){
		string result = "";
		for (int i = 0; i < ingredients.Length; i++) {
			if (ingredients[i] != 0) {
				result += string.Format ("{0} {1}, ", ingredients[i], (Ingredients)i);
			}
		}
		return result.Substring (0, result.Length - 2);
	}

	// Prints out the contents of the ingredient set
	public void Print(){
		Debug.Log (ToString());
	}

	// Returns true if the other IngredientSet has the same ingredient counts
	// (i.e. same number of beans, cheese, tomatoes, etc.)
	public bool Equivalent(IngredientSet other) {
		bool ingredientsAllMatch = Enumerable.SequenceEqual(this.ingredients, other.ingredients);
		return (other != null && ingredientsAllMatch);
	}
}
