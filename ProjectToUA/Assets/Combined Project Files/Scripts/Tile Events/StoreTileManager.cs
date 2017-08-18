using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreTileManager : MonoBehaviour 
{
	// Reference to the CoinManager script.
	CoinManager coinObj;

	void Start () 
	{
		// Getting the CoinManager script component by finding the "Coins" gameobject.
		coinObj = GameObject.Find ("Coins").GetComponent<CoinManager> ();
	}
	
	void Update () 
	{
        // Setting the coin value that is displayed.
		coinObj.coinValText.text = "" + coinObj.coinValue.ToString();

        // if the coin value is less than equal to 0, set it as 0.
		if(coinObj.coinValue<=0)
		{
			coinObj.coinValText.text = "0";
		}
	}


	public void SelectItem(Button button)
	{
		int coinV = int.Parse (button.tag);
		coinObj.coinValue = coinObj.coinValue - coinV;
	}
}
