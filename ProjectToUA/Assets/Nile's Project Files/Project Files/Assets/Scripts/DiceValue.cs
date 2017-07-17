using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceValue : GetDiceValue
{
    public int faceUpValue;

    //Add value in Inspector
    //When the camera looks at the dice's face
	public override int ReturnDiceValue ()
    {
        return faceUpValue;
	}
}
