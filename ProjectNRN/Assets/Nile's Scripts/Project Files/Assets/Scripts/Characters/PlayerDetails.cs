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
    GameObject myDie;
    GameObject myModel;


	// Use this for initialization
	void Start ()
    {
		
	}

    public int RollTheirDie ()
    {
        //myDie.GetComponent<RollDie>().RollDie();
        return dieValue;
        dieValue = 0;
    }

    
	
	// Update is called once per frame
	void Update () {
		
	}
}
