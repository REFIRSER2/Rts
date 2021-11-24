using System.Collections.Generic;
using UnityEngine;

namespace RF.Team
{
    public static class Team
    {
        private static Dictionary<int, TeamData> teamDatas = new Dictionary<int, TeamData>();

        public static void SetUp(int index, string name)
        {
            TeamData data = new TeamData();
            data.name = name;

            if (!teamDatas.ContainsKey(index))
            {
                teamDatas.Add(index, data);
            }
        }

        public static void AddPlayer(int index, Player.Player ply)
        {
            if (!teamDatas.ContainsKey(index))
            {
                return;
            }
            
            if (teamDatas.ContainsKey(ply.GetTeam()))
            {
                if (teamDatas[ply.GetTeam()].members.Contains(ply))
                {
                    teamDatas[ply.GetTeam()].members.Remove(ply);
                }
            }
            
            teamDatas[index].members.Add(ply);
        }

        public static void RemovePlayer(int index, Player.Player ply)
        {
            ply.SetTeam((int)TeamEnum.UNASSIGNED);
            
            if (!teamDatas.ContainsKey(index))
            {
                return;
            }

            teamDatas[index].members.Remove(ply);
        }

        public static string GetName(int index)
        {
            if (teamDatas.ContainsKey(index))
            {
                return teamDatas[index].name;
            }

            return "NONE";
        }

        public static List<Player.Player> GetMembers(int index)
        {
            if (teamDatas.ContainsKey(index))
            {
                return teamDatas[index].members;
            }

            return null;
        }
        
        public static Dictionary<int, TeamData> GetTeams()
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

        public List<Player.Player> members = new List<Player.Player>();
    }
}
