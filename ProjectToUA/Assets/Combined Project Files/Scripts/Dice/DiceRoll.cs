using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DiceRoll : MonoBehaviour
{

	public int diceRolledA, diceRolledB, diceTotal;
    public Image diceA, diceB;
	public Sprite zero, one, two, three, four, five, six;
	public Text diceText;

    public int fightDiceRollA, fightDiceRollB, fightDiceTotal;
    public Image fightDiceA, fightDiceB;

	// Use this for initialization
	void Start ()
	{

    }
	
	// Update is called once per frame
	void Update ()
	{
		if (diceTotal != 0)
			diceText.enabled = false;
		else
			diceText.enabled = true;		
	}

	public void RollDice ()
	{
		//diceRolledA = Random.Range (1, 7);
		//diceRolledB = Random.Range (1, 7);
		diceRolledA = 1;
		diceRolledB = 1;
		diceTotal = diceRolledA + diceRolledB;
		DisplayDice (diceRolledA, diceRolledB);
	}

	public void DisplayDice (int a, int b)
	{
		showDice (diceA, a);
		showDice (diceB, b);
	}

    public void FightRollDice()
    {
        fightDiceRollA = Random.Range(1, 7);
        fightDiceRollB = Random.Range(1, 7);

        fightDiceTotal = fightDiceRollA + fightDiceRollB;
        FightDisplayDice(fightDiceRollA, fightDiceRollB);
    }

    public void FightDisplayDice(int a, int b)
    {
        showDice(fightDiceA, a);
        showDice(fightDiceB, b);
    }


    void showDice (Image img, int x)
	{
		switch (x) {
		case 0:
			img.sprite = zero;
			break;

		case 1:
			img.sprite = one;
			break;

		case 2:
			img.sprite = two;
			break;

		case 3:
			img.sprite = three;
			break;

		case 4:
			img.sprite = four;
			break;

		case 5:
			img.sprite = five;
			break;

		case 6:
			img.sprite = six;
			break;

		default:
			break;
		}
	}
}
