using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//-0.2 -> 1.8 (y axis)

public class TileEventManager : Photon.PunBehaviour
{
	GameObject storePanel, chancePanel, enemyPanel;
	public GameObject tempPanel;

    public GameObject[] enemyCardPrefabs = new GameObject[4];
    EnemyTileManager enemyTileManager;

    public GameObject card;

	//PlayerMovement playerMovementObj;
	DiceRoll rollObj;

	public bool chanceTriggered;

    // Variables for player position and rotation
    Vector3 playerPos, playerRot;
    GameObject playerGO;

    // Use this for initialization
    void Start () 
	{
        enemyTileManager = GameObject.FindWithTag("enemy").GetComponent<EnemyTileManager>();

        // Getting the Player Gameobject, player position and player rotation
        playerGO = GameObject.FindWithTag("Player");
        playerPos = GameObject.FindWithTag("Player").transform.position;
        playerRot = GameObject.FindWithTag("Player").transform.eulerAngles;

        if (photonView.isMine)
        {
            //playerMovementObj = this.GetComponent<PlayerMovement>();
            rollObj = GameObject.Find("Dice").GetComponent<DiceRoll>();

            // Getting and setting the panels in a false state
            storePanel = GameObject.FindGameObjectWithTag("store");
            storePanel.SetActive(false);

            chancePanel = GameObject.FindGameObjectWithTag("chance");
            chancePanel.SetActive(false);

            enemyPanel = GameObject.FindGameObjectWithTag("enemy");
            enemyPanel.SetActive(false);

            chanceTriggered = false;

        }
	}

	// Update is called once per frame
	void Update () 
	{
        // Updating the values every frame
        playerPos = playerGO.transform.position;
        playerRot = playerGO.transform.eulerAngles;
    }


	void OnTriggerEnter(Collider other)
	{
        //// Checking if the player triggered the enemy event
        //if (other.tag == "Fight") //&& rollObj.diceTotal == 0)
        //{
        //    Debug.Log("enemy called");

        //    //this.GetComponent<PlayerMovement>().canRoll = false;

        //    other.gameObject.GetComponent<EnemyTileManager>().GetTileInfront(card);

        //} 

            if (photonView.isMine)
        {

            if (other.tag == "Shop" && rollObj.diceTotal == 0)
            {

                Debug.Log("Store called");
                tempPanel = storePanel;
                tempPanel.SetActive(true);
            }

            // Checking if the player has triggered the chance event
            if (other.tag == "QMark" && rollObj.diceTotal == 0 && this.GetComponent<PlayerMovement>().extraMoveCount == 0 && !chanceTriggered)
            {
                chanceTriggered = true;
                this.GetComponent<PlayerMovement>().canRoll = false;
                Debug.Log("chance called");
                tempPanel = chancePanel;
                tempPanel.SetActive(true);
            }

            // Checking if the player triggered the enemy event
            if (other.tag == "Fight" && rollObj.diceTotal == 0)
            {
                Debug.Log("enemy called");

                this.GetComponent<PlayerMovement>().canRoll = false;
                card = enemyCardPrefabs[Random.Range(0, 4)];
                GetTileInfront(card);

                tempPanel = enemyPanel;
               // tempPanel.SetActive(true);
                // TODO!
                //RAISE THE ENEMY CARD ON Y AXIS TO 0.87f
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
        GameObject go = (GameObject)Instantiate(newCard, cardDestination, Quaternion.identity);
        

        if(playerRot.x == 90f)
        {
            go.transform.eulerAngles = new Vector3(0f, playerRot.y, playerRot.z);
        }
        else if(playerRot.y == 90f)
        {
            go.transform.eulerAngles = new Vector3(playerRot.x, 0f, playerRot.z);
        }
        else if (playerRot.z == 90f)
        {
            go.transform.eulerAngles = new Vector3(playerRot.x, playerRot.y, 0f);
        }
        else
        {
            go.transform.eulerAngles = new Vector3(90f, playerRot.y, playerRot.z);
        }

        // Setting the rotation of the card as per the rotation of the player.

        // Debug.Log("newcard rot part 2: " + go.transform.eulerAngles);

         StartCoroutine(RotateCard(go));

        // StartCoroutine(CardPanel());

    }

    

    IEnumerator RotateCard(GameObject cardGO)
    {
        Debug.Log("x Rotate: " + cardGO.transform.eulerAngles.x);
        if (photonView.isMine)
        {
        float time = 0.7f;
        float i = 0f;
        float rate = 1f / time;
        while (i < 1)
        {
            i = Time.deltaTime * rate;
            cardGO.transform.rotation = Quaternion.Lerp(Quaternion.Euler(cardGO.transform.eulerAngles), Quaternion.Euler(0f, cardGO.transform.eulerAngles.y, cardGO.transform.eulerAngles.z), i);

                yield return null;
                StartCoroutine(enemyTileManager.CardPanel(tempPanel));
            }
        }

        this.gameObject.GetComponent<PlayerMovement>().canRoll = true;
    }

}
