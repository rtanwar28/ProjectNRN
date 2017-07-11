using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int numOfPlayers;
    public GameObject TurnManager;
    public GameObject[] players;
    public GameObject[] playerOrder;
    public int[] initialRoll;
    int temp;
    GameObject tempPlayer;
    bool orderDecided;

	// Use this for initialization
	void Start ()
    {
        orderDecided = false;
        players = GameObject.FindGameObjectsWithTag("Player");
        numOfPlayers = players.Length;
        //initialRoll = new int[] { 2, 1, 6, 3 };
	}

    void DecidePlayerOrder()
    {
        //Get Each of the Players
        //Roll their die
        //Compare their die score
        //order from highest to lowest for TurnHandler
        for (int i = 0; i < numOfPlayers; i++)
        {
            initialRoll[i] = players[i].GetComponent<PlayerDetails>().RollTheirDie();
            Debug.Log(players[i]);
            Debug.Log(initialRoll[i]);
        }

        playerOrder = OrderRollValues();

        for(int i = 0; i < playerOrder.Length; i++)
        {
            Debug.Log(playerOrder[i]);
        }
    }

    GameObject[] OrderRollValues()
    {
        for (int i = 0; i < initialRoll.Length; i++)
        {
            for (int j = i + 1; j < initialRoll.Length; j++)
            {
                if (initialRoll[i] < initialRoll[j])
                {
                    temp = initialRoll[j];
                    tempPlayer = players[j];
                    initialRoll[j] = initialRoll[i];
                    players[j] = players[i];
                    initialRoll[i] = temp;
                    players[i] = tempPlayer;
                    Debug.Log(initialRoll[i]);
                }
            }
        }
        return players;
    }

    public GameObject[] GetPlayerOrder()
    {
        return playerOrder;
    }

    // Update is called once per frame
    void Update ()
    {
        if(orderDecided == false)
        {
            DecidePlayerOrder();
            orderDecided = true;
        }
	}
}
