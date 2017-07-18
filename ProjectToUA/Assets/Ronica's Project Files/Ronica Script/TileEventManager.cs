using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileEventManager : MonoBehaviour 
{
	GameObject storePanel, chancePanel, topPane, bottomPane;
	public GameObject tempPanel;

	GameObject playerCam;

	// Use this for initialization
	void Start () 
	{
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
		if(Input.GetKey(KeyCode.C))
		{
			playerCam.SetActive (true);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		// Set the panel to an active on triggering with the object.
		if(other.tag == "store")
		{
			tempPanel = storePanel;
			tempPanel.SetActive (true);
		}

		if(other.tag == "chance")
		{
			tempPanel = chancePanel;
			tempPanel.SetActive (true);
		}

		if(other.tag == "enemy")
		{
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
