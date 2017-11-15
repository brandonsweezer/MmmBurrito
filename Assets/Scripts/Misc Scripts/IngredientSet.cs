using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IngredientSet : MonoBehaviour {




 
	public Sprite f_bean, f_cheese, f_lettuce, f_meatball, f_rice, f_tomato;
	public Sprite g_bean, g_cheese, g_lettuce, g_meatball, g_rice, g_tomato;
	public Sprite rot_bean, rot_cheese, rot_lettuce, rot_meatball, rot_rice, rot_tomato;
	public Sprite indicator_shape_bean, indicator_shape_cheese, indicator_shape_lettuce, indicator_shape_meatball, indicator_shape_rice, indicator_shape_tomato;
	public Sprite indicator_geom_bean, indicator_geom_cheese, indicator_geom_lettuce, indicator_geom_meatball, indicator_geom_rice, indicator_geom_tomato;

 
	public static Dictionary<IngredientSet.Ingredients,Sprite> ingredientSprites_full; 
	public static Dictionary<IngredientSet.Ingredients,Sprite> ingredientSprites_glowing;  
	public static Dictionary<Sprite,Sprite> ingredientSprites_GlowtoFull;
	public static Dictionary<IngredientSet.Ingredients,Sprite> ingredientSprites_rot;
	public static Dictionary<IngredientSet.Ingredients,Sprite> ingredientSprites_indicator_shape;
	public static Dictionary<IngredientSet.Ingredients,Sprite> ingredientSprites_indicator_geom;

	public GameObject[] IngredientPrefabsAlphabetical;
	public static GameObject[] IngredientPrefabsAlphabeticalStatic;
	private static Color[] INGREDIENT_COLORS = {
		new Color(103, 42, 5, 255)/255f,
		new Color(254, 224, 59, 255)/255f,
		new Color(128, 205, 66, 255)/255f,
		new Color(130, 85, 41, 255)/255f,
		new Color(210, 247, 247, 255)/255f,
		new Color(239, 5, 6, 255)/255f
	};




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
		//return (Ingredients) Enum.Parse(typeof(Ingredients), ingredientString);
		switch (Char.ToUpper (ingredientString [0])) {
		case 'B':
			return Ingredients.Beans;
		case 'C':
			return Ingredients.Cheese;
		case 'L':
			return Ingredients.Lettuce;
		case 'M':
			return Ingredients.Meatball;
		case 'R':
			return Ingredients.Rice;
		case 'T':
			return Ingredients.Tomato;
		default:
			return Ingredients.Rice;
		}
	}

	public static Color GetColorForIngredient(string ingredientString) {
		return INGREDIENT_COLORS[(int)StringToIngredient (ingredientString)];
	}

	public static Sprite GetIndicatorSpriteForIngredient(string ingredientString) {
		if (GameController.instance.ABValue >= 2)
		{
			return ingredientSprites_indicator_shape[StringToIngredient (ingredientString)];
		}
		else
		{
			return ingredientSprites_indicator_geom[StringToIngredient (ingredientString)];
		}
	}


	// The count of each of the different ingredient types (from the Ingredients enum defined above)
	private static int numIngredientTypes = Enum.GetNames(typeof(Ingredients)).Length;

	// The number of each ingredient type in this ingredient set
	public int[] ingredients;

	public IngredientSet(){
		ingredients = new int[numIngredientTypes];
	}

	void Start () {
		CreateDictFull ();
		CreateDictGlow ();
		CreateDictGlowtoFull ();
		CreateDictRot ();
		CreateDictIndicatorsShape ();
		CreateDictIndicatorsGeom ();
		IngredientPrefabsAlphabeticalStatic = IngredientPrefabsAlphabetical;
	}


	// Sets the count of a specific ingredient in this ingredient set
	public void SetCount(Ingredients ingredient, int count) {
		ingredients[(int) ingredient] = count;
	}

	// Returns the count of a specific ingredient in this ingredient set
	public int GetCount(Ingredients ingredient) {
		return ingredients[(int) ingredient];
	}

	// Adds the given value to a specific ingredient's count in this ingredient set
	public void AddToCount(Ingredients ingredient, int value) {
		ingredients[(int) ingredient] += value;
	}

	// Returns the number of total ingredient in the order (counts duplicate ingredients)
	public int GetFullCount() {
		int count = 0; 


		for (int i = 0; i < ingredients.Length; i++) {
			if (ingredients [i] != 0) {
				count=count + ingredients [i];
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


	private void CreateDictGlow () {
		if (IngredientSet.ingredientSprites_glowing != null) {
			return;
		}
		IngredientSet.ingredientSprites_glowing=new Dictionary<IngredientSet.Ingredients,Sprite> ();
		IngredientSet.ingredientSprites_glowing.Add  (Ingredients.Beans, g_bean);
		IngredientSet.ingredientSprites_glowing.Add  (Ingredients.Cheese, g_cheese);
		IngredientSet.ingredientSprites_glowing.Add  (Ingredients.Lettuce, g_lettuce);
		IngredientSet.ingredientSprites_glowing.Add  (Ingredients.Meatball, g_meatball);
		IngredientSet.ingredientSprites_glowing.Add  (Ingredients.Rice, g_rice);
		IngredientSet.ingredientSprites_glowing.Add  (Ingredients.Tomato, g_tomato);
	}


	private void CreateDictGlowtoFull () {
		if (IngredientSet.ingredientSprites_GlowtoFull != null) {
			return;
		}
		IngredientSet.ingredientSprites_GlowtoFull=new Dictionary<Sprite,Sprite> ();
		IngredientSet.ingredientSprites_GlowtoFull.Add  (g_bean, f_bean);
		IngredientSet.ingredientSprites_GlowtoFull.Add  (g_cheese, f_cheese);
		IngredientSet.ingredientSprites_GlowtoFull.Add  (g_lettuce, f_lettuce);
		IngredientSet.ingredientSprites_GlowtoFull.Add  (g_meatball, f_meatball);
		IngredientSet.ingredientSprites_GlowtoFull.Add  (g_rice, f_rice);
		IngredientSet.ingredientSprites_GlowtoFull.Add  (g_tomato, f_tomato);
	}


	private void CreateDictRot () {
		if (IngredientSet.ingredientSprites_rot != null) {
			return;
		}
		IngredientSet.ingredientSprites_rot=new Dictionary<IngredientSet.Ingredients,Sprite> ();
		IngredientSet.ingredientSprites_rot.Add  (Ingredients.Beans, rot_bean);
		IngredientSet.ingredientSprites_rot.Add  (Ingredients.Cheese, rot_cheese);
		IngredientSet.ingredientSprites_rot.Add  (Ingredients.Lettuce, rot_lettuce);
		IngredientSet.ingredientSprites_rot.Add  (Ingredients.Meatball, rot_meatball);
		IngredientSet.ingredientSprites_rot.Add  (Ingredients.Rice, rot_rice);
		IngredientSet.ingredientSprites_rot.Add  (Ingredients.Tomato, rot_tomato);
	}

	private void CreateDictIndicatorsShape () {
		if (IngredientSet.ingredientSprites_indicator_shape != null) {
			return;
		}
		IngredientSet.ingredientSprites_indicator_shape=new Dictionary<IngredientSet.Ingredients,Sprite> ();
		IngredientSet.ingredientSprites_indicator_shape.Add  (Ingredients.Beans, indicator_shape_bean);
		IngredientSet.ingredientSprites_indicator_shape.Add  (Ingredients.Cheese, indicator_shape_cheese);
		IngredientSet.ingredientSprites_indicator_shape.Add  (Ingredients.Lettuce, indicator_shape_lettuce);
		IngredientSet.ingredientSprites_indicator_shape.Add  (Ingredients.Meatball, indicator_shape_meatball);
		IngredientSet.ingredientSprites_indicator_shape.Add  (Ingredients.Rice, indicator_shape_rice);
		IngredientSet.ingredientSprites_indicator_shape.Add  (Ingredients.Tomato, indicator_shape_tomato);
	}
	private void CreateDictIndicatorsGeom () {
		if (IngredientSet.ingredientSprites_indicator_geom != null) {
			return;
		}
		IngredientSet.ingredientSprites_indicator_geom=new Dictionary<IngredientSet.Ingredients,Sprite> ();
		IngredientSet.ingredientSprites_indicator_geom.Add  (Ingredients.Beans, indicator_geom_bean);
		IngredientSet.ingredientSprites_indicator_geom.Add  (Ingredients.Cheese, indicator_geom_cheese);
		IngredientSet.ingredientSprites_indicator_geom.Add  (Ingredients.Lettuce, indicator_geom_lettuce);
		IngredientSet.ingredientSprites_indicator_geom.Add  (Ingredients.Meatball, indicator_geom_meatball);
		IngredientSet.ingredientSprites_indicator_geom.Add  (Ingredients.Rice, indicator_geom_rice);
		IngredientSet.ingredientSprites_indicator_geom.Add  (Ingredients.Tomato, indicator_geom_tomato);
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
		if (result.Length < 1) {
			return "Probably an empty ingredient set? (error printing)";
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

	public static GameObject GetPrefab(Ingredients ingredientType) {
		return IngredientPrefabsAlphabeticalStatic[(int)ingredientType];
	}

	// Returns true if the other Ingredient set contains ingredients not part
	public bool FailsOrder(IngredientSet order) {
		foreach (Ingredients ingredientType in Enum.GetValues(typeof(Ingredients))) {
			if (GetCount (ingredientType) > order.GetCount (ingredientType)) {
				return true;
			}
		}
		return false;
	}
}
