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



public class TurnHandler : Photon.PunBehaviour
{
    public int numOfPlayers;
    public int[] playerOrder;
    public GameObject[] playerObjects;
    public GameObject turnUI, timerUI, currentTurnUI, gameManager;
    public Transform TOne, TTwo, TThree, TFour;

    public int currentTurn;
    public int currentPlayerNum;
    string cPlayerName;
    public GameObject currentPlayerObject;

    bool pTurnEnabled;
    bool pDiceEnabled;

    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        currentTurnUI = GameObject.FindGameObjectWithTag("TurnHL");
        TOne = GameObject.FindGameObjectWithTag("TOne").GetComponent<Transform>();
        TTwo = GameObject.FindGameObjectWithTag("TTwo").GetComponent<Transform>();
        TThree = GameObject.FindGameObjectWithTag("TThree").GetComponent<Transform>();
        TFour = GameObject.FindGameObjectWithTag("TFour").GetComponent<Transform>();

        //Get numOfPlayers either from player tagged objects in scene or direct from lobby
        playerObjects = gameManager.GetComponent<NRN.Tales.ConnectionManager>().players;
        playerOrder = gameManager.GetComponent<NRN.Tales.ConnectionManager>().GetPlayerOrder();

        numOfPlayers = playerOrder.Length;
        currentPlayerNum = 0;
        currentTurn = 1; //playerOrder[currentPlayerNum];
        for (int i = 0; i < playerObjects.Length; i++)
        {
            Debug.Log("Player ID: " + playerOrder[i]);
            if (playerObjects[i].GetComponent<PlayerMovement>().playerID == currentTurn)
            {
                Debug.Log("Current Player ID: " + currentTurn);
                currentPlayerObject = playerObjects[i];
                currentPlayerObject.GetComponent<PlayerMovement>().playerTurn = true;
                cPlayerName = currentPlayerObject.GetComponent<PlayerMovement>().username;
                TurnUIMovement(currentTurn);
            }
            else
            {
                Debug.Log("Player " + playerOrder[i] + "your turn should be false.");
                playerObjects[i].GetComponent<PlayerMovement>().playerTurn = false;
            }
        }

    }

    void TurnUIMovement(int currentPlayer)
    {
        if (currentPlayer == 1)
        {
            currentTurnUI.GetComponent<Transform>().position = TOne.transform.position;
        }
        else if (currentPlayer == 2)
        {
            currentTurnUI.GetComponent<Transform>().position = TTwo.transform.position;
        }
        else if (currentPlayer == 3)
        {
            currentTurnUI.GetComponent<Transform>().position = TThree.transform.position;
        }
        else if (currentPlayer == 4)
        {
            currentTurnUI.GetComponent<Transform>().position = TFour.transform.position;
        }

    }

    public void IncrementCurrentTurn(bool turnState)
    {
        if (turnState == false)
        {
            currentPlayerNum++;
            if (currentPlayerNum == playerOrder.Length)
            {
                currentPlayerNum = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPlayerObject.GetComponent<PlayerMovement>().playerTurn == true)
        {
            //Enable Appropriate ui if your photonview

        }
        else if (currentPlayerObject.GetComponent<PlayerMovement>().playerTurn == false)
        {
            Debug.Log("IS THIS BEING CALLED!!!!!");
            IncrementCurrentTurn(currentPlayerObject.GetComponent<PlayerMovement>().playerTurn);
            //Disable all appropriate UI
            currentTurn = playerOrder[currentPlayerNum];

            for (int i = 0; i < playerObjects.Length; i++)
            {
                if (playerObjects[i].GetComponent<PlayerMovement>().playerID == currentTurn)
                {
                    if (playerObjects[i].GetComponent<PlayerMovement>().reachedEnd == false)
                    {
                        currentPlayerObject = playerObjects[i];
                        currentPlayerObject.GetComponent<PlayerMovement>().playerTurn = true;
                        cPlayerName = currentPlayerObject.GetComponent<PlayerMovement>().username;
                        TurnUIMovement(currentTurn);
                    }
                    Debug.Log("Player " + playerOrder[i] + "your turn should be false.");
                }
            }
        }
    }
}
