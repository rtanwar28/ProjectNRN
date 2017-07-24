using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreTileManager : MonoBehaviour 
{
	public Text coinText;
	float coinVal;

	// Use this for initialization
	void Start () 
	{
		coinVal = float.Parse (coinText.text);
		Debug.Log (coinVal);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(coinVal<=0)
		{
			coinText.text = "0";
		}
	}

	public void SelectItem(Button button)
	{
		int coinV = int.Parse (button.tag);
		coinVal = coinVal - coinV;

		coinText.text = "" + coinVal;

	}
}
