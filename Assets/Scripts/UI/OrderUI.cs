using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;




public class OrderUI : MonoBehaviour {


	public GameObject canvasHUD;

	public Text levelOrderList;
	public Text currentBurrito;
	public Text submissionMessage;
	public Text winMessage;
	public Text loseMessage;
	public Text orderTotalDisplay;

	public GameObject TicketPrefab;
	public GameObject IngredientPrefab;
	public GameObject Divider;
	public GameObject TicketHUD;

	public GameObject CollectionHUD;
	public GameObject CollectionContainerPrefab;
	public GameObject CollectedIngredientPrefab;

	private Text countText;

	private bool initializeTickets; 

	private ObjectCatcher objectcatcher; 


	private int orderTotal;
	private int ingredientTotal;

	private List<IngredientSet> orders;
	private int orderCount;

	private Dictionary<IngredientSet.Ingredients,Sprite> spriteDict; 
	private Dictionary<IngredientSet.Ingredients,Sprite> spriteDict_full; 


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
	public void SetUIsize(int ingredientTotal, GridLayoutGroup tick){
		if (ingredientTotal == 1) {
			tick.cellSize = new Vector2 (150, 60);
			tick.constraintCount = 1;
			//Debug.Log ("one order");
		}
		else if (ingredientTotal == 2) {
			tick.cellSize = new Vector2 (75, 60);
			tick.constraintCount = 2;
		}

		else if (ingredientTotal <= 4) {
			tick.cellSize = new Vector2 (75, 30);
			tick.constraintCount = 2;
		}

		else {
			tick.cellSize = new Vector2 (50, 30);
			tick.constraintCount = 3;
		}
	}


	public void TicketInit (int index){
		if (orders.Count >= index+1) {
			
			//gets grid to give to SetUISize to set boxes
			GridLayoutGroup tick = TicketPrefab.GetComponent<GridLayoutGroup> ();

			//gets the current order
			IngredientSet currOrder = orders [index];
			//number of ingredients in the order 
			ingredientTotal = currOrder.GetFullCount ();
			SetUIsize (ingredientTotal, tick);

			//creates the ticket prefab container and divider
			GameObject container = Instantiate (TicketPrefab) as GameObject;
			container.transform.SetParent (TicketHUD.transform, false);
			container.tag=("Ticket");
			GameObject divider = Instantiate (Divider) as GameObject;
			divider.transform.SetParent (TicketHUD.transform, false);
				

			//creates an ingredient prefab for each ingredient in the current order
			int count;
			foreach (IngredientSet.Ingredients ingredient in Enum.GetValues(typeof(IngredientSet.Ingredients))) {
				count = currOrder.GetCount (ingredient);
				if (count == 0)
					continue;
				GameObject icon = Instantiate (IngredientPrefab) as GameObject;
				icon.transform.SetParent (container.transform, false);

				//sets the correct ingredient sprite
				icon.GetComponent<Image> ().sprite = IngredientSet.ingredientSprites[ingredient];
				//sets the required number of this particular ingredient 
				icon.transform.GetChild(0).GetChild(0).GetComponent<Text>().text= count.ToString();


			//We have a UI ticket (tick). We have an order (currOrd). Need to figure out how many inredients in that order (ingredientsTotal). Iterate through the enum 
			//creating a box for each ingredient. Affix the ingredient number to that box. 

			}

		}
	}

	public void TicketUpdate() {
		CaughtIngredientSet currIngredsSet=GameController.instance.player.GetComponent<ObjectCatcher> ().getIngredients();
		IngredientSet currIngreds = currIngredsSet.ingredientSet;


		if (GameController.instance.player.GetComponent<ObjectCatcher> ().GetnewIngredient ()==true){
			//if TicketHUD has Ticket children
			if (TicketHUD.transform.childCount > 0) {
				//Get each ticket
				for (int i = 0; i < TicketHUD.transform.childCount; i++) {
					//individual ticket 
					GameObject ticket= TicketHUD.transform.GetChild(i).gameObject;
					bool invalidTicket=false;
					bool OrderComplete=true;
					for (int ii = 0; ii < ticket.transform.childCount; ii++) {
						Debug.Log ("index is:"+ii);
						Debug.Log ("bool= "+OrderComplete);
						//individual ingredient 
						GameObject ingredient= ticket.transform.GetChild(ii).gameObject;

						bool match = IngredientSet.ingredientSprites [GameController.instance.player.GetComponent<ObjectCatcher> ().ingredientType] == ingredient.GetComponent<Image> ().sprite
						            || IngredientSet.ingredientSprites_full [GameController.instance.player.GetComponent<ObjectCatcher> ().ingredientType] == ingredient.GetComponent<Image> ().sprite;

						invalidTicket = (invalidTicket || match);

//						foreach (IngredientSet.Ingredients collectedIngredient in Enum.GetValues(typeof(IngredientSet.Ingredients))){
//							invalidTicket= (invalidTicket || (
//						}

						//Check if ingredient collected matches an order ingredient
						if (match) {

							//Set filled image
							ingredient.GetComponent<Image> ().sprite = IngredientSet.ingredientSprites_full [GameController.instance.player.GetComponent<ObjectCatcher> ().ingredientType];
							ingredient.GetComponent<Image> ().color = Color.white;

							//Decrement the count by one 
							Text countText = ingredient.transform.GetChild (0).GetChild (0).GetComponent<Text> ();
							string stringCount = countText.text.ToString ();
							int intCount = Int32.Parse (stringCount);
							intCount -= 1;


							for (int iii = 0; iii < CollectionHUD.transform.childCount; iii++) {
								GameObject collectedItem = CollectionHUD.transform.GetChild (iii).GetChild(0).gameObject;
								if (ingredient.transform.GetChild (0).GetChild (0).GetComponent<Text> ()== collectedItem.transform.GetChild (0).GetChild (0).GetComponent<Text> ()) {
									ingredient.GetComponent<Image> ().color =new Color (0f, 1f, 0f, 1f);
									countText.color = Color.green;
								}
							}




							//Ingredient Remaning Logic
							if (intCount == 0) {
								ingredient.GetComponent<Image> ().color =new Color (0f, 1f, 0f, 1f);
								countText.color = Color.green;
								Debug.Log ("IS ZERO");
							}
							if (intCount < 0) {
								Debug.Log ("LESS than zero");
								intCount = 0; 
								ticket.GetComponent<Image> ().color = new Color (1f, 0f, 0f, .5f);
								countText.color = Color.black;
								ingredient.GetComponent<Image> ().color =new Color (.2f, .2f, .2f, .5f);
							}
							if (intCount > 0) {
								Debug.Log ("MORE to go");
								OrderComplete = false;
							}
							string newStringCount = intCount.ToString ();
							//countText.text = newStringCount;
						}
							
					}
					if (!invalidTicket && ticket.tag=="Ticket") {
						Debug.Log ("BAD TICKET");
						ticket.GetComponent<Image> ().color = new Color (1f, 0f, 0f, .5f);
					}
//					if (OrderComplete && ticket.tag=="Ticket") {
//						Debug.Log ("Turn Green");
//						ticket.GetComponent<Image> ().color = Color.green;
//						OrderComplete = false;
//					}
				}
			}
		}
	}

