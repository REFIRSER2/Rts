using Photon.Pun;
using RF.Team;
using Steamworks;

namespace RF.Player
{
    public class Player
    {
        #region PhotonID

        private string photonID = "";

        public string GetPhotonID()
        {
            photonID = PhotonNetwork.LocalPlayer.UserId;
            return photonID;
        }
        #endregion
        
        #region SteamID

        private SteamId steamID;

        public SteamId GetSteamID()
        {
            steamID = SteamManager.Instance.steamID;
            
            return steamID;
        }
        #endregion
        
        #region 본인 여부
        public bool IsMe()
        {
            return SteamManager.Instance.steamID == steamID && GetPhotonID() == photonID;
        }
        #endregion
        
        #region 생존 여부
        private bool isAlive = true;

        public bool IsAlive()
        {
            return isAlive;
        }
        #endregion
    
        #region 체력
    
        #endregion
    
        #region 팀
        private int team = (int)TeamEnum.UNASSIGNED;

        public void SetTeam(int num)
        {
            team = num;
        }

        public int GetTeam()
        {
            return team;
        }

        public bool IsTeam(Player ply)
        {
            return ply.GetTeam() == GetTeam();
        }
        #endregion
    }
}
