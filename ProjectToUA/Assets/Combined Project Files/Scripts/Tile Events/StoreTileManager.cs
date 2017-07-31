using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreTileManager : MonoBehaviour 
{
	CoinManager coinObj;

	// Use this for initialization
	void Start () 
	{
		coinObj = GameObject.Find ("Coins").GetComponent<CoinManager> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		coinObj.coinValText.text = "" + coinObj.coinValue.ToString();

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
