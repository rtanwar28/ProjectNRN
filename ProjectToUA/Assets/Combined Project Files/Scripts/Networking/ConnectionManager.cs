using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;


namespace NRN.Tales
{
	//Connection Manager
	//Handles the Photon Connection, Creates the Player in the Room
	//Handles quitting

	public class ConnectionManager : Photon.MonoBehaviour 
	{
		#region Publics Variables
		//Sets up an static instance of ConnectionManager
		static public ConnectionManager Instance;

        public int numOfPlayers;
        public GameObject TurnManager;
        public GameObject playerObjectK;
        public GameObject playerObjectA;
        public GameObject playerObjectW;
        public GameObject playerObjectE;
        public GameObject[] players;
        public int[] playerOrder;
        public int[] playerIDs;
        public int[] initialRolls;
        public int[] playerScores;
        public Vector3 playerPosition;
        public Quaternion playerRotation;

        #endregion

        #region Private Variables

        private GameObject instance;
        int Pcount = 0;
        int temp;
        int tempPlayer;
        GameObject tempPlayerO;
        bool orderDecided = false;
        bool gameStart = false;
        GameObject Timer;

		#endregion

		// Mono Method called for GameObject by unity for initialisaation
		void Start () 
		{
			Instance = this;

			if(!PhotonNetwork.connected)
			{
				SceneManager.LoadScene ("Level1");
				return;
			}
            Timer = GameObject.FindGameObjectWithTag("Timer");

            if (playerObjectK == null && playerObjectA == null && playerObjectW == null && playerObjectE == null)
            {
				Debug.LogError ("<Color=Red><b>Missing</b></Color> playerObject is Missing. Please assign it in 'ConnectionManager' player object.", this);
			} 
			else 
			{
				if (PlayerMovement.LocalPlayerInstance == null) 
				{
					Debug.Log("We are Instantiating LocalPlayer from " + SceneManagerHelper.ActiveSceneName + " " + Pcount);
                    //playerPosition = GameObject.Find("Map Generator").GetComponent<FileIO>().GetPlayerSPostion();
                    playerPosition = new Vector3 (10.0f, 1.25f, 0.0f);
                    //PhotonNetwork.Instantiate (this.playerObject.name, playerPosition, Quaternion.identity, 0);

                    if (PhotonNetwork.player.NickName.Contains("Knight"))
                    {
                        PhotonNetwork.Instantiate(this.playerObjectK.name, playerPosition, Quaternion.identity, 0);
                    }
                    else if (PhotonNetwork.player.NickName.Contains("Assassin"))
                    {
                        PhotonNetwork.Instantiate(this.playerObjectA.name, playerPosition, Quaternion.identity, 0);
                    }
                    else if (PhotonNetwork.player.NickName.Contains("Wizard"))
                    {
                        PhotonNetwork.Instantiate(this.playerObjectW.name, playerPosition, Quaternion.identity, 0);
                    }
                    else if (PhotonNetwork.player.NickName.Contains("Elf"))
                    {
                        PhotonNetwork.Instantiate(this.playerObjectE.name, playerPosition, Quaternion.identity, 0);
                    }
                    //Pcount++;
                } 
				else 
				{
					Debug.Log ("Ignoring Scene Load" + SceneManagerHelper.ActiveSceneName);
				}
			}

            Invoke("GetPlayersAndIDs", 1.0f);
            Invoke("DecidePlayerOrder", 2.0f);
            gameStart = true;
        }

        void GetPlayersAndIDs()
        {
            //Get Each of the Players
            players = GameObject.FindGameObjectsWithTag("Player");
            numOfPlayers = players.Length;
            playerIDs = new int[numOfPlayers];
            initialRolls = new int[numOfPlayers];
            playerScores = new int[numOfPlayers];
            for (int i = 0; i < players.Length; i++)
            {
                playerIDs[i] = players[i].GetComponent<PlayerMovement>().playerID;
            }
        }

        void DecidePlayerOrder()
        {
            //Compare their die score
            //order from highest to lowest for TurnHandler
            playerOrder = OrderRollValues();

            for (int i = 0; i < playerOrder.Length; i++)
            {
                Debug.Log(playerOrder[i]);
            }
            if (orderDecided == true)
            {
                Invoke("ActivateTurnManager", 3.0f);
            }
        }

