using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

    public GameObject startTile;
    int diceNumber;
    public Text diceText;
    public float step;
    public GameObject optionTilePanel;
    public bool stepIsZero, leftRotate, rightRotate, upRotate, diceRolled;
    Rigidbody myRB;
    Vector3 myTransform; 

    private void Awake()
    {
        myRB = GetComponent<Rigidbody>();
    }
    // Use this for initialization
    void Start ()
    {
        optionTilePanel.SetActive(false);

        leftRotate = false;
        rightRotate = false;
        upRotate = false;
        diceRolled = false;

        this.transform.position = Vector3.Lerp(transform.position, Vector3.zero, 1f);
        Debug.Log("lerping... but not really");

    }        
        
        // Update is called once per frame
        void Update ()
    {
        if (step == 0 && diceRolled)
        {
            ResetValues();
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            diceNumber = Random.Range(1, 7);
            diceText.text = "" + diceNumber;

            step = float.Parse(diceText.text);
            Debug.Log("step: " + step);

            diceRolled = true;

            //Delay before we move to our first tile.

            //MoveForward(float.Parse(diceText.text));
            MoveForward();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "startTile" && diceRolled)
        {
            Debug.Log("collided with start tile");
            MoveForward();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (diceRolled)
        {
            if (step > 0)
            {
                if (other.tag == "upTile")
                {
                    MoveForward();
                    Debug.Log("moving forwarddddd");
                }

                else if (other.tag == "leftTile")
                {
                    leftRotate = false;
                    MoveLeft();
                    Debug.Log("moving left");
                }

                else if (other.tag == "rightTile")
                {

                    MoveRight();
                    Debug.Log("moving right");
                }

                else if (other.tag == "optionTile")
                {
                    optionTilePanel.SetActive(true);
                    Debug.Log("collided with option tile");
                }
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        optionTilePanel.SetActive(false);
    }

    //public void CloseOptionPanel()
    //{
    //    optionTilePanel.SetActive(false);
    //}

    public void RollDice() 
    {
      
            diceNumber = Random.Range(1, 7);
            diceText.text = "" + diceNumber;
       
        step = float.Parse(diceText.text);
        Debug.Log("step: " + step);

        diceRolled = true;

        //Delay before we move to our first tile.

        //MoveForward(float.Parse(diceText.text));
        MoveForward();



    }

    public void MoveForward()
    {
        //Debug.Log("player moves: " + times);

        //for (int i = 1; i <= times; i++)
        //{
        //if (!upRotate)
        //{
        //    this.transform.rotation = Quaternion.Euler(0, 0f, 0);
        //    upRotate = true;
        //    //MoveForward();

        //}

        this.transform.position += transform.forward; // new Vector3(0, 0, 1f);
            step--;

        //myRB.AddForce(force);

        Debug.Log("MoveForward().step: " + step);
        //}
    }

    public void MoveBackward()
    {
        //Debug.Log("player moves: " + times);

        //for (int i = 1; i <= times; i++)
        //{
            this.transform.position -= new Vector3(0, 0, 1f);
        step--;
        //}
    }

    public void MoveRight()
    {
        if (!rightRotate)
        {
            myTransform = this.transform.eulerAngles;
            Debug.Log("Before: " + myTransform.y);
            //myTransform.y = 0f;
            myTransform.y += 90f;
            Debug.Log("After: " + myTransform.y);


            this.transform.rotation = Quaternion.Euler(0, myTransform.y, 0);
            rightRotate = true;
        //MoveForward();

          }
        this.transform.position += transform.forward; //new Vector3(1f, 0, 0);
        step--;
        Debug.Log("MoveRight().step: " + step);
    }

    public void MoveLeft()
    {
        if (!leftRotate)
        {
            myTransform = this.transform.eulerAngles;
            Debug.Log("Before: " + myTransform.y);
            //myTransform.y = 0f;
            myTransform.y -= 90f;
            Debug.Log("After: " + myTransform.y);


            this.transform.rotation = Quaternion.Euler(0, myTransform.y, 0);
            leftRotate = true;
            //MoveForward();
        }
        this.transform.position += transform.forward; //-= new Vector3 (1f, 0, 0);

        step--;


        Debug.Log("MoveLeft().step: " + step);


        //}
    }


    public void ResetValues()
    {
        Debug.Log("resetting the values");
        stepIsZero = true;
        leftRotate = false;
        rightRotate = false;
       // upRotate = false;
        diceRolled = false;
    }

}
