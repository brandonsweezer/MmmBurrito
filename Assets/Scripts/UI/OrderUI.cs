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

	public Text generalMessage;
	public Text qualityMessage;
	public Text orderTotalDisplay;
	public Text orderTotalText;

	public Text winMessage;
	public Text loseMessage;

	public Text Score;
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
	public GameObject caughtIngredientContainer;
	public GameObject CollectionContainerPrefab;
	public GameObject CollectedIngredientPrefab;

	public Sprite CompletedTicket;
	public Sprite InvalidTicket;

	public Sprite[] QualitySprites; 

	public Image WinScreen;
	public Image LoseScreen;


}




public class OrderUI : MonoBehaviour {

	public TextFields textfields;
	public GameObjectFields gameobjectfields;

	private Text countText;

	private float ticketHeight;
	private static float ticketMargin = 10f;
	private float ticketWidthPerIngredient = 70f;
	private bool initializeTickets; 

	private ObjectCatcher objectcatcher;

	private int lastNumCaughtIngredients;


	private int numberOfIngredients;

	private List<Order> orders;
	private int orderCount;

	private Dictionary<IngredientSet.Ingredients,Sprite> spriteDict_glowing; 
	private Dictionary<IngredientSet.Ingredients,Sprite> spriteDict_full;
	private Dictionary<Sprite,Sprite> spriteDict_GlowtoFull;

    private static int SUBMISSION_TIMER = 150;
    private int submissionTextTimer = SUBMISSION_TIMER;
//	private static float SUBMISSION_TIMER2 = 3f;
//	private float start; 
//	private float submissionTextTimer2 = SUBMISSION_TIMER2;


	private static int QUALITY_TIMER = 150;
	private int qualityTextTimer = QUALITY_TIMER;

	private int qualitySum; 


