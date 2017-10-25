using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaughtIngredientSet {

	public IngredientSet ingredientSet;

	public List<int>[] ingredientQualities;

	public CaughtIngredientSet () {
		ingredientSet = new IngredientSet ();
		ingredientQualities = new List<int>[ingredientSet.ingredients.Length];
		for (int i = 0; i < ingredientQualities.Length; i++) {
			ingredientQualities [i] = new List<int> ();
		}
	}

	public void CatchIngredient(IngredientSet.Ingredients ingredient, int quality) {
        ingredientSet.SetCount(ingredient, ingredientSet.GetCount(ingredient) + 1);
        ingredientQualities[(int)ingredient].Add(quality);		
	}

	public void Empty() {
		ingredientSet.Clear ();
		for (int i = 0; i < ingredientQualities.Length; i++) {
			ingredientQualities [i].Clear();
		}
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
		return ingredientSet.ToString ();
	}
}
