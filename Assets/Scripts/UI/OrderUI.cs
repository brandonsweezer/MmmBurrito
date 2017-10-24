using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



[System.Serializable]
public class TextFields
{
	public Text levelOrderList;
	public Text currentBurrito;
	public Text submissionMessage;
	public Text winMessage;
	public Text loseMessage;
	public Text orderTotalDisplay;
}

[System.Serializable]
public class GameObjectFields
{
	public GameObject canvasHUD;

	public GameObject TicketPrefab;
	public GameObject IngredientPrefab;
	public GameObject Divider;
	public GameObject TicketHUD;

	public GameObject CollectionHUD;
	public GameObject CollectionContainerPrefab;
	public GameObject CollectedIngredientPrefab;

	public Sprite CompletedTicket;
	public Sprite InvalidTicket;
}




public class OrderUI : MonoBehaviour {

	public TextFields textfields;
	public GameObjectFields gameobjectfields;

	private Text countText;

	private bool initializeTickets; 

	private ObjectCatcher objectcatcher; 


	private int orderTotal;
	private int ingredientTotal;

	private List<IngredientSet> orders;
	private int orderCount;

	private Dictionary<IngredientSet.Ingredients,Sprite> spriteDict_glowing; 
	private Dictionary<IngredientSet.Ingredients,Sprite> spriteDict_full;
	private Dictionary<Sprite,Sprite> spriteDict_GlowtoFull;

    private static int SUBMISSION_TIMER = 300;
    private int submissionTextTimer = SUBMISSION_TIMER;


