using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTileManager : MonoBehaviour
{
    // Variables for player position and rotation
    Vector3 playerPos, playerRot;

    GameObject playerGO;

    void Start()
    {
        //// Getting the Player Gameobject, player position and player rotation
        //playerGO = GameObject.FindWithTag("Player");
        //playerPos = GameObject.FindWithTag("Player").transform.position;
        //playerRot = GameObject.FindWithTag("Player").transform.eulerAngles;
    }

    void Update()
    {
        //// Updating the values every frame
        //playerPos = playerGO.transform.position;
        //playerRot = playerGO.transform.eulerAngles;
    }

    public void GetTileInfront(GameObject newCard)
    {
       // // Setting the destination by further 2 units of the player
       // Vector3 playerDest = playerPos + playerGO.transform.forward * 2f;

       // // Setting the card destination value
       // Vector3 cardDestination = new Vector3(playerDest.x, playerDest.y + 0.6f, playerDest.z);
        
       // // Instantiating the card game object.
       // GameObject go= (GameObject) Instantiate(newCard, cardDestination, Quaternion.identity);

       // // Setting the rotation of the card as per the rotation of the player.
       //go.transform.eulerAngles = playerRot;

       // //StartCoroutine(rotatePlayer());
   
    }

   /* IEnumerator rotatePlayer()
    {
        //if (photonView.isMine)
       // {
            float time = 0.7f;
            float i = 0f;
            float rate = 1f / time;
            while (i < 1)
            {
                i += Time.deltaTime * rate;
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0f, 0f, this.transform.eulerAngles.z + 90f), i);
                yield return null;
            }
       // }
    }*/
}
