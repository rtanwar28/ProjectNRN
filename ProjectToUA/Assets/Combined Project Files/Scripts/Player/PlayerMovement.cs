using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : Photon.PunBehaviour
{

	FileIO fileIOObj;
	DiceRoll diceObj;

	Vector3 playerPos, playerDest, tileDestination;

    public bool canMove, leftRot, rightRot, isTimerActive;
	public bool canRoll;

	public int extraMoveCount;

	//public Stack<Transform> playerMoveHistory;

	ChanceTileManager ctmObj;
    EnemyTileManager etmObj;

	public bool extraM;
    public static GameObject LocalPlayerInstance;
    public GameObject mainCamera;

    Text timerTxt;

    public void Awake()
    {
        if (photonView.isMine)
        {
            LocalPlayerInstance = gameObject;
            mainCamera = GameObject.FindGameObjectWithTag("playerCam");
            mainCamera.transform.parent = this.gameObject.transform;
        }

        DontDestroyOnLoad(gameObject);

        timerTxt = GameObject.Find("Timer").GetComponent<Text>();

        timerTxt.gameObject.SetActive((false));

        isTimerActive = false;
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
            //playerMoveHistory = new Stack<Transform>();
        }
	}

	void Update()
	{
		if (photonView.isMine)
		{

			if (canRoll && Input.GetKeyDown(KeyCode.Space))
			{
				canRoll = false;
				diceObj.RollDice();
			}

			if (Input.GetKeyDown(KeyCode.UpArrow) && (diceObj.diceTotal != 0 || extraMoveCount != 0 || extraM))
			{
				playerPos = this.transform.position;
				playerDest = playerPos + transform.forward * 2f;
				tileDestination = new Vector3(playerDest.x, playerDest.y - 1.25f, playerDest.z);
				PlayerMove(tileDestination);
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow) && (diceObj.diceTotal != 0 && !rightRot || extraMoveCount != 0 || extraM))
			{
				StartCoroutine(rotatePlayer(this.transform.rotation, Quaternion.Euler(0f, this.transform.eulerAngles.y + 90f, 0f)));
    
                if (!leftRot)
                    rightRot = true;
                 leftRot = false;
			}
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && (diceObj.diceTotal != 0 && !leftRot || extraMoveCount != 0 || extraM))
			{
                StartCoroutine(rotatePlayer(this.transform.rotation, Quaternion.Euler(0f, this.transform.eulerAngles.y - 90f, 0f)));

                if (!rightRot)
                    leftRot = true;
                rightRot = false;
			}
		}
	}

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

	IEnumerator rotatePlayer (Quaternion startRot, Quaternion newRot)
	{
        if (photonView.isMine)
        {
            float time = 0.7f;
            float i = 0f;
            float rate = 1f / time;
            while (i < 1)
            {
                i += Time.deltaTime * rate;
                this.transform.rotation = Quaternion.Lerp(startRot, newRot, i);
                yield return null;
            }
        }
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
                            }
                            else
                            {
                                diceObj.diceRolledA--;
                                diceObj.DisplayDice(diceObj.diceRolledA, diceObj.diceRolledB);
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
            }
        }
	}

    void CalledOnLevelLoaded(int level)
    {

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            Vector3 pos = transform.position;
            stream.Serialize(ref pos);
        }
        else
        {
            Vector3 pos = Vector3.zero;
            stream.Serialize(ref pos);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FinishPoint" && !isTimerActive)
        {
			timerTxt.gameObject.SetActive((true));

			isTimerActive = true;
        }
    }

}

/* TO DO:
 * Make sure that players cannot go in reverse direction after new roll of dice.
 * Make sure playes cannot visit tiles parallel to paths already traversed.
*/
