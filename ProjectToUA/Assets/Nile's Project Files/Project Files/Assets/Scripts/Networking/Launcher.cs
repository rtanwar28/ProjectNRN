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
        public Text serverText;
        public InputField cServerName;
        public InputField jServerName;
        public GameObject LogInPanels;
        public GameObject FBTexts;
        public GameObject MainMenu;
        public GameObject Lobby;

        public RoomInfo [] lobbyRooms;
        RoomOptions roomDeets;

		public byte maxPlayersPerRoom = 4;

        //int i = 0;

		bool isConnecting;
		string _gameV = "1";
			
		void Awake()
		{
			PhotonNetwork.autoJoinLobby = false;

            PhotonNetwork.automaticallySyncScene = true;
		}

        void Start()
        {
            roomDeets = new RoomOptions();
            roomDeets.IsVisible = true;
            roomDeets.MaxPlayers = 4;
        }

		public void Connect () 
		{
			feedbackText.text = "";

			isConnecting = true;

			LogInPanels.SetActive (false);

			if (PhotonNetwork.connected) 
			{
				LogFeedback ("Joining Lobby...");
                PhotonNetwork.JoinLobby();
				//PhotonNetwork.JoinRandomRoom ();
				//PhotonNetwork.JoinRandomRoom (null, (byte)(2));
			} 
			else 
			{
				LogFeedback ("Connecting to  Photon Server...");
				PhotonNetwork.ConnectUsingSettings(_gameV);
			}
		}

        public void CreateRoom ()
        {
            LogFeedback("Creating Room...");
            if (cServerName.text == "")
            {
                LogFeedback("Room Creation Failed...");
            }
            else
            {
                PhotonNetwork.CreateRoom(cServerName.text, roomDeets, TypedLobby.Default);
                LogFeedback("Created Room.");
                Lobby.SetActive(false);
               // PhotonNetwork.JoinRoom(cServerName.text);
            }
        }

        public void JoinRoom ()
        {
            if (jServerName.text == "")
            {
                LogFeedback("Server not Selected");
            }
            else
            {
                LogFeedback("Joining Server...");
                PhotonNetwork.JoinRoom(jServerName.text);
                Lobby.SetActive(false);
            }
        }

        public void LeaveRoom ()
        {
            PhotonNetwork.LeaveRoom();
            jServerName = null;
        }

        public void LeaveLobby ()
        {
            LogFeedback("Left Lobby.");
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.Disconnect();
            jServerName = null;
            LogInPanels.SetActive(true);
            FBTexts.SetActive(true);
        }

        public void GetLobbyList()
        {
            serverText.text = "";
            LogFeedback("Populating Server List");
            lobbyRooms = PhotonNetwork.GetRoomList();
            Debug.Log(lobbyRooms.Length);
            for(int i = 0; i < lobbyRooms.Length; i++)
            {
                serverText.text += lobbyRooms[i].Name + " " +  lobbyRooms[i].PlayerCount + System.Environment.NewLine;
            }
            //for (int j = 0; j <= lobbyRooms.Length; j++)
            //{
            //    roomEntryButtons[i].GetComponentInChildren<Text>().text = lobbyRooms[j].Name + lobbyRooms[j].PlayerCount;
            //    roomEntryButtons[i].GetComponent<RoomName>().roomName = lobbyRooms[j].Name;
            //    i++;
            //    if (i == 9)
            //    {
            //        i = 0;
            //    }
            //}
            //i = 0;
        }

        //public void SetServerJoin(string room)
        //{
        //    LogFeedback("Server Selected");
        //    jServerName = room;
        //}

		void LogFeedback(string message)
		{
			// we do not assume there is a feedbackText defined.
			if (feedbackText == null) {
				return;
			}

            // add new messages as a new line and at the bottom of the log.
            feedbackText.text = "";
			feedbackText.text += message+ System.Environment.NewLine;
		}
	
		public override void OnConnectedToMaster () 
		{
            Debug.Log("Region:" + PhotonNetwork.networkingPeer.CloudRegion);

            if(isConnecting)
            {
                LogFeedback("OnConnectedToMaster: Next try to join Lobby");
                Debug.Log("Launcher: OnConnectedToMaster() was called and should join.");
                FBTexts.SetActive(false);
                MainMenu.SetActive(true);
                PhotonNetwork.JoinLobby();
                LogFeedback("Lobby joined.");
                //PhotonNetwork.JoinRandomRoom();
            }
		}

        public override void OnConnectionFail(DisconnectCause cause)
        {
            Debug.Log(cause);
            base.OnConnectionFail(cause);
            LogInPanels.SetActive(true);
            LogFeedback("Connection failed.");
        }

        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            LogFeedback("OnConnectedToMaster: Next -> Create a new room");
            Debug.Log("Launcher: OnConnectedToMaster() was called, No Room was Available and should create a new room.");

            PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = this.maxPlayersPerRoom }, null);
        }

        public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
        {
            LogFeedback("Join Room Failed.");
            Lobby.SetActive(true);
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

            if(PhotonNetwork.room.PlayerCount == 1)
            {
                Debug.Log("We load the room for 1");
                //ToDo: Change to actual map scene
                PhotonNetwork.LoadLevel("MapScene");
            }
        }
    }
}
