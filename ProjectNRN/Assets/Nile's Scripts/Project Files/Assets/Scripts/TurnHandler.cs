using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Functionality
//Players have individual ID number assigned upon joining room in player's class/alternatively set at start of game using dice roll (Determine order method in Game Manager)
//Select first player in order to act first, block other players actions.
//Start Turn
//Initial Stage: Item use and/or Dice roll
//Mid-Stage: Path Selection and movement
//Final Stage: Event Trigger and Reward/Stat Allocation
//End Turn and Select next player in order
//Repeat for all players and reset order when last player is done



public class TurnHandler : MonoBehaviour
{
    public int numOfPlayers;
    public GameObject[] playerOrder;
    public GameObject interactionUI;
    public GameObject movementUI;
    public GameObject eventUI;
    public GameObject endTurnUI;

    int currentPlayerNum;
    string tileType;
    GameObject currentPlayerObject;

    bool turnStart;
    bool initialStage;
    bool midStage;
    bool finalStage;
    bool endTurn;

	// Use this for initialization
	void Start ()
    {
        //Get numOfPlayers either from player tagged objects in scene or direct from lobby
        playerOrder = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GetPlayerOrder();
        numOfPlayers = playerOrder.Length;
        currentPlayerNum = 0;

        turnStart = true;
        initialStage = false;
        midStage = false;
        finalStage = false;
        endTurn = false;
    }

    void StartTurn ()
    {
        //Assign the turn to the current player
        currentPlayerObject = playerOrder[currentPlayerNum];
        //Only Activate UI for currentPlayerObject's Player
        interactionUI.SetActive(true);

    }

    public void InteractionStage()
    {
        //Get Dice Roll Value From Player

        //Pass into Tile Movement System

        //Disable Rolling and Item UI
        interactionUI.SetActive(false);
        //Enable Movement UI
        movementUI.SetActive(true);
    }

    public void MovementStage()
    {
        //Show Available Paths to Player and Allow Selection of One

        //Confirm movement of player and navigate them to destination

        //Check type of tile player has landed on and store as variable

        //Disable Movement UI
        movementUI.SetActive(false);
        //Enable Event UI
        eventUI.SetActive(true);
    }

    public void EventStage()
    {
        //First Check what type of title has been landed on
        if(tileType == "EnemyEncounter")
        {
            //Enable Enemy Encounter UI
            eventUI.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (tileType == "ChanceEncounter")
        {
            //Enable Event Encounter UI
            eventUI.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (tileType == "ShopEncounter")
        {
            //Enable Shop Encounter UI
            eventUI.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (tileType == "Normal")
        {
            //Proceed to EndTurn
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(turnStart == true)
        {
            StartTurn();
        }
        
        if (endTurn == true)
        {
            currentPlayerNum++;
            endTurn = false;
            turnStart = true;
        }
    }
}
