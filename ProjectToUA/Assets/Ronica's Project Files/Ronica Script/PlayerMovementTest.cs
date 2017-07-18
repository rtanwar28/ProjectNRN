using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementTest : MonoBehaviour 
{
	float steps, randomNumber, oldX, newX;
	bool diceRolled;

	// Use this for initialization
	void Start () 
	{
		diceRolled = false;
		steps = 0;
		oldX = transform.position.x;
		Debug.Log ("Old X: " + oldX);
		var t = transform.position;
		t.x= oldX;
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			randomNumber = Random.Range (1, 3);
			steps = randomNumber;
			Debug.Log ("Player moves: " + steps);
			diceRolled = true;
		}

		//if(diceRolled)
		if(Input.GetKeyDown(KeyCode.Space))
			
		{
			newX = oldX - 2f;
			Debug.Log("Old X:"+oldX+" newX: "+newX);
			float temp = newX;

			transform.position = Vector3.Lerp (new Vector3 (oldX, 1.75f, 10f), new Vector3 (newX, 1.75f, 10f), 0.05f * Time.deltaTime);
			oldX = temp;

			//steps--;
		}

		/*	if(steps == 0)
		{
			Debug.Log ("No more steps");
			diceRolled = false;
		}*/
	}

	/*public void RollDice() 
	{

		randomNumber = Random.Range(1, 7);

		steps = randomNumber;
		Debug.Log("step: " + steps);

		diceRolled = true;
	}*/
}