	// Make this class a singleton
	public static OrderUI instance = null;
	void Awake () {
		initializeTickets = false;
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (this);
		}

	}

								//sets the ingredient box sizes based on the number of ingredients in the order
							//	public void SetUIsize(int ingredientTotal, GameObject tick){
							////		if (ingredientTotal == 1) {
							////			tick.cellSize = new Vector2 (150, 60);
							////			tick.constraintCount = 1;
							////			//Debug.Log ("one order");
							////		}
							////		else if (ingredientTotal == 2) {
							////			tick.cellSize = new Vector2 (75, 60);
							////			tick.constraintCount = 2;
							////		}
							////
							////		else if (ingredientTotal <= 4) {
							////			tick.cellSize = new Vector2 (75, 30);
							////			tick.constraintCount = 2;
							////		}
							////
							////		else {
							////			tick.cellSize = new Vector2 (50, 30);
							////			tick.constraintCount = 3;
							////		}
							//		tick.GetComponent<RectTransform>().rect.width=60*ingredientTotal;
							//	}


	public void TicketInit (int index){
		if (orders.Count >= index+1) {
			
							//gets grid to give to SetUISize to set boxes
							//GridLayoutGroup tick = gameobjectfields.TicketPrefab.GetComponent<GridLayoutGroup> ();
							//HorizontalLayoutGroup tick = gameobjectfields.TicketPrefab.GetComponent<HorizontalLayoutGroup>();

			//gets the current order
			IngredientSet currOrder = orders [index];
			//number of ingredients in the order 
			ingredientTotal = currOrder.GetFullCount ();


			//creates the ticket prefab container and divider
			GameObject container = Instantiate (gameobjectfields.TicketPrefab) as GameObject;
			container.transform.SetParent (gameobjectfields.TicketHUD.transform, false);
			container.tag=("Ticket");
			GameObject divider = Instantiate (gameobjectfields.Divider) as GameObject;
			divider.transform.SetParent (gameobjectfields.TicketHUD.transform, false);

			int containerWidth = 70 * ingredientTotal;
			Debug.Log (ingredientTotal);

			//container.GetComponent<RectTransform> ().rect.width = containerWidth;
			container.GetComponent<RectTransform> ().sizeDelta = new Vector2 (containerWidth,60f);


									//SetUIsize (ingredientTotal, container);
				

			//creates an ingredient prefab for each ingredient in the current order
			int count;
			foreach (IngredientSet.Ingredients ingredient in Enum.GetValues(typeof(IngredientSet.Ingredients))) {
				//Debug.Log ("In foreach");
				count = currOrder.GetCount (ingredient);
				for (int i = 0; i<count; i++){
					//Debug.Log ("In for");
					GameObject icon = Instantiate (gameobjectfields.IngredientPrefab) as GameObject;
					icon.transform.SetParent (container.transform, false);

					//sets the correct ingredient sprite
					icon.GetComponent<Image> ().sprite = IngredientSet.ingredientSprites_full[ingredient];
					//sets the required number of this particular ingredient 
					//icon.transform.GetChild(0).GetChild(0).GetComponent<Text>().text= count.ToString();
				}


									//We have a UI ticket (tick). We have an order (currOrd). Need to figure out how many inredients in that order (ingredientsTotal). Iterate through the enum 
									//creating a box for each ingredient. Affix the ingredient number to that box. 

			}

		}
	}


	public void markInvalid(GameObject ticket) {
		ticket.GetComponent<Image> ().sprite = gameobjectfields.InvalidTicket;
		ticket.GetComponent<Image> ().color = new Color (.7f, .7f, .7f, .5f);
		ticket.tag = ("InvalidTicket");
		for (int i = 0; i < ticket.transform.childCount; i++) {
			//individual ingredient 
			GameObject ingredient = ticket.transform.GetChild (i).gameObject;
			ingredient.GetComponent<Image> ().color = new Color (.7f, .7f, .7f, .5f);
				//ingredient.GetComponent<Image> ().sprite = IngredientSet.ingredientSprites_full [GameController.instance.player.GetComponent<ObjectCatcher> ().ingredientType];
			if (ingredient.tag == "MarkedIngredient"){
					//Debug.Log ("sprite: " + ingredient.GetComponent<Image> ().sprite);
					//Debug.Log ("dict entry: " + IngredientSet.ingredientSprites_GlowtoFull [ingredient.GetComponent<Image> ().sprite]);
				ingredient.GetComponent<Image> ().sprite = IngredientSet.ingredientSprites_GlowtoFull [ingredient.GetComponent<Image> ().sprite];
				ingredient.tag = ("Untagged");
			}

		}
	}

	public void TicketUpdate() {
		if (GameController.instance.player == null) {
			return;
		}
			
		if (GameController.instance.player.GetComponent<ObjectCatcher> ().GetnewIngredient ()==true){
			//Checks each ticket agaisnt the newly picked up ingredient to determine if new or duplicate
			//if TicketHUD has Ticket children

			if (gameobjectfields.TicketHUD.transform.childCount > 0) {
				//Get each ticket
				for (int i = 0; i < gameobjectfields.TicketHUD.transform.childCount; i++) {

					//individual ticket 
					GameObject ticket= gameobjectfields.TicketHUD.transform.GetChild(i).gameObject;
					bool validTicket=false;
					bool validIngredient = false;
					bool markedInvalidTicket=false;
					bool continueLoop = true;
					//checks if ticket is already invalid
					if (ticket.tag == "InvalidTicket") {
						markedInvalidTicket = true;
					}
					int completedIngredients = 0; 
					for (int ii = 0; ii < ticket.transform.childCount; ii++) {

						//individual ingredient 
						GameObject ingredient= ticket.transform.GetChild(ii).gameObject;
						IngredientSet.Ingredients caughtIngredient = GameController.instance.player.GetComponent<ObjectCatcher> ().ingredientType;

						bool match = IngredientSet.ingredientSprites_full [caughtIngredient] == ingredient.GetComponent<Image> ().sprite;

						validTicket = ((validTicket || match) && !markedInvalidTicket);

						if (match && validTicket && ingredient.tag!="MarkedIngredient") {

							Debug.Log ("mark ingredient");


							//Set filled image in top HUD
							//Marks ingredient active
							if (continueLoop) {
								ingredient.GetComponent<Image> ().sprite = IngredientSet.ingredientSprites_glowing [GameController.instance.player.GetComponent<ObjectCatcher> ().ingredientType];
								ingredient.GetComponent<Image> ().color = Color.white;
								ingredient.tag = ("MarkedIngredient");
								validIngredient = true;
								continueLoop = false;
							}

							//Decrement the count by one (might be taken out)
							//							Text countText = ingredient.transform.GetChild (0).GetChild (0).GetComponent<Text> ();
							//							string stringCount = countText.text.ToString ();
							//							int intCount = Int32.Parse (stringCount);
							//							intCount -= 1;
							//Ingredient Remaning Logic
							//							if (intCount == 0) {
							//								ingredient.GetComponent<Image> ().color =new Color (0f, 1f, 0f, 1f);
							//								countText.color = Color.green;
							//								Debug.Log ("IS ZERO");
							//							}
							//							if (intCount < 0) {
							//								Debug.Log ("LESS than zero");
							//								intCount = 0; 
							//								ticket.GetComponent<Image> ().color = new Color (1f, 0f, 0f, .5f);
							//								countText.color = Color.black;
							//								ingredient.GetComponent<Image> ().color =new Color (.2f, .2f, .2f, .5f);
							//							}
							//							if (intCount > 0) {
							//								Debug.Log ("MORE to go");
							//								OrderComplete = false;
							//							}
							//							string newStringCount = intCount.ToString ();
							//countText.text = newStringCount;
						}

						//Check if ingredient collected matches an order ingredient
						if (ingredient.tag == "MarkedIngredient") {
							completedIngredients++; 
						}

					}
					if (completedIngredients == ticket.transform.childCount && ticket.tag=="Ticket") {
						//Mark Complete
						ticket.GetComponent<Image> ().sprite = gameobjectfields.CompletedTicket;
						Debug.Log ("complete");
					}
					if ((!validTicket || !validIngredient) && ticket.tag=="Ticket") {
						//Debug.Log ("BAD TICKET");
						//Mark Invalid
						//ticket.GetComponent<Image> ().color = new Color (1f, 0f, 0f, .5f);
						//ticket.GetComponent<Image> ().sprite = gameobjectfields.InvalidTicket;
						markInvalid(ticket);
					}
				}
			}
		}
	}


	public void CreateCollectedItem () {
		//Debug.Log ("Create a new item");
		//creates the collection container prefab 
		GameObject container = Instantiate (gameobjectfields.CollectionContainerPrefab) as GameObject;
		container.transform.SetParent (gameobjectfields.CollectionHUD.transform, false);	

		//creates collected item prefab
		GameObject icon = Instantiate (gameobjectfields.CollectedIngredientPrefab) as GameObject;
		icon.transform.SetParent (container.transform, false); 

		//sets the correct ingredient sprite
		icon.GetComponent<Image> ().sprite = IngredientSet.ingredientSprites_full [GameController.instance.player.GetComponent<ObjectCatcher> ().ingredientType];
//		//sets the collected amount to 1 
//		icon.transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = "1";
//		CheckIngredientComplete (icon.transform.GetChild (0).GetChild (0).GetComponent<Text>(), icon);
	}

								//	public void CheckIngredientComplete (Text collectedIngredCount, GameObject collectedItem) {
								//		//Iterates through top HUD and checks the ingredient requirements with the collection number of collectedItem
								//		//Gets each ticket
								//		for (int ii = 0; ii < gameobjectfields.TicketHUD.transform.childCount; ii++) {
								//			//bool orderComplete = true;
								//			GameObject ticket = gameobjectfields.TicketHUD.transform.GetChild (ii).gameObject;
								//			//Gets each ingredient item
								//			for (int iii = 0; iii < ticket.transform.childCount; iii++) {
								//				GameObject ingredient = ticket.transform.GetChild (iii).gameObject;
								//				//Checks if ingredient and collected item are the same
								//				if (ingredient.GetComponent<Image> ().sprite == collectedItem.GetComponent<Image> ().sprite) {
								//					//Equal
								//					if (ingredient.transform.GetChild (0).GetChild (0).GetComponent<Text> ().text == collectedIngredCount.text) {
								//						//Debug.Log ("turn text green");
								//						ingredient.transform.GetChild (0).GetChild (0).GetComponent<Text> ().color = Color.green;
								//						//ingredient.GetComponent<Image> ().color =new Color (0f, 1f, 0f, 1f);
								//					} 
								//					//Check if you have too many
								////					if (Int32.Parse (ingredient.transform.GetChild (0).GetChild (0).GetComponent<Text> ().text) < Int32.Parse (collectedIngredCount.text)) {
								////						//Debug.Log ("Collected too many: turn ticket red");
								////						//Mark Invalid
								//						markInvalid(ticket);
								////						ingredient.transform.GetChild (0).GetChild (0).GetComponent<Text> ().color = Color.red;
								////						//ticket.GetComponent<Image> ().color = new Color (1f, 0f, 0f, .5f);
								////						//ingredient.GetComponent<Image> ().color =new Color (0f, 1f, 0f, 1f);
								////						ticket.GetComponent<Image> ().sprite = gameobjectfields.InvalidTicket;
								////					}
								//				} 
								//			}
								//		}
								//	}
								//
								//	public void CheckOrderComplete () {
								//		if (GameController.instance.player.GetComponent<ObjectCatcher> ().GetnewIngredient () == true) {
								//			for (int i = 0; i < gameobjectfields.TicketHUD.transform.childCount; i++) {
								//				//individual ticket 
								//				GameObject ticket = gameobjectfields.TicketHUD.transform.GetChild (i).gameObject;
								//				bool orderComplete = true; 
								//				bool validTicket=false;
								//				for (int ii = 0; ii < ticket.transform.childCount; ii++) {
								//					//individual ingredient 
								//					GameObject ingredient = ticket.transform.GetChild (ii).gameObject;
								//
								//					bool ingredientChecked = false; 
								//
								//					bool match = IngredientSet.ingredientSprites [GameController.instance.player.GetComponent<ObjectCatcher> ().ingredientType] == ingredient.GetComponent<Image> ().sprite
								//						|| IngredientSet.ingredientSprites_full [GameController.instance.player.GetComponent<ObjectCatcher> ().ingredientType] == ingredient.GetComponent<Image> ().sprite;
								//
								//					validTicket = (validTicket || match);
								//
								////					Debug.Log ("ingredient index is: "+ii);
								////					Debug.Log ("ingredient is: "+ingredient.GetComponent<Image> ().sprite);
								////					Debug.Log ("orderComplete= "+orderComplete);
								//
								//
								//					//Get each collected ingredient
								//					for (int iii = 0; iii < gameobjectfields.CollectionHUD.transform.childCount; iii++) {
								//						GameObject collectedItem = gameobjectfields.CollectionHUD.transform.GetChild (iii).GetChild (0).gameObject;
								//
								////
								////						Debug.Log ("collection index is: "+iii);
								////						Debug.Log ("collected item: "+collectedItem.GetComponent<Image> ().sprite);
								////						Debug.Log ("orderComplete= "+orderComplete);
								//
								//						//Checks if ingredient matches collected item
								//						if (ingredient.GetComponent<Image> ().sprite == collectedItem.GetComponent<Image> ().sprite) {
								//
								//
								//
								//							//Debug.Log ("ingredient and collection sprites match");
								//
								//							//checks if the required count and collected count are equal
								////							if (ingredient.transform.GetChild (0).GetChild (0).GetComponent<Text> ().text == collectedItem.transform.GetChild (0).GetChild (0).GetComponent<Text> ().text) {
								////								orderComplete = orderComplete && true; 
								////								//Debug.Log ("Numbers match!!! orderComplete now= "+orderComplete);
								////							} 
								////							else {
								////								orderComplete = orderComplete && false;
								////								//Debug.Log ("Numbers DO NOT MATCH. orderComplete now= "+orderComplete);
								////							}
								////							ingredientChecked = ingredientChecked || true; 
								//						}
								//						ingredientChecked = ingredientChecked || false; 
								//						//Debug.Log ("did a collection item ");
								//						////Debug.Log ("ingred checked "+ingredientChecked);
								//					}
								//					if (!ingredientChecked) {
								//						orderComplete = orderComplete && false;
								//						//Debug.Log ("orderComplete= "+orderComplete);
								//					}
								//					//Debug.Log ("did a ticket ingredient ");
								//				}
								//				//Debug.Log ("did a ticket");
								//				//Check is previously marked invalid
								//				if (ticket.GetComponent<Image> ().sprite == gameobjectfields.InvalidTicket) {
								//					validTicket = validTicket && false;
								//					//Debug.Log ("SET INVALIDDDDDDDDD");
								//
								//				}
								//
								//				if (orderComplete && ticket.tag=="Ticket" && validTicket) {
								//					//Debug.Log("Mark ticket complete");
								//					//Mark Compelete
								//					//ticket.GetComponent<Image> ().color = new Color (0f, 1f, 0f, .5f);
								//					ticket.GetComponent<Image> ().sprite = gameobjectfields.CompletedTicket;
								//					for (int iii = 0; iii < ticket.transform.childCount; iii++) {
								//						GameObject ingredient = ticket.transform.GetChild (iii).gameObject;
								//						//ingredient.transform.GetChild (0).GetChild (0).GetComponent<Text> ().color = Color.white;
								//					}
								//				}
								//				if (!validTicket && ticket.tag=="Ticket") {
								//					//Debug.Log ("BAD TICKET");
								//					//Mark Invalid
								//					//ticket.GetComponent<Image> ().color = new Color (1f, 0f, 0f, .5f);
								//					//ticket.GetComponent<Image> ().sprite = gameobjectfields.InvalidTicket;
								//					markInvalid(ticket);
								//				}
								//
								//			}
								//		}
								//	
								//	}

	public void CollectionUIUpdate () {
		if (GameController.instance.player == null) {
			return;
		}

		if (GameController.instance.player.GetComponent<ObjectCatcher> ().GetnewIngredient () == true) {
							//			if (gameobjectfields.CollectionHUD.transform.childCount > 0) {
							//				bool haveIt=false;
							//				//Get each collected ingredient
							//				for (int i = 0; i < gameobjectfields.CollectionHUD.transform.childCount; i++) {
							//					GameObject collectedItem = gameobjectfields.CollectionHUD.transform.GetChild (i).GetChild(0).gameObject;
							//					//Check if previously collectedItem matches new ingredient sprite 
							//					if (IngredientSet.ingredientSprites_full [GameController.instance.player.GetComponent<ObjectCatcher> ().ingredientType] == collectedItem.GetComponent<Image> ().sprite) {
							//						//Already Collected, Update Count
							//						Text countText = collectedItem.transform.GetChild (0).GetChild (0).GetComponent<Text> ();
							//						string stringCount = countText.text.ToString ();
							//						int intCount = Int32.Parse (stringCount);
							//						intCount += 1;
							//						string newStringCount = intCount.ToString ();
							//						countText.text = newStringCount;
							//						haveIt = true;
							//						CheckIngredientComplete (countText, collectedItem);
							//
							//						break;
							//					}
							//
							//				}
							//				if (!haveIt) {
							//					CreateCollectedItem ();
							//				}
							//			}
							//			//New Item
							//			else {
							//				CreateCollectedItem ();
							//			}
			if (gameobjectfields.CollectionHUD.transform.childCount < 6) {
				CreateCollectedItem ();
			}
		}
						//CheckOrderComplete ();
		GameController.instance.player.GetComponent<ObjectCatcher> ().SetnewIngredient(false);
	}


	public void ResetUI () {
		initializeTickets = true;
		DeleteTickets ();
		DeleteCollectedIngredients ();
		// reset text fields
		textfields.levelOrderList.text = "";
		textfields.currentBurrito.text = "";
		textfields.submissionMessage.text = "";
		textfields.winMessage.text = "";
		textfields.loseMessage.text = "";
		textfields.orderTotalDisplay.text = "";
	}

	public void ResetAfterDeath() {
		DeleteTickets();
		TicketInit (0);
		TicketInit (1);
		TicketInit (2);
		DeleteCollectedIngredients ();
	}

	public void DeleteTickets() {
		DeleteTicket (0);
		DeleteTicket (1);
		DeleteTicket (2);
	}

	public void DeleteTicket(int index) {
		if (gameobjectfields.TicketHUD.transform.childCount > 2*index) {
			GameObject.DestroyObject (gameobjectfields.TicketHUD.transform.GetChild (2*index).gameObject);
			GameObject.DestroyObject (gameobjectfields.TicketHUD.transform.GetChild (2*index+1).gameObject);
		}
	}
		
	public void DeleteCollectedIngredients() {
		DeleteColelctedContianer (0); DeleteColelctedContianer (1); DeleteColelctedContianer (2);
		DeleteColelctedContianer (3); DeleteColelctedContianer (4); DeleteColelctedContianer (5);

	}


	public void DeleteColelctedContianer(int index) {
		if (gameobjectfields.CollectionHUD.transform.childCount > index) {
			GameObject.DestroyObject (gameobjectfields.CollectionHUD.transform.GetChild (index).gameObject);
		}
	}


	// Use this for initialization
	public void UpdateUI () {
		TicketUpdate();
		CollectionUIUpdate ();

        
		orders = OrderController.instance.orderList;
		textfields.levelOrderList.text = OrderController.instance.OrderListToString();
		orderTotal = orders.Count;
		textfields.orderTotalDisplay.text = orders.Count.ToString();
		if (GameController.instance.player != null) {
			textfields.currentBurrito.text = GameController.instance.player.GetComponent<ObjectCatcher> ().GetTextString ();
		}

        // display submission text for SUBMISSION_TIMER ms
        if (submissionTextTimer == 0) {
            textfields.submissionMessage.text = "";
            submissionTextTimer = SUBMISSION_TIMER;
        }
        else
        {
            submissionTextTimer--;
        }
	}

	public void setWinMessage(string msg) {
		textfields.winMessage.text = msg;
	}
	public void setSubmissionMessage(string msg) {
		textfields.submissionMessage.text = msg;
        submissionTextTimer = SUBMISSION_TIMER;
	}



	// Update is called once per frame
	void Update() {
		if (gameobjectfields.canvasHUD.activeSelf) {
			UpdateUI ();

			if (initializeTickets) {
				//	ingredientprefabs.setList();
				TicketInit (0);
				TicketInit (1);
				TicketInit (2);
				initializeTickets = false;
			}

		}
	}

}