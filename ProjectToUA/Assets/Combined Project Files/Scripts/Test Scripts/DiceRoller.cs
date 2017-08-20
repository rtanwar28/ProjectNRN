using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{

    public GameObject dice;
    public Transform diceWaitPosition;
    public Transform diceDropPosition;
    public GameObject fakeEye;

    public Vector3 rollingForce;
    Vector3 dieSpeed;
    Vector3 stop;
    Vector3 eyeReposition;
    public float height;
    int faceUpValue;

    bool rollStart;
    bool dieRoll;
    bool dieRolled;
    bool dieReset;

    //Position Die above the game's map
    //Apply force/drop down slanted surface
    //Wait until die has stopped moving before repositioning fake eye looking down at top face
    //Fire a raycast from eye to face to get value

	// Use this for initialization
	void Start ()
    {
        rollingForce = new Vector3(5, 0, 0);
        stop = new Vector3(0, 0, 0);
        dieSpeed = new Vector3(0, 0, 0);
        height = 5.0f;
        rollStart = false;
        dieRoll = false;
        dieRolled = false;
        dieReset = false;
    }

    public int GetDieValue()
    {
        return faceUpValue;
    }

    public void SetReset()
    {
        dieReset = true;
    }

    public void StartRoll()
    {
        rollStart = true;
        dieRoll = true;
    }

    void GetCurrentDiceMovement()
    {
        if (dieRolled == false)
        {
            if (dieRoll == false)
            {
                dieSpeed = dice.GetComponent<Rigidbody>().velocity;
                if(dieSpeed == stop)
                {
                    dieRolled = true;
                    rollStart = false;
                }
            }
        }
    }


    // Update is called once per frame
    void Update ()
    {
        if (dieRoll == true)
        {
            dice.GetComponent<Transform>().position = diceDropPosition.GetComponent<Transform>().position;
            dice.GetComponent<Rigidbody>().AddTorque(rollingForce, ForceMode.Impulse);
            dice.GetComponent<Rigidbody>().AddForce(rollingForce, ForceMode.Impulse);
            dieRoll = false;
        }

        if (rollStart == true)
        {
            GetCurrentDiceMovement();
        }
        
        if(dieRolled == true)
        {
            eyeReposition = dice.GetComponent<Transform>().position;
            eyeReposition.y = height;
            fakeEye.GetComponent<Transform>().position = eyeReposition;
            //Fire Raycast, Get Value from face up. Send value through.
            //Ray
            dieRolled = false;
        }

        if(dieReset == true)
        {
            dice.GetComponent<Transform>().position = diceWaitPosition.GetComponent<Transform>().position;
            faceUpValue = 0;
            dieReset = false;
        }
	}
}
