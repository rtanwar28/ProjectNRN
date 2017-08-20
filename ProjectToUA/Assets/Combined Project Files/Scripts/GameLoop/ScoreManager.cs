using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Photon.PunBehaviour {

    int newScore;
    int tilePointValue;
    int winFightValue;
    int loseFightValue;
    int reachEndValue;
    int sChanceValue;
    int fChanceValue;

	// Use this for initialization
	void Start ()
    {
        tilePointValue = 10;
        winFightValue = 200;
        loseFightValue = -100;
        reachEndValue = 1000;
	}

    public int AffectScore(string pointEvent, int currentScore)
    {
        newScore = currentScore;
        if(pointEvent == "Tile Movement")
        {
            newScore += tilePointValue;
        }
        else if (pointEvent == "Won Fight")
        {
            newScore += winFightValue;
        }
        else if (pointEvent == "Lost Fight")
        {
            newScore += loseFightValue;
        }
        else if (pointEvent == "Reached End")
        {
            newScore += reachEndValue;
            reachEndValue = reachEndValue / 2;
        }
        return newScore;
    }
}
