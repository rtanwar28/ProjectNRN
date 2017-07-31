using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChanceTileManager : MonoBehaviour 
{

	public GameObject[] goodCards = new GameObject[3];
	public GameObject[] badCards = new GameObject[3];

	GameObject gCard, bCard, cp;//, checkCard;
	string activeCard;

	bool isGood, isBad, isCoinChance;

	CoinManager coinObj;
	PlayerMovement playerMoveObj;

	public int extraMoves;

	// Use this for initialization
	void Start () 
	{
		coinObj = GameObject.Find ("Coins").GetComponent<CoinManager> ();

		cp = this.gameObject;
		activeCard=null;
		extraMoves = 0;

		isGood = false;
		isBad = false;
		isCoinChance = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void SelectGoodCard()
	{
		if(activeCard == null)
		{
			
			int random = Random.Range (0, 3);

			gCard = goodCards [random];
	
			gCard.SetActive (true);
			isGood = true;
			activeCard = gCard.name;
			Debug.Log ("Active Card: " + activeCard);

			//StartCoroutine (WaitForPanel());
			GameObject.FindWithTag ("Player").GetComponent<TileEventManager> ().chanceTriggered = false;
			PerformAction (activeCard);
		}
	}

	public void SelectBadCard()
	{
		if (activeCard == null) 
		{
			int random = Random.Range (0, 3);

			bCard = badCards [random];

			bCard.SetActive (true);
			isBad = true;

			activeCard = bCard.name;
			Debug.Log ("Active Card: " + activeCard);

			//StartCoroutine (WaitForPanel ());
			GameObject.FindWithTag ("Player").GetComponent<TileEventManager> ().chanceTriggered = false;
			PerformAction (activeCard);
		}
	}
		
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

	void PerformAction(string selectedCard)
	{
		switch(selectedCard)
		{
		case "GoodCard_1":
			
			coinObj.coinValue += 50f;

			//Debug.Log ("G1 works");
			isCoinChance = true;
			StartCoroutine (WaitForPanel ());
			break;

		case "GoodCard_2":
			
			extraMoves = 2;
			playerMoveObj = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement> ();
			playerMoveObj.extraM = true;
			//Debug.Log ("G2 works");
			StartCoroutine (WaitForPanel ());
			break;

		case "GoodCard_3":
			
			extraMoves = 5;
			playerMoveObj = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement> ();
			playerMoveObj.extraM = true;
			//Debug.Log ("G3 works");
			StartCoroutine (WaitForPanel ());
			break;

		case "BadCard_1":
			
			coinObj.coinValue -= 25f;
			isCoinChance = true;
			StartCoroutine (WaitForPanel ());
			//Debug.Log ("B1 works");
			break;
		case "BadCard_2":
			
			/*playerMoveObj = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement> ();
			for (int x=0;x<3;x++)
			{
				Transform destTransform = playerMoveObj.playerMoveHistory.Peek ();
				//playerMoveObj.playerMoveHistory.Pop ();
				Debug.Log (x+": "+destTransform.position);
				StartCoroutine (playerMoveObj.FollowPlayer (GameObject.FindGameObjectWithTag ("Player").transform.position,destTransform.position));
				GameObject.FindGameObjectWithTag ("Player").transform.rotation = destTransform.rotation;
			}*/

			coinObj.coinValue -= 5f;
			isCoinChance = true;
			StartCoroutine (WaitForPanel ());
			//Debug.Log ("B2 works");
			break;
		case "BadCard_3":
			
			coinObj.coinValue -= 10f;
			isCoinChance = true;
			StartCoroutine (WaitForPanel ());
			//Debug.Log ("B3 works");
			break;
		default:
			break;
		}
		activeCard = null;
	}

}
