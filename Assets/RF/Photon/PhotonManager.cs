using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using RF.Debug;
using UnityEngine;

namespace RF.Photon
{
    public class PhotonManager : MonoBehaviourPunCallbacks, ILobbyCallbacks, IConnectionCallbacks
    {
        #region 싱글톤
        public static PhotonManager Instance;
        #endregion
        
        #region 로비
        private void SetupLobby()
        {
            DebugManager.Instance.Debug("photon", "로비 셋업");

            PhotonNetwork.ConnectUsingSettings();
        }

        public void CreateQuickRoom(int rank, int gameMode)
        {
            Hashtable expectedCustomRoomProperties = new Hashtable(){{"rank", rank}, {"gameMode", gameMode}};
            RoomOptions options = new RoomOptions();
            options.IsOpen = true;
            options.IsVisible = false;
            options.MaxPlayers = 8;
            options.CustomRoomProperties = new Hashtable() {{"rank", rank}, {"gameMode", gameMode}};
            options.CustomRoomPropertiesForLobby = new string[] {"rank", "gameMode"};
            options.PublishUserId = true;
            PhotonNetwork.JoinRandomOrCreateRoom(expectedCustomRoomProperties, 8, MatchmakingMode.FillRoom, null, null, null);
        }
        
        public void OnJoinedLobby()
        {
            DebugManager.Instance.Debug("photon", "로비 참여");
        }

        public void OnLeftLobby()
        {
            DebugManager.Instance.Debug("photon", "로비 떠남");
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            DebugManager.Instance.Debug("photon", "방 목록 새로고침");
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            DebugManager.Instance.Debug("photon", "로비 새로고침");
        }
        #endregion
        
        #region 연결
        public void OnConnected()
        {
           
        }

        public void OnConnectedToMaster()
        {
            DebugManager.Instance.Debug("photon", "마스터 서버 연결");
            
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
        
        #region 유니티 기본 내장 함수
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
            SetupLobby();
        }

        #endregion
    }
}
