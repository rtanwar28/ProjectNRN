using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTileManager : Photon.MonoBehaviour
{
    // Reference to the TileEventManager script.
    TileEventManager tileManObj;
    // Reference to the DiceRoll script.
    DiceRoll diceRollObj;
    // Reference to the CoinManager script.
    CoinManager coinObj;

    // public variables for the choice, fight and dice roll panel.
    public GameObject cPanel;
    public GameObject fPanel;
    public GameObject rPanel;

    public bool choiceSelected, isFightRoll, enemyRolling;

    // Integer values during the enemy fight.
    public int playerAVal, enemyStatsVal, fDiceTotal;

    // Texts to display values or statements.
    public Text playerATxt,enemyATxt,winLoseTxt, enemyRollText;

    void Start()
    {
        // Getting the DiceRoll script component by finding the "Dice" gameobject.
        diceRollObj = GameObject.Find("Dice").GetComponent<DiceRoll>();
        // Getting the CoinManager script component by finding the "Coins" gameobject.
        coinObj = GameObject.Find("Coins").GetComponent<CoinManager>();
		// Getting the TileEventManager script attached to the Player game object.
		tileManObj = GameObject.FindGameObjectWithTag("Player").GetComponent<TileEventManager>();

        choiceSelected = false;
        isFightRoll = false;

        // Setting the panel to a deactive state.
        fPanel.SetActive(false);
        rPanel.SetActive(false);

        enemyRollText.enabled = false;
        enemyRolling = false;

        winLoseTxt.text = "";
    }

    void Update()
    {
        // if the player has selected a choice then do the following.
        if (choiceSelected)
        {
            // Set the choice panel to deactive state.
            cPanel.SetActive(false);

            // TODO: To retrieve stats of the player and then change the value of the playerAVal.
            // Randomly generate values for the player and enemy.
            playerAVal = Random.Range(55, 80);
            enemyStatsVal = Random.Range(45, 80);

            // Display the values for the player and enemy respectively.
            playerATxt.text = playerAVal.ToString();
            enemyATxt.text = enemyStatsVal.ToString();

            // Setting the fight panel to an active state.
            fPanel.SetActive(true);

            // Resetting the text.
            winLoseTxt.text = "";
            // Resetting the dice values.
            diceRollObj.ResetFightDice();
            // Setting the FightRollPanel to active state.
            rPanel.SetActive(true);

            choiceSelected = false;
        }
    
    }

    // Coroutine called from the TileEventManager script.
    public IEnumerator CardPanel()
    {
        yield return null;
        // If the value of showEnemy is true then do the following.
        if(tileManObj.showEnemy)
        {
            // Set the boolean value to false
			tileManObj.showEnemy = false;
			
            // Activate the choice panel.
			cPanel.SetActive(true);
        }
    }

    // For when the player selects the option between magic/attack.
    public void ChooseButton()
    {
        choiceSelected = true;
    }

    // To roll the mini-dice for the fight.
    public void FightRollButton()
    {
        isFightRoll = true;
        // Calling the FightRollDice method from the DiceRoll script.
        diceRollObj.FightRollDice();

        // Getting and setting the dice total value.
        fDiceTotal = diceRollObj.fightDiceTotal;
        
        StartCoroutine(CloseRollButton());
    }

    // Coroutine to close the FightRollButton.
    IEnumerator CloseRollButton()
    {
        while(isFightRoll)
        {
            // Update and display the attack value of the player.
            playerAVal = playerAVal + fDiceTotal;
            playerATxt.text = playerAVal.ToString();
            yield return new WaitForSeconds(1);

            // Deactivate the FightRollPanel.
            rPanel.SetActive(false);
            isFightRoll = false;
            enemyRolling = true;
            StartCoroutine(EnemyRoll());
        }   
    }

    IEnumerator EnemyRoll()
    {
        while(enemyRolling)
        {
            enemyRollText.enabled = true;
            // For the enemy dice value roll
            int first = Random.Range(1, 7);
            int second = Random.Range(1, 7);
            yield return new WaitForSeconds(2);
            enemyRollText.text = "Enemy has rolled";
            enemyRolling = false;

            // Updating and displaying the stats of the enemy.
            enemyStatsVal = enemyStatsVal + first + second;
            enemyATxt.text = enemyStatsVal.ToString();
            enemyRollText.enabled = false;
            FinalDecide();
        }
    }

    // Displaying the win/lose text and updating the coin values.
    void FinalDecide()
    {
            if (playerAVal > enemyStatsVal)
            {
                winLoseTxt.color = Color.yellow;
                winLoseTxt.fontSize = 25;
                winLoseTxt.text = "Enemy defeated. You got coins.";
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().wonFight = true;

                coinObj.coinValue += 25;

                StartCoroutine(ClosePanel());
            }
            else
            {
                winLoseTxt.color = Color.red;
                winLoseTxt.text = "You got defeated.The enemy took your gold.";
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().lostFight = true;

                coinObj.coinValue -= 35;

                StartCoroutine(ClosePanel());
            }
    }

	IEnumerator ClosePanel()
	{
		yield return new WaitForSeconds(2);
        //Deactivate the fight panel.
		fPanel.SetActive(false);
       
        // Destroy the current active enemy card in the scene.
        Destroy(tileManObj.go.gameObject);

        //Setting the canRoll value to true.
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().canRoll = true;
	}
}
