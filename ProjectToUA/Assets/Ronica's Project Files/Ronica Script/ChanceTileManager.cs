using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceTileManager : MonoBehaviour 
{

	public GameObject[] goodCards = new GameObject[3];
	public GameObject[] badCards = new GameObject[3];

	GameObject gCard, bCard, cp;

	// Use this for initialization
	void Start () 
	{
		cp = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void SelectGoodCard()
	{
		int random = Random.Range (0, 3);

		gCard = goodCards [random];

		gCard.SetActive (true);

		StartCoroutine (Chill());
	}

	public void SelectBadCard()
	{
		int random = Random.Range (0, 2);

		bCard = badCards [random];

		bCard.SetActive (true);

		StartCoroutine (Chill());
	}


	public void CloseChancePanel()
	{
		gCard.SetActive (false);
		bCard.SetActive (false);
	}

	IEnumerator Chill()
	{
		yield return new WaitForSeconds (1f);

		cp.SetActive (false);
		gCard.SetActive (false);
		bCard.SetActive (false);
	}


}
