using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

	// Reference to the FileIO script
	FileIO fileIOObj;

	// Player's position
	Vector3 playerPos;
	// Player's Destination
	Vector3 playerDest;
	// Destination of the tile
	Vector3 tileDestination;

	// To rotate the player
	Vector3 upRot, downRot, leftRot, rightRot;

	int prefCount;
	int diceRolledA;
	int diceRolledB;
	public int diceTotal;

	// bool to check for player movement
	public bool canMove;
	// bool to rotate the player left/right/up/down
	bool canRoll, leftMove, rightMove, upMove, downMove;

	void Start(){
		canRoll = true;

		// Finding the map generator in the game scene
		fileIOObj = GameObject.Find("Map Generator").GetComponent<FileIO>();

		// Setting the rotation of the player as per the movement
		upRot = new Vector3 (0f, 0f, 0f);
		downRot = new Vector3 (0f, 180f, 0f);
		leftRot = new Vector3 (0f, 270, 0f);
		rightRot = new Vector3 (0f, 90f, 0f);

		canMove = false;
		upMove = false;
		downMove = false;
		leftMove = false;
		rightMove = false;
	}

	void Update(){

		// Rolling the dice and getting the value.
		if (canRoll && Input.GetKeyDown (KeyCode.Space)) 
		{
			canRoll = false;
			diceRolledA = Random.Range (1,6);
			diceRolledB = Random.Range (1, 6);
			diceTotal = diceRolledA + diceRolledB;
			Debug.Log ("Dice A: "+diceRolledA+" + Dice B: "+diceRolledB+" = Dice Total: "+diceTotal);
		}

		// If the player has selected the Up Arrow key. The total of the tile is not 0. Making sure player does not move backwards.
		if (Input.GetKeyDown (KeyCode.UpArrow) && diceTotal!=0 && !downMove)
		{
			Debug.Log ("Up pressed.");
			// Getting the position of the player  
			playerPos = this.transform.position;
			// Setting and moving the player position
			playerDest = new Vector3 (playerPos.x, playerPos.y, playerPos.z+2f);
			// The tile the player has to move towards
			tileDestination = new Vector3 (playerPos.x, playerPos.y-1.75f, playerPos.z+2f);
			// Passing value to the PlayerMove function.
			PlayerMove (tileDestination, upRot,'u');
		}
		else if (Input.GetKeyDown (KeyCode.DownArrow) && diceTotal!=0 && !upMove) 
		{
			Debug.Log ("Down pressed.");
			playerPos = this.transform.position;
			playerDest = new Vector3 (playerPos.x, playerPos.y, playerPos.z-2f);
			tileDestination = new Vector3 (playerPos.x, playerPos.y-1.75f, playerPos.z-2f);
			PlayerMove (tileDestination, downRot,'d');
		}
		else if (Input.GetKeyDown (KeyCode.RightArrow) && diceTotal!=0 && !leftMove) 
		{
			Debug.Log ("Right pressed.");
			playerPos = this.transform.position;
			playerDest = new Vector3 (playerPos.x+2f, playerPos.y, playerPos.z);
			tileDestination = new Vector3 (playerPos.x+2f, playerPos.y-1.75f, playerPos.z);
			PlayerMove (tileDestination, rightRot,'r');
		}
		else if (Input.GetKeyDown (KeyCode.LeftArrow) && diceTotal!=0 && !rightMove) 
		{
			Debug.Log ("Left pressed.");
			playerPos = this.transform.position;
			playerDest = new Vector3 (playerPos.x-2f, playerPos.y, playerPos.z);
			tileDestination = new Vector3 (playerPos.x-2f, playerPos.y-1.75f, playerPos.z);
			PlayerMove (tileDestination, leftRot,'l');
		}

	}

	void PlayerMove (Vector3 playerDestination, Vector3 playerRot,char moveDir)
	{
		for (int i = 0; i < fileIOObj.prefabTransforms.Count; i++) 
		{

			if (playerDestination == fileIOObj.prefabTransforms [i].position) 
			{
				canMove = true;
			}

			if (canMove)
			{
				this.transform.position = playerDest;
				this.transform.eulerAngles = playerRot;
				canMove = false;
				diceTotal--;

				switch (moveDir) {

				case 'u': 
					upMove = true;
					break;

				case 'd':
					downMove = true;
					break;

				case 'r':
					rightMove = true;
					break;

				case 'l':
					leftMove = true;
					break;

				default:
					break;
				}
			}
		}


		if (diceTotal == 0) {
			canRoll = true;
			switch (moveDir) {

			case 'u':
				downMove = false;
				leftMove = false;
				rightMove = false;
				break;

			case 'd':
				upMove = false;
				leftMove = false;
				rightMove = false;
				break;

			case 'r':
				upMove = false;
				downMove = false;
				leftMove = false;
				break;

			case 'l':
				upMove = false;
				downMove = false;
				rightMove = false;
				break;

			default:
				break;
			}

		}

		Debug.Log (diceTotal);
	}
}

/* TO DO:
 * Make sure that players cannot go in reverse direction after new roll of dice.
 * Make sure playes cannot visit tiles parallel to paths already traversed.
*/