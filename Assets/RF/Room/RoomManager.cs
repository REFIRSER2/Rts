using System;
using System.Collections.Generic;
using RF.Lobby;
using RF.Photon;
using UnityEngine;

namespace RF.Room
{
    public class RoomData
    {
        public string roomName;
        public List<MemberData> players = new List<MemberData>();
        public List<MemberData> team1 = new List<MemberData>();
        public List<MemberData> team2 = new List<MemberData>();
    }
    
    public class RoomManager : MonoBehaviour
    {
        #region 싱글톤
        public static RoomManager Instance;
        #endregion
        
        #region 방
        private RoomData roomData;

        public RoomData GetRoomData()
        {
            return roomData;
        }
        
        public void CreateRoom(string roomName)
        {
            switch (LobbyManager.Instance.GetGamemode())
            {
                case 0 :
                    PhotonManager.Instance.CreateQuickMatchRoom(roomName);
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
        }

        public void CreateRoomData(string roomName)
        {
            roomData = new RoomData();
            roomData.roomName = roomName;
        }

        public void AddRoomPlayer(MemberData data)
        {
            roomData.players.Add(data);
        }

        public void AddRoomPlayer(List<MemberData> data1, List<MemberData> data2)
        {
            roomData.team1 = data1;
            roomData.team2 = data2;
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
            
        }

        #endregion
    }
}
