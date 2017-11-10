using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TicketManager : MonoBehaviour {

	private static Color grayedOutTint = new Color (.7f, .7f, .7f, .5f);

	public GameObject ingredientIconPrefab;
	public Sprite regularTicketImage;
	public Sprite fulfilledTicketImage;
	public Sprite invalidTicketImage;

	private Order order;
	private Image ticketImage;
	private bool ticketIsInvalid;

	public void TicketInit(Order order) {
		this.order = order;
		CreateIcons ();
		ticketImage = GetComponent<Image> ();
		ticketIsInvalid = false;
	}

	void CreateIcons() {
		int count;
		foreach (IngredientSet.Ingredients ingredient in Enum.GetValues(typeof(IngredientSet.Ingredients))) {
			// Create icons for each ingredient in the order
			count = order.ingredientSet.GetCount (ingredient);
			for (int i = 0; i < count; i++){
				GameObject icon = Instantiate (ingredientIconPrefab) as GameObject;
				icon.transform.SetParent (transform, false);
				icon.GetComponent<Image> ().sprite = IngredientSet.ingredientSprites_full[ingredient];
			}
		}
	}

	public void UpdateGraphics() {
		UpdateTicketStatus ();
		UpdateTicketIngredientIcons ();
	}

	// Gives a green outline/grays out depending on whether we can fulfill the order or have failed it
	void UpdateTicketStatus() {
		if (OrderController.instance.BurritoContentsFailOrder (order)) {
			ticketImage.sprite = invalidTicketImage;
			ticketImage.color = grayedOutTint;
			ticketIsInvalid = true;
		} else {
			ticketIsInvalid = false;
			if (OrderController.instance.BurritoContentsFulfillOrder (order)) {
				ticketImage.sprite = fulfilledTicketImage;
				ticketImage.color = Color.white;
			} else {
				ticketImage.sprite = regularTicketImage;
				ticketImage.color = Color.white;
			}
		}
	}

	void UpdateTicketIngredientIcons () {
		// If it's an invalid ticket, just gray out all the icons.
		if (ticketIsInvalid) {
			int numIngredients = order.ingredientSet.GetFullCount ();
			for (int i = 0; i < numIngredients; i++) {
				Image ingredientIcon = transform.GetChild (i).gameObject.GetComponent<Image> ();
				Sprite currentSprite = ingredientIcon.sprite;
				Sprite newSprite;
				if (IngredientSet.ingredientSprites_GlowtoFull.TryGetValue(currentSprite, out newSprite)) { 
					ingredientIcon.sprite = newSprite;
					ingredientIcon.color = grayedOutTint;
				}
			}
			return;
		}

		// Else ticket is valid, so we need to determine which icons to give a green outline to
		IngredientSet caughtIngredients = GameController.instance.player.GetComponent<ObjectCatcher> ().GetIngredients ().ingredientSet;
		int lastIndex = 0;
		foreach (IngredientSet.Ingredients ingredientType in Enum.GetValues(typeof(IngredientSet.Ingredients))) {
			// Determine how many icons should be outline in green for this ingredient type
			int ingredientsOfTypeCaught = caughtIngredients.GetCount (ingredientType);
			int ingredientsOfTypeInOrder = order.ingredientSet.GetCount (ingredientType);
			int numIconsToLightUp = Math.Min (ingredientsOfTypeCaught, ingredientsOfTypeInOrder);

			// use the green outline image for those icons
			for (int i = 0; i < numIconsToLightUp; i++) {
				Image ingredientIcon = transform.GetChild(lastIndex + i).gameObject.GetComponent<Image> ();
				ingredientIcon.sprite = IngredientSet.ingredientSprites_glowing [ingredientType];
				ingredientIcon.color = Color.white;
			}

			// reset the image for the rest of that ingredient's icons
			for (int i = numIconsToLightUp; i < ingredientsOfTypeInOrder; i++) {
				Image ingredientIcon = transform.GetChild(lastIndex + i).gameObject.GetComponent<Image> ();
				ingredientIcon.sprite = IngredientSet.ingredientSprites_full [ingredientType];
				ingredientIcon.color = grayedOutTint;
			}

			lastIndex += ingredientsOfTypeInOrder;
		}
	}
}
