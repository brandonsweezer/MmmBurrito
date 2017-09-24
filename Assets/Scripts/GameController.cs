using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject player;
	
	public Dictionary<Order, int> orderList;

    public int gameTime;

	private OrderList globalOrders;

	void Start () {
		orderList = new Dictionary<Order, int> ();
		globalOrders = new OrderList ();
		//should be different for each level
		orderList.Add (globalOrders.getOrders(3), 1);
		foreach (KeyValuePair<Order, int> entry in orderList) {
			for (int i = 0; i < entry.Value; i++) {
				entry.Key.print ();
			}
		}
        StartCoroutine(StartCountdown());
	}
    float currCountdownValue;
    public IEnumerator StartCountdown(float countdownValue = 10f){
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            //We should display this number instead of printing
            Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
        //Timer is now 0, we lose
        //Again, display this
        Debug.Log("Time Elapsed - YOU LOSE");
    }
}
