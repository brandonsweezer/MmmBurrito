using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IngredientSet : MonoBehaviour {




	public Sprite e_bean, e_cheese, e_lettuce, e_meatball, e_rice, e_tomato; 
	public Sprite f_bean, f_cheese, f_lettuce, f_meatball, f_rice, f_tomato; 

	public static Dictionary<IngredientSet.Ingredients,Sprite> ingredientSprites;  
	public static Dictionary<IngredientSet.Ingredients,Sprite> ingredientSprites_full; 




	// Note: the first ingredient (in this case "Beans") will have
	// value 0.
	public enum Ingredients {
		Beans,
		Cheese,
		Lettuce,
		Meatball,
		Rice,
		Tomato
	};
		

	// Converts an input like "Tomato" to Ingredients.Tomato
	// (Note that the string should match the ingredient name.)
	public static Ingredients StringToIngredient(string ingredientString) {
		return (Ingredients) Enum.Parse(typeof(Ingredients), ingredientString);
	}

	// The count of each of the different ingredient types (from the Ingredients enum defined above)
	private static int numIngredientTypes = Enum.GetNames(typeof(Ingredients)).Length;

	// The number of each ingredient type in this ingredient set
	public int[] ingredients;

	public IngredientSet(){
		ingredients = new int[numIngredientTypes];
	}

	void Start () {
		CreateDict ();
		CreateDictFull ();
	}


	// Sets the count of a specific ingredient in this ingredient set
	public void SetCount(Ingredients ingredient, int count) {
		ingredients[(int) ingredient] = count;
	}

	// Returns the count of a specific ingredient in this ingredient set


	public int GetCount(Ingredients ingredient) {
		return ingredients[(int) ingredient];
	}

	// Returns the ingreident count of the order
	public int GetFullCount() {
		int count = 0; 


		for (int i = 0; i < ingredients.Length; i++) {
			if (ingredients [i] != 0) {
				count++;
			}
		}
		return count;
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



	private void CreateDict () {
		if (IngredientSet.ingredientSprites != null) {
			return;
		}
		IngredientSet.ingredientSprites=new Dictionary<IngredientSet.Ingredients,Sprite> ();
		IngredientSet.ingredientSprites.Add  (Ingredients.Beans, e_bean);
		IngredientSet.ingredientSprites.Add  (Ingredients.Cheese, e_cheese);
		IngredientSet.ingredientSprites.Add  (Ingredients.Lettuce, e_lettuce);
		IngredientSet.ingredientSprites.Add  (Ingredients.Meatball, e_meatball);
		IngredientSet.ingredientSprites.Add  (Ingredients.Rice, e_rice);
		IngredientSet.ingredientSprites.Add  (Ingredients.Tomato, e_tomato);
	}


	private void CreateDictFull () {
		if (IngredientSet.ingredientSprites_full != null) {
			return;
		}
		IngredientSet.ingredientSprites_full=new Dictionary<IngredientSet.Ingredients,Sprite> ();
		IngredientSet.ingredientSprites_full.Add  (Ingredients.Beans, f_bean);
		IngredientSet.ingredientSprites_full.Add  (Ingredients.Cheese, f_cheese);
		IngredientSet.ingredientSprites_full.Add  (Ingredients.Lettuce, f_lettuce);
		IngredientSet.ingredientSprites_full.Add  (Ingredients.Meatball, f_meatball);
		IngredientSet.ingredientSprites_full.Add  (Ingredients.Rice, f_rice);
		IngredientSet.ingredientSprites_full.Add  (Ingredients.Tomato, f_tomato);
	}

	// Clears contents of the ingredient set
	public void Clear(){
		Array.Clear (ingredients, 0, ingredients.Length);
	}

	// Formats the ingredient set as a string
	override public string ToString(){
		if (ingredients.Length == 0 || this.ingredients == null) {
			return "Empty ingredient set";
		}

		string result = "";
		for (int i = 0; i < ingredients.Length; i++) {
			if (ingredients[i] != 0) {
				result += string.Format ("{0} {1}, ", ingredients[i], (Ingredients)i);
			}
		}
		if (result.Length < 2) {
			return "Empty ingredient set (error printing)";
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
        if (this.ingredients == null || other.ingredients == null)
        {
            return false;
        }
		bool ingredientsAllMatch = Enumerable.SequenceEqual(this.ingredients, other.ingredients);
		return ingredientsAllMatch;
	}
}
