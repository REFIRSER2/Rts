using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using RF.Lobby;
using RF.Room;
using UnityEngine;

namespace RF.Photon
{
    public class PhotonManager : MonoBehaviourPunCallbacks
    {
        #region 싱글톤
        public static PhotonManager Instance;
        #endregion
        
        #region 로컬 플레이어
        public string GetLocalID()
        {
            return PhotonNetwork.LocalPlayer.UserId;
        }
        #endregion
        
        #region 매칭
        public void CreateQuickMatchRoom(string roomName)
        {
            Debug.Log("매치 룸 생성 포톤");
            
            var options = new RoomOptions();
            options.IsOpen = true;
            options.IsVisible = false;
            options.PublishUserId = true;
            options.MaxPlayers = 8;

            var members = LobbyManager.Instance.GetLobbyData().members;
            List<string> ids = new List<string>();
            foreach (var data in members)
            {
                ids.Add(data.id);
            }

            PhotonNetwork.CreateRoom(roomName, options, null, ids.ToArray());
        }
        
        public override void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            
        }

        public override void OnCreatedRoom()
        {
            switch (LobbyManager.Instance.GetGamemode())
            {
                case 0:
                    LobbyManager.Instance.OnCreateQuickMatchRoom(PhotonNetwork.CurrentRoom.Name);
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
           
        }

        public override void OnJoinedRoom()
        {
            GameObject lbPlObj = PhotonNetwork.Instantiate("LobbyPlayer", Vector3.zero, Quaternion.identity);
            LobbyPlayer lobbyPlayer = lbPlObj.GetComponent<LobbyPlayer>();
            //lobbyPlayer.
            
            Debug.Log("매치 룸 참가");  
                
            //RoomManager.Instance.CreateRoomData(PhotonNetwork.CurrentRoom.Name);
            //RoomManager.Instance.CreateWaitData();
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            
        }

        public override void OnLeftRoom()
        {
            
        }
        
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            switch (LobbyManager.Instance.GetGamemode())
            {
                case 0:
                    LobbyManager.Instance.OnJoinQuickMatchRoom(PhotonNetwork.CurrentRoom.PlayerCount);
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
           
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
           
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            
        }
        #endregion
        
        #region 로비
        public override void OnJoinedLobby()
        {
            Debug.Log("joined lobby");
        }

        public override void OnLeftLobby()
        {
           
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            
        }

        public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            
        }
        #endregion
        
        #region 마스터
        public override void OnConnected()
        {
            Debug.Log("connected");
            
            
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
            Debug.Log("connected master");
            
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
           
        }

        public override void OnRegionListReceived(RegionHandler regionHandler)
        {
            
        }

        public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            
        }

        public override void OnCustomAuthenticationFailed(string debugMessage)
        {
            
        }
        #endregion

        #region 기본 내장 함수
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        private void Update()
        {
            Debug.Log("is connect : " + PhotonNetwork.IsConnected);
            Debug.Log("is in lobby : " + PhotonNetwork.InLobby);
        }

        #endregion
    }
}
