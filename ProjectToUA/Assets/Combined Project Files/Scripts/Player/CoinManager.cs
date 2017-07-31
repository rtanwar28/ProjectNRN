using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour 
{
	public float coinValue;
	public Text coinValText;


	// Use this for initialization
	void Start () 
	{
		coinValue = 100f;

	}
	
	// Update is called once per frame
	void Update ()
	{
		coinValText.text = coinValue.ToString ();	
	}
}
