using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileEventManager : Photon.PunBehaviour
{
    GameObject storePanel, chancePanel;
    public GameObject enemyPanel;

    // To store the active panel temporary
	public GameObject tempPanel;

    // Stores the enemy card game object
    public GameObject go;

    // Stores the different enemy cards
    public GameObject[] enemyCardPrefabs = new GameObject[4];

    // Reference to the Enemy Tile Manager Script
    EnemyTileManager enemyTileManager;

    // Reference to the Enemy Card that pops up.
    public GameObject card;

    // Reference to the DiceRoll script.
	DiceRoll rollObj;

    // Bool value that will keep a check to activate/deactivate a panel.
    public bool showEnemy;

    // For triggering the specific events.
    public bool chanceTriggered, fightTriggered;

    // Variables for player position and rotation.
    Vector3 playerPos, playerRot;

    // Stores the player game object.
    GameObject playerGO;

    // Use this for initialization
    void Start () 
	{
        // Getting the EnemyTileManager script component from a game object with tag "enemy".
        enemyTileManager = GameObject.FindWithTag("enemy").GetComponent<EnemyTileManager>();

        // Getting the Player Gameobject, player position and player rotation
        playerGO = GameObject.FindWithTag("Player");
        playerPos = GameObject.FindWithTag("Player").transform.position;
        playerRot = GameObject.FindWithTag("Player").transform.eulerAngles;

        if (photonView.isMine)
        {
            //playerMovementObj = this.GetComponent<PlayerMovement>();
            rollObj = GameObject.Find("Dice").GetComponent<DiceRoll>();

            // Getting and setting the panels to a false state
            storePanel = GameObject.FindGameObjectWithTag("store");
            storePanel.SetActive(false);

            chancePanel = GameObject.FindGameObjectWithTag("chance");
            chancePanel.SetActive(false);

            enemyPanel = GameObject.FindGameObjectWithTag("enemy").transform.Find("EnemyChoicePanel").gameObject;
            enemyPanel.SetActive(false);

            chanceTriggered = false;
            fightTriggered = false;

        }
	}

	// Update is called once per frame
	void Update () 
	{
        // Updating the values every frame
        playerPos = playerGO.transform.position;
        playerRot = playerGO.transform.eulerAngles;
    }

    // OnTriggerEnter function to check with what game object has the player collided and set the panel active.
	void OnTriggerEnter(Collider other)
	{
        if (photonView.isMine)
        {
            if (other.tag == "Shop" && rollObj.diceTotal == 0)
            {
                tempPanel = storePanel;
                tempPanel.SetActive(true);
            }

            // Checking if the player has triggered the chance event
            if (other.tag == "QMark" && rollObj.diceTotal == 0 && this.GetComponent<PlayerMovement>().extraMoveCount == 0 && !chanceTriggered)
            {
                chanceTriggered = true;
                // Setting the ability to roll the dice to false.
                this.GetComponent<PlayerMovement>().canRoll = false;
                tempPanel = chancePanel;
                tempPanel.SetActive(true);
            }

            // Checking if the player triggered the enemy event
            if (other.tag == "Fight" && rollObj.diceTotal == 0 && !fightTriggered)
            {
                fightTriggered = true;
				// Setting the ability to roll the dice to false.
				this.GetComponent<PlayerMovement>().canRoll = false;

                // Randomly generating the card to be displayed.
                card = enemyCardPrefabs[Random.Range(0, 4)];

                GetTileInfront(card);

                tempPanel = enemyPanel;
                tempPanel.SetActive(true);

            }
        }
	}


    public void GetTileInfront(GameObject newCard)
    {
        // Setting the destination by further 2 units of the player
        Vector3 playerDest = playerPos + (playerGO.transform.forward * 2f);

        // Setting the card destination value
        Vector3 cardDestination = new Vector3(playerDest.x,0.85f, playerDest.z);

        // Instantiating the card game object.
        go = (GameObject)Instantiate(newCard, cardDestination, Quaternion.identity);

        // Setting the inital rotation value of the card
        go.transform.eulerAngles = new Vector3(90f, playerRot.y, 0f);
            
        StartCoroutine(RotateCard(go));
    }

    // This allow for the card to rotate from one angle value to another
    IEnumerator RotateCard(GameObject cardGO)
    {
        if (photonView.isMine)
        {
            // Setting the value to true in order to display the panel
            showEnemy = true;
            float time = 0.7f;
            float i = 0f;
            float rate = 1f / time;
            while (i < 1)
            {
                i = Time.deltaTime * rate;
                cardGO.transform.rotation = Quaternion.Lerp(Quaternion.Euler(cardGO.transform.eulerAngles), Quaternion.Euler(0f, cardGO.transform.eulerAngles.y, cardGO.transform.eulerAngles.z), i);

                yield return null;

                // Starting the CardPanel coroutine in the EnemyTileManager script.
                StartCoroutine(enemyTileManager.CardPanel());
            }
        }
        // Setting the ability to roll the dice to true.
        this.gameObject.GetComponent<PlayerMovement>().canRoll = true;
    }


}
