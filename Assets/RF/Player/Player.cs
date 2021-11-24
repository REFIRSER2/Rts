using RF.Team;

namespace RF.Player
{
    public class Player
    {
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
