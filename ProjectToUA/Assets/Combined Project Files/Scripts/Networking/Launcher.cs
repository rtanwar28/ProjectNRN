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

        public Text feedbackText, serverText, roomText, user, roomHeading;
        public InputField cServerName, jServerName;
        public GameObject LogInPanels, FBTexts;
        public GameObject MainMenu, Lobby, Room, loginMenu, registerMenu, csMenu;

        public RoomInfo [] lobbyRooms;
        //public String[][] playersInfo;
        //public String[] playerInfo;
        RoomOptions roomDeets;

		public byte maxPlayersPerRoom = 2;

        //int i = 0;

		bool isConnecting;
        bool isReady;
		string _gameV = "1";
        string playerUsername;
        int readyCount;
			
		void Awake()
		{
			PhotonNetwork.autoJoinLobby = false;

            PhotonNetwork.automaticallySyncScene = true;

            namePanel = GameObject.Find("LUNText");

            feedbackText = GameObject.Find("ConnectionText").GetComponent<Text>();
            serverText = GameObject.Find("ServerText").GetComponent<Text>();
            roomText = GameObject.Find("RoomInfoText").GetComponent<Text>();
            user = GameObject.Find("UserHeadText").GetComponent<Text>();
            roomHeading = GameObject.Find("RHeadingText").GetComponent<Text>();
            cServerName = GameObject.Find("CreateServer").GetComponent<InputField>();
            jServerName = GameObject.Find("JoinServer").GetComponent<InputField>();

            LogInPanels = GameObject.Find("LogInAndRegisterPanels");
            FBTexts = GameObject.Find("FBTexts");
            MainMenu = GameObject.Find("MainMenu");
            Lobby = GameObject.Find("Lobby");
            Room = GameObject.Find("Room");
            loginMenu = GameObject.Find("LoginMenu");
            registerMenu = GameObject.Find("RegisterMenu");

            
            MainMenu.gameObject.SetActive(false);
            Lobby.gameObject.SetActive(false);
            Room.gameObject.SetActive(false);
            loginMenu.SetActive(true);
            FBTexts.gameObject.SetActive(true);
            registerMenu.SetActive(false);

        }

        void Start()
        {
            roomDeets = new RoomOptions();
            roomDeets.IsVisible = true;
            roomDeets.MaxPlayers = 4;
            //playerInfo = new String[4];
            //playersInfo = new String[roomDeets.MaxPlayers][];
            //playersInfo[0] = new String[4];
            //playersInfo[1] = new String[4];
            //playersInfo[2] = new String[4];
            //playersInfo[3] = new String[4];
            //isReady = false;
        }

		public void Connect (string username) 
		{
			feedbackText.text = "";

			isConnecting = true;
            PhotonNetwork.playerName = username;
            user.text = username;
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
                //playerInfo[0] = "Host";
                //playerInfo[1] = PhotonNetwork.playerName;
                //playerInfo[2] = "Character";
                //playerInfo[3] = "Unready";
                PhotonNetwork.CreateRoom(cServerName.text, roomDeets, TypedLobby.Default);
                LogFeedback("Created Room.");
                Lobby.SetActive(false);
                Room.SetActive(true);
                roomHeading.text += cServerName.text;
                //for (int j = 0; j < playerInfo.Length; j++)
                //{
                //    roomText.text += "   " + playerInfo[j];
                //}
                //roomText.text += System.Environment.NewLine;
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
                //playerInfo[0] = "    ";
                //playerInfo[1] = PhotonNetwork.playerName;
                //playerInfo[2] = "Character";
                //playerInfo[3] = "Unready";
                LogFeedback("Joining Server...");
                PhotonNetwork.JoinRoom(jServerName.text);
                Lobby.SetActive(false);
                Room.SetActive(true);
                roomHeading.text += jServerName.text;
                //for (int j = 0; j < playerInfo.Length; j++)
                //{
                //    roomText.text += "   " + playerInfo[j];
                //}
                //roomText.text += System.Environment.NewLine;
            }
        }

        public void LeaveRoom ()
        {
            //for (int x = 0; x < playersInfo.Length; x++)
            //{
            //    playersInfo[PhotonNetwork.player.ID - 1][x] = "";
            //}
            PhotonNetwork.LeaveRoom();
            jServerName.text = "";
            Lobby.SetActive(true);
            Room.SetActive(false);
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

        public void GetCharNameFromCS()
        {

        }

        public void LoadLevel()
        {
            if (PhotonNetwork.playerList.Length >= 1)//roomDeets.MaxPlayers)
            {
                Room.SetActive(false);
                PhotonNetwork.room.IsOpen = false;
                PhotonNetwork.room.IsVisible = false;
                foreach (PhotonPlayer pep in PhotonNetwork.playerList)
                {
                    Debug.Log("Loading the Map for the Game");
                    //ToDo: Change to actual map scene

                    //PhotonNetwork.LoadLevel("Level1");
                    PhotonNetwork.LoadLevel(1);

                }
            }
            else
            {
                LogFeedback("Not Enough Players are present.");
            }
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

        //public override void OnJoinedRoom()
        //{
        //    LogFeedback("OnJoinedRoom with " +PhotonNetwork.room.PlayerCount+" player(s).");
        //    Debug.LogError("Launcher: OnJoinedRoom has been called and is in a room now.");

        //    //if(PhotonNetwork.room.PlayerCount == 1)
        //    //{
        //    Debug.Log("We load the room for Board Game");
        //    //ToDo: Change to actual map scene
        //    PhotonNetwork.LoadLevel("Level1");
        //    //}
        //}


        public void SignUpButton()
        {
            loginMenu.SetActive(false);
            registerMenu.SetActive(true);
        }

        public void ProceedToLogin()
        {
            registerMenu.SetActive(false);
            loginMenu.SetActive(true);
        }

        void Update()
        {
            if(PhotonNetwork.inRoom)
            {
                roomText.text = "";
                for (int i = 0; i <  PhotonNetwork.playerList.Length; i++)
                {
                    roomText.text += "    " + PhotonNetwork.playerList[i].ID + "         " + PhotonNetwork.playerList[i].NickName + System.Environment.NewLine;
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                QuitApplication();
            }
        }

        public void QuitApplication()
        {
            Application.Quit();
        }
    }
}
