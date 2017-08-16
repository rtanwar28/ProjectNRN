using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTileManager : Photon.MonoBehaviour
{
    TileEventManager tileManObj;

    public GameObject ePanel;
    public GameObject fPanel;
    public GameObject rPanel;
    public Canvas canvasGO;
    GameObject enPanel;

    DiceRoll diceRollObj;
    CoinManager coinObj;
    PlayerMovement moveObj;

    bool epVisible;

    public bool choiceSelected, isFightRoll, enemyRolling, closePanel;



    public int playerAVal, enemyStatsVal, fDiceTotal;

    public Text playerATxt,enemyATxt,winLoseTxt, enemyRollText;

    void Start()
    {
        diceRollObj = GameObject.Find("Dice").GetComponent<DiceRoll>();
        coinObj = GameObject.Find("Coins").GetComponent<CoinManager>();

        epVisible = false;

        choiceSelected = false;
        isFightRoll = false;

        fPanel.SetActive(false);
        rPanel.SetActive(false);

        // TODO: To retrieve stats of the player and then change the value of the playerAVal.
        playerAVal = Random.Range(55,80);
        enemyStatsVal = Random.Range(45,80);

        playerATxt.text = playerAVal.ToString();
        enemyATxt.text = enemyStatsVal.ToString();

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
        //if(!epVisible)
        //{
			enemyPanel.SetActive(true);

        //    epVisible = true;
        //}

        //else if (epVisible)
        //{
        //    enemyPanel.SetActive(false);
        //    epVisible = false;
        //}
       
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
            enemyStatsVal = enemyStatsVal + first + second;
            enemyATxt.text = enemyStatsVal.ToString();
            enemyRollText.enabled = false;
            FinalDecide();
        }
    }

    void FinalDecide()
    {
 
            if (playerAVal > enemyStatsVal)
            {
                winLoseTxt.color = Color.yellow;
                winLoseTxt.fontSize = 25;
                winLoseTxt.text = "Enemy defeated. You got coins.";

                coinObj.coinValue += 25;

            fPanel.SetActive(false);

          
                // canvasGO.enabled = false;
                // enemyPanel.SetActive(false);
                //this.gameObject.SetActive(false);
                // moveObj.canRoll = true;
            }
            else
            {
                winLoseTxt.color = Color.red;
                winLoseTxt.text = "You got defeated.The enemy took your gold.";

                coinObj.coinValue -= 35;
            fPanel.SetActive(false);
          

                //moveObj.canRoll = true;
            }

        if (photonView.isMine)
        {
            tileManObj = GameObject.FindGameObjectWithTag("Player").GetComponent<TileEventManager>();

            tileManObj.fightTriggered = false;
            Debug.Log("The fight triggered");
            moveObj = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

            moveObj.canRoll = true;

            //Destroy(tileManObj.card);
            DestroyImmediate(tileManObj.card, true);

        }
        
    }
}
