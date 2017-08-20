using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetails : MonoBehaviour
{
    public int dieValue;
    public int health;
    public int dexterity;
    public int magic;
    public int attack;
    public GameObject[] Items;

    string playerName;
    int playerID;
    int orderID;
    GameObject gameDie;
    GameObject myModel;


	// Use this for initialization
	void Start ()
    {
		
	}

    public int RollTheDie ()
    {
        dieValue = 0;
        gameDie.GetComponent<DiceRoller>().StartRoll();

        dieValue = gameDie.GetComponent<DiceRoller>().GetDieValue();
        return dieValue;
    }

    
	
	// Update is called once per frame
	void Update () {
		
	}
}
