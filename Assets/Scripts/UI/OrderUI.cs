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

    private static int SUBMISSION_TIMER = 100;
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
		

	public void TicketInit (int index){
		if (orders.Count >= index+1) {

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

			//container.GetComponent<RectTransform> ().rect.width = containerWidth;
			container.GetComponent<RectTransform> ().sizeDelta = new Vector2 (containerWidth,60f);

	
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
				}
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
				ingredient.GetComponent<Image> ().sprite = IngredientSet.ingredientSprites_GlowtoFull [ingredient.GetComponent<Image> ().sprite];
				ingredient.tag = ("Untagged");
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
		icon.GetComponent<Image> ().sprite = IngredientSet.ingredientSprites_full [GameController.instance.player.GetComponent<ObjectCatcher> ().GetIngredientType()];

		int quality = GameController.instance.player.GetComponent<ObjectCatcher> ().GetIngredientQuality();
		if (quality == 5 || quality == 4) {
			icon.GetComponent<Image> ().color = new Color (1f, 1f, 1f, 1f);
		} else if (quality == 3 || quality == 2) {
			icon.GetComponent<Image> ().color = new Color (.64f, .83f, .0f, 1f);
		} else {
			icon.GetComponent<Image> ().color = new Color (.38f, .71f, .28f, 1f);
		}
	}

	public void CollectionUIUpdate () {
		if (GameController.instance.player == null) {
			return;
		}

		if (GameController.instance.player.GetComponent<ObjectCatcher> ().GetnewIngredient () == true) {
			if (gameobjectfields.CollectionHUD.transform.childCount < 5) {
				CreateCollectedItem ();
				//CollectionInit ();
			}
			else if (gameobjectfields.CollectionHUD.transform.childCount ==5) {
				CreateCollectedItem ();
				//CollectionInit ();
				setSubmissionMessage ("Buritto Full!");
			}else {
				setSubmissionMessage ("Cannot Pickup");
				GameController.instance.player.GetComponent<ObjectCatcher> ().SetnewIngredient(false);
			}
		}

	}


//	public void CollectionInit(){
//		Debug.Log ("collection Init");
//		DeleteCollectedIngredients ();
//
//		List<IngredientSet.Ingredients> collectionList = GameController.instance.player.GetComponent<ObjectCatcher> ().getIngredients().getIngredientOrderList();
//
//		Debug.Log (collectionList.Count);
//		for (int i = 0; i < collectionList.Count; i++) {
//				CreateCollectedItem (collectionList[i]);
//			Debug.Log ("Create");
//		}

//		IngredientSet collectionSet = GameController.instance.player.GetComponent<ObjectCatcher> ().getIngredients ().ingredientSet;
//
//		Debug.Log (collectionSet.ingredients.Length);
//		for (int i = 0; i < collectionSet.ingredients.Length; i++) {
//			Debug.Log (collectionSet.ingredients [i]);
//			for (int ii = 0; ii < collectionSet.ingredients [i]; ii++) {
//				CreateCollectedItem ((IngredientSet.Ingredients)i);
//				Debug.Log ("Create");
//			}
//		}
//	}



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

						bool match = IngredientSet.ingredientSprites_full [GameController.instance.player.GetComponent<ObjectCatcher> ().GetIngredientType()] == ingredient.GetComponent<Image> ().sprite;

						validTicket = ((validTicket || match) && !markedInvalidTicket);

						if (match && validTicket && ingredient.tag!="MarkedIngredient") {

	
							//Set filled image in top HUD
							//Marks ingredient active
							if (continueLoop) {
								ingredient.GetComponent<Image> ().sprite = IngredientSet.ingredientSprites_glowing [GameController.instance.player.GetComponent<ObjectCatcher> ().GetIngredientType()];
								ingredient.GetComponent<Image> ().color = Color.white;
								ingredient.tag = ("MarkedIngredient");
								validIngredient = true;
								continueLoop = false;
							}
						}

						//Check if ingredient collected matches an order ingredient
						if (ingredient.tag == "MarkedIngredient") {
							completedIngredients++; 
						}

					}
					if (completedIngredients == ticket.transform.childCount && ticket.tag=="Ticket") {
						//Mark Complete
						ticket.GetComponent<Image> ().sprite = gameobjectfields.CompletedTicket;
						GameController.instance.canSubmit= true; 
					}
					if ((!validTicket || !validIngredient) && ticket.tag=="Ticket") {
						//Mark Invalid
						markInvalid(ticket);
					}
				}
			}
			GameController.instance.player.GetComponent<ObjectCatcher> ().SetnewIngredient(false);
		}
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
		//CollectionInit ();
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
			CollectionUIUpdate ();
			TicketUpdate ();



        
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