	// Make this class a singleton
	public static OrderUI instance = null;
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (this);
		}
	}
	void Start() {
		initializeTickets = false;
		ticketHeight = gameobjectfields.TicketHUD.GetComponent<RectTransform> ().rect.height;
		orders = OrderController.instance.orderList;
	}

	public void TicketInit (int index){
		if (orders.Count >= index+1) {
			Order currOrder = orders [index];

			// Create the ticket prefab.
			GameObject ticket = Instantiate (gameobjectfields.TicketPrefab) as GameObject;
			ticket.transform.SetParent (gameobjectfields.TicketHUD.transform, false);
			ticket.tag=("Ticket");
			currOrder.uiTicket = ticket;

			// Size the ticket prefab based on the number of ingredients.
			numberOfIngredients = currOrder.ingredientSet.GetFullCount ();
			float containerWidth = ticketWidthPerIngredient * numberOfIngredients;
			RectTransform ticketRectangle = ticket.GetComponent<RectTransform> ();
			ticketRectangle.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, containerWidth);
			ticketRectangle.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ticketHeight);

			// Position the ticket prefab.
			ticketRectangle.anchoredPosition = new Vector2(GetTargetXPosForTicket(index), 0);
	
			// Initialize the ticket
			ticket.GetComponent<TicketManager> ().TicketInit(currOrder);
			ticket.GetComponent<TicketAnimations> ().StartSpawnAnimation ();
		}
	}

	// Returns the x position where the ticket should be after all current animations are over
	public float GetTargetXPosForTicket(int index) {
		float xOffset = 0;
		for (int i = 0; i < index; i++) {
			xOffset += orders [i].uiTicket.GetComponent<RectTransform> ().rect.width;
			xOffset += ticketMargin;
		}
		return xOffset;
	}


	public void SetQualityIndicator() {
		// Debug.Log ("Quality");
		//Debug.Log ("child="+gameobjectfields.CollectionHUD.transform.childCount.ToString()  );

		ObjectCatcher objectCatcher = GameController.instance.player.GetComponent<ObjectCatcher> ();
		if (objectCatcher.GetNumCaughtIngredients() != 0) {
			int qualitySum = objectCatcher.GetIngredients ().getSumOfQualities ();

			gameobjectfields.CollectionHUD.transform.GetChild (0).GetChild (0).GetComponent<Image> ().color = new Color (1f, 1f, 1f, 1f);
			float qualityAverage = ((float)qualitySum) / (gameobjectfields.CollectionHUD.transform.childCount - 2f);

			if (qualityAverage >= 2.4f) {
				gameobjectfields.CollectionHUD.transform.GetChild (0).GetChild (0).GetComponent<Image> ().sprite = (Sprite) gameobjectfields.QualitySprites.GetValue (0);
			} else if (qualityAverage >= 1.4f) {
				gameobjectfields.CollectionHUD.transform.GetChild (0).GetChild (0).GetComponent<Image> ().sprite = (Sprite) gameobjectfields.QualitySprites.GetValue (1);
			} else {
				gameobjectfields.CollectionHUD.transform.GetChild (0).GetChild (0).GetComponent<Image> ().sprite = (Sprite) gameobjectfields.QualitySprites.GetValue (2);
			} 
		}
		else {
			//Debug.Log ("TEST");
			gameobjectfields.CollectionHUD.transform.GetChild (0).GetChild (0).GetComponent<Image> ().color = new Color (1f, 1f, 1f, 0f);
			//gameobjectfields.CollectionHUD.transform.GetChild (0).GetComponent<Image> ().sprite = gameobjectfields.QualitySprites.GetValue (4);

		}
	}

	public void CollectionUIUpdate () {
		if (GameController.instance.player == null) {
			return;
		}

		SetQualityIndicator ();

		DeleteCollectedIngredients ();

		ObjectCatcher objectCatcherScript = GameController.instance.player.GetComponent<ObjectCatcher> ();
		int numCaughtIngredients = objectCatcherScript.GetNumCaughtIngredients ();
		for (int i = 0; i < numCaughtIngredients; i++) {
			//create the collection container prefab 
			GameObject container = Instantiate (gameobjectfields.CollectionContainerPrefab) as GameObject;
			container.transform.SetParent (gameobjectfields.caughtIngredientContainer.transform, false);	

			//create collected item prefab
			GameObject icon = Instantiate (gameobjectfields.CollectedIngredientPrefab) as GameObject;
			icon.transform.SetParent (container.transform, false); 

			//set the correct ingredient sprite
			IngredientSet.Ingredients ingredientType;
			int quality;
			// set sprite
			objectCatcherScript.GetIngredients().GetNthIngredient(i, out ingredientType, out quality);
			icon.GetComponent<Image> ().sprite = IngredientSet.ingredientSprites_full [ingredientType];
			// set quality tint
			if (quality == 3) {
				icon.GetComponent<Image> ().color = new Color (1f, 1f, 1f, 1f);
				setQualityMessage ("Mmm!!!");
			} else if (quality == 2) {
				icon.GetComponent<Image> ().color = new Color (.64f, .83f, .0f, 1f);
				setQualityMessage ("Okay");
			} else {
				icon.GetComponent<Image> ().color = new Color (.38f, .71f, .28f, 1f);
				setQualityMessage ("Bleh!!");
			}
		}
	}



	public void TicketUpdate() {
		if (GameController.instance.player == null) {
			return;
		}

		int numActiveTickets = GetNumberOfActiveTickets ();
		for (int i = 0; i < numActiveTickets; i++) {
			orders[i].uiTicket.GetComponent<TicketManager>().UpdateGraphics();
			orders[i].uiTicket.GetComponent<TicketAnimations> ().StartMoveToXPosition(GetTargetXPosForTicket(i));
		}
	}
		


	public void ResetUI () {
		initializeTickets = true;
		DeleteTickets ();
		DeleteCollectedIngredients ();
		// reset text fields
		textfields.levelOrderList.text = "";
		textfields.currentBurrito.text = "";

		setGeneralMessage("");
		setQualityMessage ("");
		setLoseMessage ("");
		setWinMessage ("");

		//textfields.winMessage.text = "";
		//textfields.loseMessage.text = "";

		textfields.orderTotalDisplay.text = "";

		// make quality indicator invisible
		gameobjectfields.CollectionHUD.transform.GetChild (0).GetChild (0).GetComponent<Image> ().color = new Color (1f, 1f, 1f, 0f);

		gameobjectfields.WinScreen.gameObject.SetActive (false);
		gameobjectfields.LoseScreen.gameObject.SetActive (false);
	}

	private int GetNumberOfActiveTickets() {
		return Math.Min (3, OrderController.instance.orderList.Count);
	}

	private int GetNumberOfDisplayedTickets() {
		return gameobjectfields.TicketHUD.transform.childCount;
	}

	public void ResetAfterDeath() {
		DeleteCollectedIngredients ();
		qualitySum = 0;
		gameobjectfields.CollectionHUD.transform.GetChild (0).GetChild (0).GetComponent<Image> ().color = new Color (1f, 1f, 1f, 0f);
		//SetQualityIndiator();
		//CollectionInit ();
	}

	public void DeleteTickets() {
		foreach (Transform child in gameobjectfields.TicketHUD.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}

	public void DeleteTicket(int index) {
		if (gameobjectfields.TicketHUD.transform.childCount > index) {
			GameObject.DestroyObject (gameobjectfields.TicketHUD.transform.GetChild (index).gameObject);
		}
	}
		
	public void DeleteCollectedIngredients() {
		foreach (Transform child in gameobjectfields.caughtIngredientContainer.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}


	void UpdateUIOrdersLeft () {
		textfields.levelOrderList.text = OrderController.instance.OrderListToString();
		setOrderCount(orders.Count.ToString());
		if (orders.Count == 1) {
			setOrderCountText ("Order Left");
		} else {
			setOrderCountText ("Orders Left");
		}
	}

	void UpdateUIMessageTimers () {
		// display submission text for SUBMISSION_TIMER ms
		if (submissionTextTimer == 0) {
			setGeneralMessage ("");
			submissionTextTimer = SUBMISSION_TIMER;
		}
		else {
			submissionTextTimer--;
		}

		// display quality text for QUALITY_TIMER ms
		if (qualityTextTimer == 0) {
			setQualityMessage ("");
			qualityTextTimer = QUALITY_TIMER;
		}
		else {
			qualityTextTimer--;
		}
	}


//	void Trash () {
//		TrashingController.instance.
//	}
//


	public void setWinMessage(string msg) {
		textfields.winMessage.text = msg;
	}

	public void setLoseMessage(string msg) {
		textfields.loseMessage.text = msg;
	}

	public void setGeneralMessage(string msg) {
		textfields.generalMessage.text = msg;
	}


	public void setQualityMessage(string msg) {
		textfields.qualityMessage.text = msg;
	}

	public void setOrderCount(string msg) {
		textfields.orderTotalDisplay.text = msg;
	}

	public void setOrderCountText(string msg) {
		textfields.orderTotalText.text = msg;
	}

	public void setScore(string msg) {
		textfields.Score.text = msg;
	}

	// Update is called once per frame
	void Update() {
		if (gameobjectfields.canvasHUD.activeSelf) {
			if (initializeTickets) {
				//	ingredientprefabs.setList();
				TicketInit (0);
				TicketInit (1);
				TicketInit (2);
				initializeTickets = false;
			}
			UpdateUIOrdersLeft ();
			UpdateUIMessageTimers ();

			// only update UI for caught ingredients/ticket satisfaction if there was a change in inventory
			if (GameController.instance.player != null) {
				int newNumCaughtIngredients = GameController.instance.player.GetComponent<ObjectCatcher> ().GetNumCaughtIngredients ();
				if (newNumCaughtIngredients != lastNumCaughtIngredients) {
					UpdateUIAfterInventoryChange ();
					lastNumCaughtIngredients = newNumCaughtIngredients;
				}
			}
		}
	}

	public void UpdateUIAfterInventoryChange() {
		GameController.instance.UpdateSubmissionValidity();
		CollectionUIUpdate ();
		TicketUpdate ();
	}
}