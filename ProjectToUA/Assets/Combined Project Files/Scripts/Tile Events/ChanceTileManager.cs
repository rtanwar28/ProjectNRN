using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChanceTileManager : MonoBehaviour 
{
    // Storing the good and bad cards in individual array.
	public GameObject[] goodCards = new GameObject[3];
	public GameObject[] badCards = new GameObject[3];

	GameObject gCard, bCard, cp;
	string activeCard;

	bool isGood, isBad, isCoinChance;

	// Reference to the CoinManager script.
	CoinManager coinObj;
	// Reference to the PlayerMovement script.
	PlayerMovement playerMoveObj;

	public int extraMoves;

	void Start () 
	{
		// Getting the CoinManager script component by finding the "Coins" gameobject.
		coinObj = GameObject.Find ("Coins").GetComponent<CoinManager> ();

		cp = this.gameObject;
		activeCard=null;
		extraMoves = 0;

		isGood = false;
		isBad = false;
		isCoinChance = false;
	}

    // Generate a random good card to display it.
	public void SelectGoodCard()
	{
		if(activeCard == null)
		{
			int random = Random.Range (0, 3);

			gCard = goodCards [random];
	
			gCard.SetActive (true);
			isGood = true;
			activeCard = gCard.name;

			// Setting to set the chance triggered value to false.
			GameObject.FindWithTag ("Player").GetComponent<TileEventManager> ().chanceTriggered = false;
			PerformAction (activeCard);
		}
	}

	// Generate a random bad card to display it.
	public void SelectBadCard()
	{
		if (activeCard == null) 
		{
			int random = Random.Range (0, 3);

			bCard = badCards [random];

			bCard.SetActive (true);
			isBad = true;

			activeCard = bCard.name;

			// Setting to set the chance triggered value to false.
			GameObject.FindWithTag ("Player").GetComponent<TileEventManager> ().chanceTriggered = false;
			PerformAction (activeCard);
		}
	}
	
    // To close the chance panel.
	public void CloseChancePanel()
	{
		cp.SetActive (false);
	}

	IEnumerator WaitForPanel()
	{
		yield return new WaitForSeconds (1f);
		Debug.Log ("In coroutine");
		if(isGood)
		{
			gCard.SetActive (false);
		}
		else if(isBad)
		{
			bCard.SetActive (false);
		}

		if(isCoinChance)
		{
			cp.SetActive (false);
			isCoinChance = false;
		}

        playerMoveObj = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerMoveObj.canRoll = true;

    }

    // Based on the card the player selects perform the specific action.
	void PerformAction(string selectedCard)
	{
		switch(selectedCard)
		{
		case "GoodCard_1":
			
			coinObj.coinValue += 50;

			//Debug.Log ("G1 works");
			isCoinChance = true;
			StartCoroutine (WaitForPanel ());
			break;

		case "GoodCard_2":
			
			extraMoves = 2;
			playerMoveObj = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement> ();
			playerMoveObj.extraM = true;
			StartCoroutine (WaitForPanel ());
			break;

		case "GoodCard_3":
			
			extraMoves = 5;
			playerMoveObj = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement> ();
			playerMoveObj.extraM = true;
			StartCoroutine (WaitForPanel ());
			break;

		case "BadCard_1":
			
			coinObj.coinValue -= 25;
			isCoinChance = true;
			StartCoroutine (WaitForPanel ());
			break;
		case "BadCard_2":
			coinObj.coinValue -= 5;
			isCoinChance = true;
			StartCoroutine (WaitForPanel ());
			break;
		case "BadCard_3":
			
			coinObj.coinValue -= 10;
			isCoinChance = true;
			StartCoroutine (WaitForPanel ());
			break;
		default:
			break;
		}
        // Setting the active card to null.
		activeCard = null;
	}

}
