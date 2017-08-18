using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonResponse : MonoBehaviour {

    PlayerMovement playerGO;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void PlayerRollsDice()
    {
        playerGO = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

        playerGO.RollDice();
    }

    public void PlayerMovesForward()
    {
        playerGO = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        playerGO.MoveForward();
    }


    public void PlayerMovesRight()
    {
        playerGO = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        playerGO.MoveRight();
    }


    public void PlayerMovesLeft()
    {
        playerGO = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        playerGO.MoveLeft();
    }
}

