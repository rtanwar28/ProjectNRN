using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTileManager : Photon.MonoBehaviour
{
    public GameObject ePanel;
    public GameObject fPanel;
    public GameObject rPanel;
    GameObject enPanel;

    DiceRoll diceRollObj;
    CoinManager coinObj;
    //PlayerMovement moveObj;

    public bool choiceSelected, isFightRoll, enemyRolling, closePanel;

    public int playerAVal, enemyAVal, fDiceTotal;

    public Text playerATxt,enemyATxt,winLoseTxt, enemyRollText;

    void Start()
    {
        diceRollObj = GameObject.Find("Dice").GetComponent<DiceRoll>();
        coinObj = GameObject.Find("Coins").GetComponent<CoinManager>();
       // moveObj = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

        choiceSelected = false;
        isFightRoll = false;

        fPanel.SetActive(false);
        rPanel.SetActive(false);

        playerAVal = 36;
        enemyAVal = 50;

        playerATxt.text = playerAVal.ToString();
        enemyATxt.text = enemyAVal.ToString();

        enemyRollText.enabled = false;
        enemyRolling = false;
        winLoseTxt.text = "";

        closePanel = false;
    }

    void Update()
    {
        if (choiceSelected)
        {
            ePanel.SetActive(false);
            fPanel.SetActive(true);
            rPanel.SetActive(true);
            choiceSelected = false;
        }
    }

    public IEnumerator CardPanel(GameObject enemyPanel)
    {
        yield return new WaitForSeconds(3);
        enPanel = enemyPanel;
        enemyPanel.SetActive(true);
    }

    public void ChooseButton()
    {
        choiceSelected = true;
        Debug.Log("The player has chosen a button");
    }

    public void FightRollButton()
    {
        isFightRoll = true;
        diceRollObj.FightRollDice();
        fDiceTotal = diceRollObj.fightDiceTotal;
        
        StartCoroutine(CloseRollButton());
    }

    IEnumerator CloseRollButton()
    {
        while(isFightRoll)
        {
            playerAVal = playerAVal + fDiceTotal;
            playerATxt.text = playerAVal.ToString();
            yield return new WaitForSeconds(1);
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
            int first = Random.Range(1, 7);
            int second = Random.Range(1, 7);
            yield return new WaitForSeconds(2);
            enemyRollText.text = "Enemy has rolled";
            enemyRolling = false;
            enemyAVal = enemyAVal + first + second;
            enemyATxt.text = enemyAVal.ToString();
            enemyRollText.enabled = false;
            FinalDecide();
        }
    }

    void FinalDecide()
    {
        if(playerAVal > enemyAVal)
        {
            winLoseTxt.color = Color.yellow;
            winLoseTxt.fontSize = 25;
            winLoseTxt.text = "Enemy defeated. You got coins.";

            coinObj.coinValue += 25;
           // enemyPanel.SetActive(false);
            //this.gameObject.SetActive(false);
           // moveObj.canRoll = true;
        }
        else
        {
            winLoseTxt.color = Color.red;
            winLoseTxt.text = "You got defeated.The enemy took your gold.";

            coinObj.coinValue -= 35;
            enPanel.SetActive(false);
            //moveObj.canRoll = true;
        }
    }
}
