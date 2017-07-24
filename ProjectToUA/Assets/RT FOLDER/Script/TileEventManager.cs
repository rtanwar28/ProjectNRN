using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileEventManager : MonoBehaviour 
{
	GameObject storePanel, chancePanel, topPane, bottomPane;
	public GameObject tempPanel;

	GameObject playerCam;

	PlayerMovement playerMovementObj;

	// Use this for initialization
	void Start () 
	{
		playerMovementObj = this.GetComponent<PlayerMovement>();

		// Getting and setting the panels in a false state
		storePanel = GameObject.FindGameObjectWithTag ("store");
		storePanel.SetActive (false);

		chancePanel = GameObject.FindGameObjectWithTag ("chance");
		chancePanel.SetActive (false);

		playerCam = GameObject.FindGameObjectWithTag ("playerCam");
		playerCam.SetActive (false);

		topPane = GameObject.Find ("TopPanel");
		topPane.SetActive (false);

		bottomPane = GameObject.Find ("BottomPanel");
		bottomPane.SetActive (false);

	}

	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnTriggerEnter(Collider other)
	{

	
		if(other.tag == "store" && playerMovementObj.diceTotal == 0)
		{
				
			Debug.Log ("Store called");
			tempPanel = storePanel;
			tempPanel.SetActive (true);
		}

		// Checking if the player has triggered the chance event
		if(other.tag == "chance" && playerMovementObj.diceTotal == 0)
		{
			Debug.Log ("chance called");
			tempPanel = chancePanel;
			tempPanel.SetActive (true);
		}

		// Checking if the player triggered the enemy event
		if(other.tag == "enemy" && playerMovementObj.diceTotal == 0)
		{
			Debug.Log ("enemy called");
			tempPanel = playerCam;
			tempPanel.SetActive (true);
			topPane.SetActive (true);
			bottomPane.SetActive (true);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag == "enemy")
		{
			tempPanel = playerCam;
			tempPanel.SetActive (false);
			topPane.SetActive (false);
			bottomPane.SetActive (false);
		}
	}
}

/*if(playerMovementObj.diceTotal == 0)
		{*/
// Set the panel to an active on triggering with the object.
