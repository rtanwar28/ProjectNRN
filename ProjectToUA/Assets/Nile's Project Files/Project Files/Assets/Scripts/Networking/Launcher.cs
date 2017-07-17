using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;

namespace NRN.Tales
{
	public class Launcher : Photon.PunBehaviour 
	{

		public GameObject namePanel;

		public Text feedbackText;

		public byte maxPlayersPerRoom = 4;

		bool isConnecting;
		string _gameV = "1";
			
		void Awake()
		{
			PhotonNetwork.autoJoinLobby = false;

			PhotonNetwork.automaticallySyncScene = true;
		}

		public void Connect () 
		{
			feedbackText.text = "";

			isConnecting = true;

			namePanel.SetActive (false);

			if (PhotonNetwork.connected) 
			{
				LogFeedback ("Joining...");
				PhotonNetwork.JoinRandomRoom ();
				PhotonNetwork.JoinRandomRoom (null, (byte)(2));
			} 
			else 
			{
				LogFeedback ("Connecting...");
				PhotonNetwork.ConnectUsingSettings(_gameV);
			}
		}

		void LogFeedback(string message)
		{
			// we do not assume there is a feedbackText defined.
			if (feedbackText == null) {
				return;
			}

			// add new messages as a new line and at the bottom of the log.
			feedbackText.text += System.Environment.NewLine+message;
		}
	
		// Update is called once per frame
		public override void OnConnectedToMaster () 
		{
            Debug.Log("Region:" + PhotonNetwork.networkingPeer.CloudRegion);

            if(isConnecting)
            {
                LogFeedback("OnConnectedToMaster: Next -> try to Join Random Room");
                Debug.Log("Launcher: OnConnectedToMaster() was called and should join.");

                PhotonNetwork.JoinRandomRoom();
            }
		}

        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            LogFeedback("OnConnectedToMaster: Next -> Create a new room");
            Debug.Log("Launcher: OnConnectedToMaster() was called, No Room was Available and should create a new room.");

            PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = this.maxPlayersPerRoom }, null);
        }

        public override void OnDisconnectedFromPhoton()
        {
            LogFeedback("DisconnectedFromPhoton.");
            Debug.LogError("Launcher: Disconnected");

            isConnecting = false;
            namePanel.SetActive(true);
        }

        public override void OnJoinedRoom()
        {
            LogFeedback("OnJoinedRoom with " +PhotonNetwork.room.PlayerCount+" player(s).");
            Debug.LogError("Launcher: OnJoinedRoom has been called and is in a room now.");

            if(PhotonNetwork.room.PlayerCount ==1)
            {
                Debug.Log("We load the room for 1");

                PhotonNetwork.LoadLevel("TEST SCENE");
            }
        }
    }
}
