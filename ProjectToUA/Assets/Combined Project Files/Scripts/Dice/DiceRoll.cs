using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DiceRoll : MonoBehaviour
{
    // For the main dice for movement.
	public int diceRolledA, diceRolledB, diceTotal;

    // For the enemy fight dice.
    public int fightDiceRollA, fightDiceRollB, fightDiceTotal;

    // Image to display for the main dice.
    public Image diceA, diceB;

    // Image to display for the fight dice.
    public Image fightDiceA, fightDiceB;

	public Sprite zero, one, two, three, four, five, six;

	public Text diceText;

	// Display the dice text based on the total value of the dice
	void Update ()
	{
		if (diceTotal != 0)
			diceText.enabled = false;
		else
			diceText.enabled = true;		
	}

    // Generate random number for the main dice values
	public void RollDice ()
	{
		diceRolledA = Random.Range (1, 7);
		diceRolledB = Random.Range (1, 7);
		/* Can be set for testing purposes
        diceRolledA = 1;
		diceRolledB = 1;*/
		diceTotal = diceRolledA + diceRolledB;
		DisplayDice (diceRolledA, diceRolledB);
	}

    // Method to call another method display the main dice images.
	public void DisplayDice (int a, int b)
	{
		showDice (diceA, a);
		showDice (diceB, b);
	}

	// Generate random number for fight dice values
	public void FightRollDice()
    {
        fightDiceRollA = Random.Range(1, 7);
        fightDiceRollB = Random.Range(1, 7);

        fightDiceTotal = fightDiceRollA + fightDiceRollB;
        FightDisplayDice(fightDiceRollA, fightDiceRollB);
    }

	// Method to call another method display the fight main dice images.
	public void FightDisplayDice(int a, int b)
    {
        showDice(fightDiceA, a);
        showDice(fightDiceB, b);
    }

    // Resetting the value for the fight dices.
    public void ResetFightDice()
    {
		showDice(fightDiceA, 0);
		showDice(fightDiceB, 0);
    }

    // Displaying the image of the dice
    void showDice (Image img, int x)
	{
		switch (x) 
        {
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
