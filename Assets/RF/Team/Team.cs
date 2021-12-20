using System.Collections.Generic;
using UnityEngine;

namespace RF.Team
{
    public class Team
    {
        private Dictionary<int, TeamData> teamDatas = new Dictionary<int, TeamData>();

        public void SetUp(int index, string name, List<Transform> spawnPoints)
        {
            TeamData data = new TeamData();
            data.name = name;
            data.spawnPoints = spawnPoints;
            
            if (!teamDatas.ContainsKey(index))
            {
                teamDatas.Add(index, data);
            }
        }

        public void AddPlayer(int index, Player.Player ply)
        {
            if (!teamDatas.ContainsKey(index))
            {
                return;
            }
            
            if (teamDatas.ContainsKey(ply.GetPlayerData().GetTeam()))
            {
                if (teamDatas[ply.GetPlayerData().GetTeam()].members.Contains(ply))
                {
                    teamDatas[ply.GetPlayerData().GetTeam()].members.Remove(ply);
                }
            }
            
            teamDatas[index].members.Add(ply);
        }

        public void RemovePlayer(int index, Player.Player ply)
        {
            ply.GetPlayerData().SetTeam((int)TeamEnum.UNASSIGNED);
            
            if (!teamDatas.ContainsKey(index))
            {
                return;
            }

            teamDatas[index].members.Remove(ply);
        }

        public string GetName(int index)
        {
            if (teamDatas.ContainsKey(index))
            {
                return teamDatas[index].name;
            }

            return "NONE";
        }

        public List<Player.Player> GetMembers(int index)
        {
            if (teamDatas.ContainsKey(index))
            {
                return teamDatas[index].members;
            }

            return null;
        }
        
        public Dictionary<int, TeamData> GetTeams()
        {
            return teamDatas;
        }
    }

    public enum TeamEnum
    {
        RED = 0,
        BLUE = 1,
        SPEC = 99,
        UNASSIGNED = -99,
    }
    
    public class TeamData
    {
        public string name;
        public List<Transform> spawnPoints;
        
        public List<Player.Player> members = new List<Player.Player>();
    }
}