	public void CreateCollectedItem () {
		//Debug.Log ("Create a new item");
		//creates the collection container prefab 
		GameObject container = Instantiate (CollectionContainerPrefab) as GameObject;
		container.transform.SetParent (CollectionHUD.transform, false);	

		//creates collected item prefab
		GameObject icon = Instantiate (CollectedIngredientPrefab) as GameObject;
		icon.transform.SetParent (container.transform, false); 

		//sets the correct ingredient sprite
		icon.GetComponent<Image> ().sprite = IngredientSet.ingredientSprites_full [GameController.instance.player.GetComponent<ObjectCatcher> ().ingredientType];
		//sets the collected amount to 1 
		icon.transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = "1";
	}


	public void CollectionUIUpdate () {
		if (GameController.instance.player.GetComponent<ObjectCatcher> ().GetnewIngredient () == true) {
			if (CollectionHUD.transform.childCount > 0) {
				bool haveIt=false; 
				for (int i = 0; i < CollectionHUD.transform.childCount; i++) {
					GameObject collectedItem = CollectionHUD.transform.GetChild (i).GetChild(0).gameObject;
					if (IngredientSet.ingredientSprites_full [GameController.instance.player.GetComponent<ObjectCatcher> ().ingredientType] == collectedItem.GetComponent<Image> ().sprite) {
						//Already Collected, Update Count
						Text countText = collectedItem.transform.GetChild (0).GetChild (0).GetComponent<Text> ();
						string stringCount = countText.text.ToString ();
						int intCount = Int32.Parse (stringCount);
						intCount += 1;
						string newStringCount = intCount.ToString ();
						countText.text = newStringCount;
						haveIt = true;
						break;
					}
				}
				if (!haveIt) {
					CreateCollectedItem ();
				}
			}
			//New Item
			else {
				CreateCollectedItem ();
			}
		}

		GameController.instance.player.GetComponent<ObjectCatcher> ().SetnewIngredient(false);
	}


	public void ResetUI () {
		initializeTickets = true;
		DeleteTickets ();
		DeleteCollectedIngredients ();
		// reset text fields
		levelOrderList.text = "";
		currentBurrito.text = "";
		submissionMessage.text = "";
		winMessage.text = "";
		loseMessage.text = "";
		orderTotalDisplay.text = "";
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
		if (TicketHUD.transform.childCount > 2*index) {
			GameObject.DestroyObject (TicketHUD.transform.GetChild (2*index).gameObject);
			GameObject.DestroyObject (TicketHUD.transform.GetChild (2*index+1).gameObject);
		}
	}
		
	public void DeleteCollectedIngredients() {
		DeleteColelctedContianer (0); DeleteColelctedContianer (1); DeleteColelctedContianer (2);
		DeleteColelctedContianer (3); DeleteColelctedContianer (4); DeleteColelctedContianer (5);

	}


	public void DeleteColelctedContianer(int index) {
			if (CollectionHUD.transform.childCount > index) {
				GameObject.DestroyObject (CollectionHUD.transform.GetChild (index).gameObject);
		}
	}


	// Use this for initialization
	public void UpdateUI () {
		TicketUpdate();
		CollectionUIUpdate ();


		orders = OrderController.instance.orderList;
		levelOrderList.text = OrderController.instance.OrderListToString();
		orderTotal = orders.Count;
		orderTotalDisplay.text = orders.Count.ToString();
		if (GameController.instance.player != null) {
			currentBurrito.text = GameController.instance.player.GetComponent<ObjectCatcher> ().GetTextString ();
		}
	}

	public void setWinMessage(string msg) {
		winMessage.text = msg;
	}
	public void setSubmissionMessage(string msg) {
		submissionMessage.text = msg;
	}



	// Update is called once per frame
	void Update() {
		if (canvasHUD.activeSelf) {
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