        void ActivateTurnManager()
        {
            TurnManager.SetActive(true);
        }

        int[] OrderRollValues()
        {
            //for (int i = 0; i < initialRolls.Length; i++)
            //{
            //    for (int j = i + 1; j < initialRolls.Length; j++)
            //    {
            //        if (initialRolls[i] < initialRolls[j])
            //        {
            //            Debug.Log("Player position: " + i + " PlayerID: " + playerIDs[i] + " Their Roll: " + initialRolls[i]);
            //            temp = initialRolls[j];
            //            tempPlayer = playerIDs[j];
            //            initialRolls[j] = initialRolls[i];
            //            playerIDs[j] = playerIDs[i];
            //            initialRolls[i] = temp;
            //            playerIDs[i] = tempPlayer;
            //            Debug.Log("Player position: " + i + " PlayerID: " + playerIDs[i] + " Their Roll: " + initialRolls[i]);
            //        }
            //    }
            //}

            orderDecided = true;

            return playerIDs;
        }

        public int[] GetPlayerOrder()
        {
            return playerOrder;
        }

        void OrderScores ()
        {
            for (int i = 0; i < playerScores.Length; i++)
            {
                for (int j = i + 1; j < playerScores.Length; j++)
                {
                    if (playerScores[i] < playerScores[j])
                    {
                        Debug.Log("Player position: " + i + " PlayerID: " + players[i] + " Their Roll: " + playerScores[i]);
                        temp = playerScores[j];
                        tempPlayerO = players[j];
                        playerScores[j] = playerScores[i];
                        players[j] = players[i];
                        playerScores[i] = temp;
                        players[i] = tempPlayerO;
                        Debug.Log("Player position: " + i + " PlayerID: " + players[i] + " Their Roll: " + playerScores[i]);
                    }
                }
            }
        }

        // Update is called once per frame
        void Update () 
		{
            if (gameStart == true)
            {
                if (Timer.activeSelf == true)
                {
                    if (Timer.GetComponent<Text>().text == "0 : 00")
                    {
                        for (int i = 0; i < players.Length; i++)
                        {
                            players[i].GetComponent<PlayerMovement>().ApplyCoinsValue();
                            playerScores[i] = players[i].GetComponent<PlayerMovement>().GetScore();
                            players[i].GetComponent<PlayerMovement>().reachedEnd = true;
                            TurnManager.SetActive(false);
                            OrderScores();
                            players[i].GetComponent<PlayerMovement>().leaderboard.SetActive(true);
                            players[i].GetComponent<PlayerMovement>().playerLis.GetComponent<Text>().text += players[i].GetComponent<PlayerMovement>().username + System.Environment.NewLine;
                            players[i].GetComponent<PlayerMovement>().scoreList.GetComponent<Text>().text += "" + playerScores[i] + System.Environment.NewLine;
                            players[i].GetComponent<PlayerMovement>().Winner.GetComponent<Text>().text = players[i].GetComponent<PlayerMovement>().username + " is the winner!";
                        }
                        players[0].GetComponent<PlayerMovement>().Winner.GetComponent<Text>().text = players[0].GetComponent<PlayerMovement>().username + " is the winner!";
                        gameStart = false;
                    }
                }
            }

			if(Input.GetKeyDown(KeyCode.Escape))
			{
				QuitApplication ();
			}
		}



        public void OnPhotonPlayerConnected(PhotonPlayer other)
        {

            //	Debug.Log ("OnPhotonPlayerConnected() " + other.NickName);

            //          //&& PhotonNetwork.playerList.Length == 4
            //          if (PhotonNetwork.isMasterClient) 
            //	{
            //		Debug.Log ("OnPhotonPlayerConnected isMasterClient" + PhotonNetwork.isMasterClient);
            //		LoadArena ();
            //	}
        }

        public virtual void OnLeftRoom()
		{
			SceneManager.LoadScene ("LogInRegister_UI");
		}

		public void LeaveRoom()
		{
			PhotonNetwork.LeaveRoom ();
		}

		public void QuitApplication()
		{
			Application.Quit ();
		}

		void LoadArena()
		{
			if(!PhotonNetwork.isMasterClient)
			{
				Debug.LogError ("Won't load because we aren't the master");
			}

			Debug.Log ("PhotonNetwork: Loading Da Level");

			PhotonNetwork.LoadLevel("Level1");
		}
	}
}
