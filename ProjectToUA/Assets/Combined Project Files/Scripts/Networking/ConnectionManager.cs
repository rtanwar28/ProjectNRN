using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
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
        public int[] initialRoll;
        public GameObject TurnManager;
        public GameObject playerObject;
        public GameObject[] players;
        public GameObject[] playerOrder;
        public Vector3 playerPosition;

		#endregion

		#region Private Variables

		private GameObject instance;
        int Pcount = 0;
        int temp;
        bool orderDecided;

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

			if (playerObject == null) 
			{
				Debug.LogError ("<Color=Red><b>Missing</b></Color> playerObject is Missing. Please assign it in 'ConnectionManager' player object.", this);
			} 
			else 
			{
				if (TestPlayer.LocalPlayerInstance == null) 
				{
					Debug.Log("We are Instantiating LocalPlayer from " + SceneManagerHelper.ActiveSceneName + " " + Pcount);
                    //playerPosition = GameObject.Find("Map Generator").GetComponent<FileIO>().GetPlayerSPostion();
                    playerPosition = new Vector3 (10.0f, 1.25f, 0.0f);
					PhotonNetwork.Instantiate (this.playerObject.name, playerPosition, Quaternion.identity, 0);
                    Pcount++;
				} 
				else 
				{
					Debug.Log ("Ignoring Scene Load" + SceneManagerHelper.ActiveSceneName);
				}
			}
		}
		
		// Update is called once per frame
		void Update () 
		{
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
