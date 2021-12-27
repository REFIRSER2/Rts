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
    public class PhotonManager : MonoBehaviour, IMatchmakingCallbacks, IInRoomCallbacks, ILobbyCallbacks, IConnectionCallbacks
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
        
        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            
        }

        public void OnCreatedRoom()
        {
            switch (LobbyManager.Instance.GetGamemode())
            {
                case 0:
                    LobbyManager.Instance.OnCreateQuickMatchRoom();
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
           
        }

        public void OnJoinedRoom()
        {
            RoomManager.Instance.CreateRoomData(PhotonNetwork.CurrentRoom.Name);
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            
        }

        public void OnLeftRoom()
        {
            
        }
        
        public void OnPlayerEnteredRoom(Player newPlayer)
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

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
           
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
           
        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {
            
        }
        #endregion
        
        #region 로비
        public void OnJoinedLobby()
        {
            Debug.Log("joined lobby");
        }

        public void OnLeftLobby()
        {
           
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            
        }
        #endregion
        
        #region 마스터
        public void OnConnected()
        {
            
        }

        public void OnConnectedToMaster()
        {
            Debug.Log("connected master");
            PhotonNetwork.JoinLobby();
        }

        public void OnDisconnected(DisconnectCause cause)
        {
           
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
            
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
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
        #endregion
    }
}
