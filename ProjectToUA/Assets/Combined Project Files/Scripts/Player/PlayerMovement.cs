using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : Photon.PunBehaviour
{

    //To Be Moved to Player Details/Network
    public int playerID;
    public int initialRoll;
    public GameObject GameM;
    public GameObject leaderboard, playerLis, scoreList, Winner;
    int[] diceSValue;
    public bool playerTurn = false;
    public bool reachedEnd = false;
    public bool wonFight = false;
    public bool lostFight = false;
    public string username;
    public Sprite IDOne, IDTwo, IDThree, IDFour;

    FileIO fileIOObj;
	DiceRoll diceObj;

	Vector3 playerPos, playerDest, tileDestination;

    public bool canMove, leftRot, rightRot, isTimerActive, isRotating;
	public bool canRoll;

    public int extraMoveCount;

	//public Stack<Transform> playerMoveHistory;

	ChanceTileManager ctmObj;
    EnemyTileManager etmObj;

	public bool extraM;
    public static GameObject LocalPlayerInstance;
    public GameObject mainCamera;

    GameObject timerTxt, usernameDisplay, IDDisplay, scoreText, ScoreManager, coinValue;
    public int score;

    public void Awake()
    {
        GameM = GameObject.FindGameObjectWithTag("GameManager");
        timerTxt = GameObject.FindGameObjectWithTag("Timer");
        usernameDisplay = GameObject.FindGameObjectWithTag("user");
        IDDisplay = GameObject.FindGameObjectWithTag("ID");
        ScoreManager = GameObject.FindGameObjectWithTag("ScoreManager");
        scoreText = GameObject.FindGameObjectWithTag("Score");
        coinValue = GameObject.FindGameObjectWithTag("Coin");
        leaderboard = GameObject.FindGameObjectWithTag("Leaderboard");
        playerLis = GameObject.FindGameObjectWithTag("PlayList");
        scoreList = GameObject.FindGameObjectWithTag("ScoreList");
        Winner = GameObject.FindGameObjectWithTag("WinnerAnnouncement");

        if (photonView.isMine)
        {
            LocalPlayerInstance = gameObject;
            mainCamera = GameObject.FindGameObjectWithTag("playerCam");
            mainCamera.transform.parent = this.gameObject.transform;
            timerTxt.SetActive((false));
            leaderboard.SetActive(false);
            isTimerActive = false;

            playerID = PhotonNetwork.player.ID;
            username = PhotonNetwork.player.NickName;

            usernameDisplay.GetComponent<Text>().text = username;
            if(playerID == 1)
            {
                IDDisplay.GetComponent<Image>().sprite = IDOne;
            }
            else if (playerID == 2)
            {
                IDDisplay.GetComponent<Image>().sprite = IDTwo;
            }
            else if (playerID == 3)
            {
                IDDisplay.GetComponent<Image>().sprite = IDThree;
            }
            else if (playerID == 4)
            {
                IDDisplay.GetComponent<Image>().sprite = IDFour;
            }
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (photonView.isMine)
        {

            extraM = false;

            canRoll = true;

            fileIOObj = GameObject.Find("Map Generator").GetComponent<FileIO>();
            diceObj = GameObject.Find("Dice").GetComponent<DiceRoll>();
            etmObj = GameObject.FindWithTag("enemy").GetComponent<EnemyTileManager>();

            extraMoveCount = 0;

            canMove = false;
            leftRot = false;
            rightRot = false;

            isRotating = false;
         
            //Invoke("InitialRoll", 3.0f);
            //playerMoveHistory = new Stack<Transform>();
        }

           
    }

    protected void Update()

    {
        if (photonView.isMine)
        {
            if (reachedEnd == false)
            {
                if (playerTurn == true)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                        RollDice();

                    if (Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetKeyDown(KeyCode.W)))
                        MoveForward();

                    else if ((Input.GetKeyDown(KeyCode.RightArrow) || (Input.GetKeyDown(KeyCode.D))) && !isRotating)
                        MoveRight();

                    else if ((Input.GetKeyDown(KeyCode.LeftArrow) || (Input.GetKeyDown(KeyCode.A))) && !isRotating)
                        MoveLeft();
                }
            }

            if (wonFight == true)
            {
                score = ScoreManager.GetComponent<ScoreManager>().AffectScore("Won Fight", score);
                wonFight = false;
            }
            else if (lostFight == true)
            {
                score = ScoreManager.GetComponent<ScoreManager>().AffectScore("Lost Fight", score);
                lostFight = false;
            }
            scoreText.GetComponent<Text>().text = "" + score;
            
            if(isTimerActive == true)
            {
                timerTxt.gameObject.SetActive((true));
            }
        }
    }

    public void RollDice()
    {
        if (photonView.isMine)
        {
            Debug.Log("In Roll dice");
            if (canRoll)
            {
                Debug.Log("canRoll value: " + canRoll);

                diceObj.RollDice(); Debug.Log("diceObj.RollDice() is called");
                canRoll = false; Debug.Log("canRoll value is now: " + canRoll);

            }
            else if (!canRoll)
            {
                Debug.Log("not able to roll dice...");
            }
        }
    }

    public void MoveForward()
    {
        if (photonView.isMine)
        {
            if ((diceObj.diceTotal != 0 || extraMoveCount != 0 || extraM))
            {
                Debug.Log("player moving forward");
                playerPos = this.transform.position;
                playerDest = playerPos + transform.forward * 2f;
                tileDestination = new Vector3(playerDest.x, playerDest.y - 1.25f, playerDest.z);
                PlayerMove(tileDestination);
            }
        }
    }


    public void MoveRight()
    {
        //if (photonView.isMine)
        //{
            if (diceObj.diceTotal != 0 && !rightRot || extraMoveCount != 0 || extraM)
            {
                isRotating = true;
                Debug.Log(isRotating);
                StartCoroutine(RotatePlayer(this.transform.rotation, Quaternion.Euler(0f, this.transform.eulerAngles.y + 90f, 0f)));
                if (!leftRot)
                    rightRot = true;
                leftRot = false;
                

                //if (this.transform.rotation == Quaternion.Euler(0f, this.transform.eulerAngles.y + 90f, 0f))
                //{
                //	Debug.Log(this.transform.rotation);
                //	Debug.Log(Quaternion.Euler(0f, this.transform.eulerAngles.y + 90f, 0f));
                //	isRotating = false;
                //}

            }

        //}
    }

    public void MoveLeft()
    {
        //if (photonView.isMine)
        //{
            if (diceObj.diceTotal != 0 && !leftRot || extraMoveCount != 0 || extraM)
            {
                isRotating = true;
			    Debug.Log(isRotating);
                StartCoroutine(RotatePlayer(this.transform.rotation, Quaternion.Euler(0f, this.transform.eulerAngles.y - 90f, 0f)));
                if (!rightRot)
                    leftRot = true;
                rightRot = false;

				//if (this.transform.rotation == Quaternion.Euler(0f, this.transform.eulerAngles.y - 90f, 0f))
				//{
				//	Debug.Log(this.transform.rotation);
				//	Debug.Log(Quaternion.Euler(0f, this.transform.eulerAngles.y - 90f, 0f));
				//	isRotating = false;
				//}

            }

        //}
    }

 //   void Update()
	//{
	//	if (photonView.isMine)
	//	{

	//		if (canRoll && Input.GetKeyDown(KeyCode.Space))
	//		{
	//			canRoll = false;
	//			diceObj.RollDice();
	//		}

	//		if (Input.GetKeyDown(KeyCode.UpArrow) && (diceObj.diceTotal != 0 || extraMoveCount != 0 || extraM))
	//		{
	//			playerPos = this.transform.position;
	//			playerDest = playerPos + transform.forward * 2f;
	//			tileDestination = new Vector3(playerDest.x, playerDest.y - 1.25f, playerDest.z);
	//			PlayerMove(tileDestination);
	//		}
	//		else if (Input.GetKeyDown(KeyCode.RightArrow) && (diceObj.diceTotal != 0 && !rightRot || extraMoveCount != 0 || extraM))
	//		{
	//			StartCoroutine(RotatePlayer(this.transform.rotation, Quaternion.Euler(0f, this.transform.eulerAngles.y + 90f, 0f)));
    
 //               if (!leftRot)
 //                   rightRot = true;
 //                leftRot = false;
	//		}
 //           else if (Input.GetKeyDown(KeyCode.LeftArrow) && (diceObj.diceTotal != 0 && !leftRot || extraMoveCount != 0 || extraM))
	//		{
 //               StartCoroutine(RotatePlayer(this.transform.rotation, Quaternion.Euler(0f, this.transform.eulerAngles.y - 90f, 0f)));

 //               if (!rightRot)
 //                   leftRot = true;
 //               rightRot = false;
	//		}
	//	}
	//}

	public IEnumerator FollowPlayer (Vector3 startPos, Vector3 newPos)
	{
        if (photonView.isMine)
        {
            float time = 0.5f;
            float i = 0f;
            float rate = 1f / time;
            while (i < 1)
            {
                i += Time.deltaTime * rate;
                gameObject.transform.position = Vector3.Lerp(startPos, newPos, i);
                yield return null;
            }

            if (gameObject.GetComponent<TileEventManager>().fightTriggered)
            {
                gameObject.GetComponent<TileEventManager>().fightTriggered = false;
            }
            
        }
	}

	IEnumerator RotatePlayer (Quaternion startRot, Quaternion newRot)
	{
       
        //if (photonView.isMine)
        //{
            float time = 0.7f;
            float i = 0f;
            float rate = 1f / time;
            while (i < 1)
            {
                i += Time.deltaTime * rate;
                this.transform.rotation = Quaternion.Lerp(startRot, newRot, i);
                yield return null;
            }

        isRotating = false;

        //}
	}

	void PlayerMove (Vector3 playerDestination)
    {
        if (photonView.isMine)
        {

            for (int i = 0; i < fileIOObj.prefabTransforms.Count; i++)
            {


                if (playerDestination == fileIOObj.prefabTransforms[i].position)
                {
                    canMove = true;
                }

                if (canMove)
                {
                    if (extraM)
                    {
                        ctmObj = GameObject.Find("ChancePanel").GetComponent<ChanceTileManager>();
                        extraMoveCount = ctmObj.extraMoves;
                        ctmObj.gameObject.SetActive(false);
                        extraM = false;
                    }
                    //playerMoveHistory.Push (this.transform);
                    StartCoroutine(FollowPlayer(this.transform.position, playerDest));
                    canMove = false;
                    if (extraMoveCount == 0)
                    {
                        diceObj.diceTotal--;
                        if (diceObj.diceTotal != -1)
                        {
                            if (diceObj.diceRolledB != 0)
                            {
                                diceObj.diceRolledB--;
                                diceObj.DisplayDice(diceObj.diceRolledA, diceObj.diceRolledB);
                                score = ScoreManager.GetComponent<ScoreManager>().AffectScore("Tile Movement", score);
                            }
                            else
                            {
                                diceObj.diceRolledA--;
                                diceObj.DisplayDice(diceObj.diceRolledA, diceObj.diceRolledB);
                                score = ScoreManager.GetComponent<ScoreManager>().AffectScore("Tile Movement", score);
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("eMC: " + extraMoveCount);
                        ctmObj.extraMoves--;
                        extraMoveCount--;
                        if (extraMoveCount == 0)
                            canRoll = true;

                    }
                    leftRot = false;
                    rightRot = false;
                }
            }

            if (diceObj.diceTotal == 0 && extraMoveCount == 0)
            {
                canRoll = true;
                playerTurn = false;
            }
        }
	}

    public void ApplyCoinsValue()
    {
        score += coinValue.GetComponent<CoinManager>().coinValue * 2;
    }

    public int GetScore()
    {
        return score;
    }

    //public void InitialRoll()
    //{
    //    initialRoll = diceObj.StartRoll();
    //    this.photonView.RPC("Rolled", PhotonTargets.All, initialRoll, playerID);
    //    //return initialRoll;
    //}

    //[PunRPC]
    //public void Rolled(int tDiceValue, int playerID)
    //{
    //    for (int i = 0; i < GameM.GetComponent<NRN.Tales.ConnectionManager>().playerIDs.Length; i++)
    //    {
    //        if (GameM.GetComponent<NRN.Tales.ConnectionManager>().playerIDs[i] == playerID)
    //        {
    //            GameM.GetComponent<NRN.Tales.ConnectionManager>().initialRolls[i] = tDiceValue;
    //        }
    //    }
    //}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            Vector3 pos = transform.position;
            stream.Serialize(ref pos);
            stream.SendNext(playerID);
            stream.SendNext(playerTurn);
            stream.SendNext(reachedEnd);
            stream.SendNext(isTimerActive);
        }
        else
        {
            Vector3 pos = Vector3.zero;
            stream.Serialize(ref pos);
            this.playerID = (int)stream.ReceiveNext();
            this.playerTurn = (bool)stream.ReceiveNext();
            this.reachedEnd = (bool)stream.ReceiveNext();
            this.isTimerActive = (bool)stream.ReceiveNext();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FinishPoint")
        {
            if (!isTimerActive)
            {
                timerTxt.gameObject.SetActive((true));

                isTimerActive = true;
                score = ScoreManager.GetComponent<ScoreManager>().AffectScore("Reached End", score);

                diceObj.GameEnd();
            }
            reachedEnd = true;
            playerTurn = false;
        }
    }
}

/* TO DO:
 * Make sure that players cannot go in reverse direction after new roll of dice.
 * Make sure playes cannot visit tiles parallel to paths already traversed.
*/
