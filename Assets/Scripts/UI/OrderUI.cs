using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DigitalRuby.Tween;




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

	public Text WinScore;
	public Text WinTime;
	public Text GameCompleteTime;
	public Text LoseScore;
	public Text GameScore;
	public Text GameCompleteScore;

	public Text currentLevel;
	public Text currentLevelWin;
	public Text currentLevelLose;
	public Text currentLevelGameComplete;
	public Text timeRemaining;
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
	public GameObject CaughtIngredientTweens;

	public Sprite CompletedTicket;
	public Sprite InvalidTicket;

	public Sprite[] QualitySprites; 

	public Image WinScreen;
	public Image LoseScreen;
	public Image GameCompleteScreen;

	public Sprite FilledStar;
	public Sprite EmptyStar;


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

	private List<Order> activeOrders;
	private int orderCount;

	private Dictionary<IngredientSet.Ingredients,Sprite> spriteDict_glowing; 
	private Dictionary<IngredientSet.Ingredients,Sprite> spriteDict_full;
	private Dictionary<Sprite,Sprite> spriteDict_GlowtoFull;

	private const float SUBMISSION_TIMER = 2f;
	private float submissionTextTimer = SUBMISSION_TIMER;
	//    private static float SUBMISSION_TIMER2 = 3f;
	//    private float start; 
	//    private float submissionTextTimer2 = SUBMISSION_TIMER2;


	private const float QUALITY_TIMER = 2f;
	private float qualityTextTimer = QUALITY_TIMER;

	private int qualitySum;

	// for animations
	private Vector3 viewportToScreenOffset = new Vector3 (-Screen.width / 2, -Screen.height / 2, 0);


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
		activeOrders = OrderController.instance.activeOrders;

	}

	public GameObject TicketInit (int index){
		if (activeOrders.Count >= index+1) {
			Order currOrder = activeOrders [index];

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

			// Initialize the ticket
			ticket.GetComponent<TicketManager> ().TicketInit(currOrder);

			return ticket;
		}
		return null;
	}

	// Returns the x position where the ticket should be after all current animations are over
	public float GetTargetXPosForTicket(int index) {
		float xOffset = 0;
		for (int i = 0; i < index; i++) {
			xOffset += activeOrders [i].uiTicket.GetComponent<RectTransform> ().rect.width;
			xOffset += ticketMargin;
		}
		return xOffset;
	}

	public float GetTargetXPosForTicket(Order order) {
		int index = OrderController.instance.activeOrders.IndexOf (order);
		return GetTargetXPosForTicket(index);
	}


	public void SetQualityIndicator() {
		// Debug.Log ("Quality");
		//Debug.Log ("child="+gameobjectfields.CollectionHUD.transform.childCount.ToString()  );

		ObjectCatcher objectCatcher = GameController.instance.player.GetComponent<ObjectCatcher> ();
		if (objectCatcher.GetNumCaughtIngredients() != 0) {
			int qualitySum = objectCatcher.GetIngredients ().getSumOfQualities ();

			gameobjectfields.CollectionHUD.transform.GetChild (0).GetChild (0).GetComponent<Image> ().color = new Color (1f, 1f, 1f, 1f);
			float qualityAverage = ((float)qualitySum) / (objectCatcher.GetNumCaughtIngredients());

			if (qualityAverage > 1+2/3f) {
				gameobjectfields.CollectionHUD.transform.GetChild (0).GetChild (0).GetComponent<Image> ().sprite = (Sprite) gameobjectfields.QualitySprites.GetValue (0);
			} else if (qualityAverage >= 1+1/3f) {
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
			if (quality == 2) {
				icon.GetComponent<Image> ().sprite = IngredientSet.ingredientSprites_full [ingredientType];
				setQualityMessage ("Mmm!!!");
			} else {
				icon.GetComponent<Image> ().sprite = IngredientSet.ingredientSprites_rot [ingredientType];
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
			activeOrders[i].uiTicket.GetComponent<TicketManager>().UpdateGraphics();
		}
	}

	public void SubmitOrder(Order order) {
		order.uiTicket.GetComponent<TicketManager> ().SubmitTicket ();
	}

	public void ResetScore () {
		setScore ("0");
	}



	public void ResetUI () {
		initializeTickets = true;
		DeleteTickets ();
		DeleteCollectedIngredients ();
		// reset text fields
		textfields.levelOrderList.text = "";
		textfields.currentBurrito.text = "";

		clearGeneralMessage ();
		clearQualityMessage ();
		setLoseMessage ("");
		setWinMessage ("");


		//textfields.winMessage.text = "";
		//textfields.loseMessage.text = "";

		textfields.orderTotalDisplay.text = "";

		// make quality indicator invisible
		gameobjectfields.CollectionHUD.transform.GetChild (0).GetChild (0).GetComponent<Image> ().color = new Color (1f, 1f, 1f, 0f);

		gameobjectfields.WinScreen.gameObject.SetActive (false);
		gameobjectfields.GameCompleteScreen.gameObject.SetActive (false);
		gameobjectfields.LoseScreen.gameObject.SetActive (false);
	}

	private int GetNumberOfActiveTickets() {
		return OrderController.instance.activeOrders.Count;
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
		int count = OrderController.instance.GetNumTotalOrders ();
		setOrderCount(count.ToString());
		if (count == 1) {
			setOrderCountText ("Order Left");
		} else {
			setOrderCountText ("Orders Left");
		}
	}

	void UpdateUIMessageTimers () {
		// display submission text for SUBMISSION_TIMER ms
		if (submissionTextTimer <= 0) {
			clearGeneralMessage ();
		}
		else {
			submissionTextTimer-= Time.deltaTime;
		}

		// display quality text for QUALITY_TIMER ms
		if (qualityTextTimer <= 0) {
			clearQualityMessage ();
		}
		else {
			qualityTextTimer-= Time.deltaTime;
		}
	}


	public void Trash () {
		TrashingController.instance.Trash ();
	}



	public void setWinMessage(string msg) {
		textfields.winMessage.text = msg;
	}

	public void setLoseMessage(string msg) {
		textfields.loseMessage.text = msg;
	}

	public void setGeneralMessage(string msg) {
		submissionTextTimer = SUBMISSION_TIMER;
		textfields.generalMessage.text = msg;
	}
	public void clearGeneralMessage() {
		submissionTextTimer = 0;
		textfields.generalMessage.text = "";
	}

	public void setQualityMessage(string msg) {
		qualityTextTimer = QUALITY_TIMER;
		textfields.qualityMessage.text = msg;
	}
	public void clearQualityMessage() {
		qualityTextTimer = 0;
		textfields.qualityMessage.text = "";
	}

	public void setOrderCount(string msg) {
		textfields.orderTotalDisplay.text = msg;
	}

	public void setOrderCountText(string msg) {
		textfields.orderTotalText.text = msg;
	}

	public void setWinTime(string msg) {
		textfields.WinTime.text = msg;
		textfields.GameCompleteTime.text = msg;
	}

	public void setScore(string msg) {
		textfields.WinScore.text = msg;
		textfields.GameScore.text = msg;
		textfields.LoseScore.text = msg;
		textfields.GameCompleteScore.text = msg;
	}

	// Update is called once per frame
	void Update() {
		if (gameobjectfields.canvasHUD.activeSelf) {
			if (initializeTickets) {
				initializeTickets = false;
				CreateLevelStartTickets ();
			}
			UpdateUIOrdersLeft ();
			UpdateUIMessageTimers ();

			// only update UI for caught ingredients/ticket satisfaction if there was a change in inventory
			if (GameController.instance.player != null) {
				int newNumCaughtIngredients = GameController.instance.player.GetComponent<ObjectCatcher> ().GetNumCaughtIngredients ();
				if (newNumCaughtIngredients != lastNumCaughtIngredients) {
					GameController.instance.UpdateSubmissionValidity();
					// Only update inventory if we lost ingredients. If we caught some ingredients, we'll just update after the animated icon moves to the inventory.
					if (newNumCaughtIngredients < lastNumCaughtIngredients) {
						UpdateUIAfterInventoryChange ();
					}
					lastNumCaughtIngredients = newNumCaughtIngredients;
				}
			}
		}
	}

	void CreateLevelStartTickets() {
		// create tickets
		GameObject[] tickets = new GameObject[3];
		for (int i = 0; i < activeOrders.Count; i++) {
			tickets[i] = TicketInit (i);
		}

		// animate level start spawn
		float totalTicketWidth = GetTargetXPosForTicket(activeOrders.Count-1) + tickets[activeOrders.Count-1].GetComponent<RectTransform> ().rect.width;
		Vector2 levelStartPos = new Vector2 (320f - totalTicketWidth/2, -450f);
		for (int i = 0; i < activeOrders.Count; i++) {
			tickets[i].GetComponent<UIAnimationManager> ().LevelStartSpawnAnimation (levelStartPos);
			levelStartPos.x += tickets [i].GetComponent<RectTransform> ().rect.width + ticketMargin;
		}
	}

	public void UpdateUIAfterInventoryChange() {
		CollectionUIUpdate ();
		TicketUpdate ();
	}

	public void AnimateCaughtObject(GameObject obj) {
		GameObject icon = Instantiate (gameobjectfields.CollectedIngredientPrefab) as GameObject;
		icon.transform.SetParent (gameobjectfields.CaughtIngredientTweens.transform, false);

		// Set the correct ingredient sprite.
		IngredientSet.Ingredients ingredientType = IngredientSet.StringToIngredient(obj.name);
		int quality = obj.GetComponent<FallDecayDie>().getQuality();
		icon.GetComponent<Image> ().sprite = IngredientSet.ingredientSprites_full [ingredientType];
		if (quality == 2) {
			icon.GetComponent<Image> ().sprite = IngredientSet.ingredientSprites_full [ingredientType];
		} else {
			icon.GetComponent<Image> ().sprite = IngredientSet.ingredientSprites_rot [ingredientType];
		}

		RectTransform rect = icon.GetComponent<RectTransform> ();
		rect.anchoredPosition = Camera.main.WorldToScreenPoint (obj.transform.position) + viewportToScreenOffset;
		Vector3 startingPos = rect.anchoredPosition;
		float targetX = 65 + 55 * GameController.instance.player.GetComponent<ObjectCatcher> ().GetNumCaughtIngredients ();
		Vector3 endingPos = new Vector3(targetX + viewportToScreenOffset.x, -275, viewportToScreenOffset.z);
		float distance = Vector3.Distance (startingPos, endingPos);
		icon.Tween(obj.GetHashCode(), startingPos, endingPos, distance/500f, TweenScaleFunctions.QuadraticEaseIn, (t) => 
			{
				rect.anchoredPosition = t.CurrentValue;
			}, (t) =>
			{
				Destroy(icon);
				UpdateUIAfterInventoryChange ();
			}
		);
	}

	public void DeleteTweeningObjects() {
		foreach (Transform child in gameobjectfields.CaughtIngredientTweens.transform) {
			DigitalRuby.Tween.TweenFactory.RemoveTweenKey(child.GetHashCode(), DigitalRuby.Tween.TweenStopBehavior.DoNotModify);
			GameObject.DestroyObject (child.gameObject);
		}
	}
} 

