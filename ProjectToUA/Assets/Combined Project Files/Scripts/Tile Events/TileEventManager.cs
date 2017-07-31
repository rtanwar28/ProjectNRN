using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileEventManager : Photon.PunBehaviour
{
	GameObject storePanel, chancePanel, enemyPanel;
	public GameObject tempPanel;

	//PlayerMovement playerMovementObj;
	DiceRoll rollObj;

	public bool chanceTriggered;

	// Use this for initialization
	void Start () 
	{
        if (photonView.isMine)
        {
            //playerMovementObj = this.GetComponent<PlayerMovement>();
            rollObj = GameObject.Find("Dice").GetComponent<DiceRoll>();

            // Getting and setting the panels in a false state
            storePanel = GameObject.FindGameObjectWithTag("store");
            storePanel.SetActive(false);

            chancePanel = GameObject.FindGameObjectWithTag("chance");
            chancePanel.SetActive(false);

            enemyPanel = GameObject.FindGameObjectWithTag("enemy");
            enemyPanel.SetActive(false);

            chanceTriggered = false;
        }
	}

	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnTriggerEnter(Collider other)
	{
        if (photonView.isMine)
        {

            if (other.tag == "Shop" && rollObj.diceTotal == 0)
            {

                Debug.Log("Store called");
                tempPanel = storePanel;
                tempPanel.SetActive(true);
            }

            // Checking if the player has triggered the chance event
            if (other.tag == "QMark" && rollObj.diceTotal == 0 && this.GetComponent<PlayerMovement>().extraMoveCount == 0 && !chanceTriggered)
            {
                chanceTriggered = true;
                this.GetComponent<PlayerMovement>().canRoll = false;
                Debug.Log("chance called");
                tempPanel = chancePanel;
                tempPanel.SetActive(true);
            }

            // Checking if the player triggered the enemy event
            if (other.tag == "Fight" && rollObj.diceTotal == 0)
            {
                Debug.Log("enemy called");
                tempPanel = enemyPanel;
                tempPanel.SetActive(true);
            }
        }
	}

	void OnTriggerExit(Collider other)
	{
        if (photonView.isMine)
        {
            if (other.tag == "enemy")
            {
                tempPanel = enemyPanel;
                tempPanel.SetActive(false);
            }
        }
	}
}
