using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaughtIngredientSet {

	public IngredientSet ingredientSet;

	private List<IngredientSet.Ingredients> ingredientCatchOrder;

	public List<int>[] ingredientQualities;

	public CaughtIngredientSet () {
		ingredientSet = new IngredientSet ();
		ingredientQualities = new List<int>[ingredientSet.ingredients.Length];
		for (int i = 0; i < ingredientQualities.Length; i++) {
			ingredientQualities [i] = new List<int> ();
		}
		ingredientCatchOrder = new List<IngredientSet.Ingredients> ();
	}

	public void CatchIngredient(IngredientSet.Ingredients ingredientType, int quality) {
		ingredientSet.SetCount(ingredientType, ingredientSet.GetCount(ingredientType) + 1);
		ingredientQualities[(int)ingredientType].Add(quality);	
		ingredientCatchOrder.Add (ingredientType);
	}

	public void Empty() {
		ingredientSet.Clear ();
		for (int i = 0; i < ingredientQualities.Length; i++) {
			ingredientQualities [i].Clear();
		}
	}

	public void Undo() {
		if (ingredientCatchOrder.Count == 0) {
			Debug.Log ("Empty burrito, can't undo anything.");
			return;
		}
		Debug.Log ("Was: "+ToString());

		IngredientSet.Ingredients ingredientType = ingredientCatchOrder [ingredientCatchOrder.Count - 1];
		ingredientCatchOrder.RemoveAt (ingredientCatchOrder.Count - 1);
		ingredientSet.AddToCount (ingredientType, -1);

		Debug.Log ("subtracted from " + ingredientType);


		ingredientQualities [(int)ingredientType].RemoveAt (ingredientQualities [(int)ingredientType].Count - 1);
		Debug.Log ("After undo became: "+ToString());
	}

	public bool IsEmpty() {
		return ingredientSet.IsEmpty ();
	}

	public int getSumOfQualities() {
		int qualitySum = 0;
		for (int i = 0; i < ingredientQualities.Length; i++) {
			foreach (int quality in ingredientQualities[i]) {
				qualitySum += quality;
			}
		}
		return qualitySum;
	}

    override public string ToString() {
		string result = ingredientSet.ToString ();
		result += "; with qualities: ";
		for (int i = 0; i < ingredientQualities.Length; i++) {
			result += ((IngredientSet.Ingredients)i) + " [";
			foreach (int qual in ingredientQualities[i]) {
				result += qual + " ";
			}
			result += "] ";
		}
		return result;
	}
}
