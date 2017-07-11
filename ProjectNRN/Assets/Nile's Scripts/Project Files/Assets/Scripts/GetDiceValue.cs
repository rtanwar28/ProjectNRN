using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GetDiceValue : MonoBehaviour {

    int faceUpValue;

	// Get the Dice Face Value From Seen face
	public abstract int ReturnDiceValue();

    //Pass dice value to player class for interaction
    public int GetDieValue()
    {
        return faceUpValue;
    }
}
