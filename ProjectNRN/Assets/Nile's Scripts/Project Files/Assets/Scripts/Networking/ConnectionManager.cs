﻿using System;
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

		public GameObject playerObject;

		#endregion

		#region Private Variables

		private GameObject instance;

		#endregion

		// Mono Method called for GameObject by unity for initialisaation
		void Start () 
		{
			Instance = this;

			if(!PhotonNetwork.connected)
			{
				SceneManager.LoadScene ("TEST SCENE");
				return;
			}

			if (playerObject == null) 
			{
				Debug.LogError ("<Color=Red><b>Missing</b></Color> playerObject is Missing. Please assign it in 'ConnectionManager' GameObject.", this);
			} 
			else 
			{
				if (TestPlayer.LocalPlayerInstance == null) 
				{
					Debug.Log("We are Instantiating LocalPlayer from " + SceneManagerHelper.ActiveSceneName);
					PhotonNetwork.Instantiate (this.playerObject.name, new Vector3 (32f, -40f, 190f), Quaternion.identity, 0);
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

		public void OnPhotonPlayerConnected( PhotonPlayer other )
		{
			Debug.Log ("OnPhotonPlayerConnected() " + other.NickName);

			if (PhotonNetwork.isMasterClient) 
			{
				Debug.Log ("OnPhotonPlayerConnected isMasterClient" + PhotonNetwork.isMasterClient);
				LoadArena ();
			}
		}

		public virtual void OnLeftRoom()
		{
			SceneManager.LoadScene ("TEST SCENE");
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

			PhotonNetwork.LoadLevel("TEST SCENE");
		}
	}
}
