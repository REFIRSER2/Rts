using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using RF.Team;
using Steamworks;
using UnityEngine.UI;

namespace RF.Player
{
    public class PlayerData
    {
        #region 체력
        private int hp = 0;
        private int maxhp = 0;

        public void SetHP(int num)
        {
            hp = num;
        }

        public int GetHP()
        {
            return hp;
        }

        public void TakeHP(int num)
        {
            SetHP(Mathf.Max(0, GetHP() - num));
        }

        public void AddHP(int num)
        {
            SetHP(Mathf.Min(GetHP() + num));
        }

        public void SetMaxHP(int num)
        {
            maxhp = num;
        }

        public int GetMaxHP()
        {
            return maxhp;
        }

        public void TakeMaxHP(int num)
        {
            SetMaxHP(Mathf.Max(0, GetMaxHP() - num));
        }
        
        public void AddMaxHP(int num)
        {
            SetMaxHP(Mathf.Min(GetMaxHP() + num));
        }
        #endregion
        
        #region 방어력
        private int armor = 0;
        private int maxarmor = 0;

        public void SetArmor(int num)
        {
            armor = num;
        }
        
        public int GetArmor()
        {
            return armor;
        }

        public void TakeArmor(int num)
        {
            SetArmor(Mathf.Max(0, GetArmor() - num));
        }

        public void AddArmor(int num)
        {
            SetArmor(Mathf.Min(GetArmor() + num, GetMaxArmor()));
        }

        public void SetMaxArmor(int num)
        {
            maxarmor = num;
        }
        public int GetMaxArmor()
        {
            return maxarmor;
        }

        public void TakeMaxArmor(int num)
        {
            SetMaxArmor(Mathf.Max(0, GetMaxArmor() - num));
        }

        public void AddMaxArmor(int num)
        {
            SetMaxArmor(Mathf.Min(GetMaxArmor() + num));
        }
        #endregion
        
        #region 보호막
        private int shield = 0;
        private int maxshield = 0;

        public void SetShield(int num)
        {
            shield = num;
        }

        public int GetShield()
        {
            return shield;
        }

        public void TakeShield(int num)
        {
            SetShield(Mathf.Max(0, GetShield() - num));
        }

        public void AddShield(int num)
        {
            SetShield(Mathf.Min(GetMaxShield(), GetShield() + num));
        }

        public void SetMaxShield(int num)
        {
            maxshield = num;
        }
        
        public int GetMaxShield()
        {
            return maxshield;
        }

        public void TakeMaxShield(int num)
        {
            SetMaxShield(Mathf.Max(0, GetMaxShield() - num));
        }

        public void AddMaxShield(int num)
        {
            SetMaxShield(Mathf.Min(GetMaxShield() + num));
        }
        #endregion
        
        #region 전력
        private int energy = 0;
        private int maxenergy = 0;

        public void SetEnergy(int num)
        {
            energy = num;
        }
        
        public int GetEnergy()
        {
            return energy;
        }

        public void TakeEnergy(int num)
        {
            SetEnergy(Mathf.Max(0, GetEnergy() - num));
        }

        public void AddEnergy(int num)
        {
            SetEnergy(Mathf.Min(GetMaxEnergy(), GetEnergy() + num));
        }

        public bool HasEnergy(int num)
        {
            return GetEnergy() >= num;
        }


        public void SetMaxEnergy(int num)
        {
            maxenergy = num;
        }
        public int GetMaxEnergy()
        {
            return maxenergy;
        }

        public void TakeMaxEnergy(int num)
        {
            SetMaxEnergy(Mathf.Max(0, GetMaxEnergy() - num));
        }
        
        public void AddMaxEnergy(int num)
        {
            SetMaxEnergy(Mathf.Min(GetMaxEnergy() + num));
        }
        #endregion
        
        #region 유닛 목록
        private List<UnitBase> units = new List<UnitBase>();
        private int maxunitCount = 0;

        public void AddUnit(UnitBase unit)
        {
            units.Add(unit);
        }

        public void TakeUnit(UnitBase unit)
        {
            units.Remove(unit);
        }
        
        public List<UnitBase> GetUnits()
        {
            return units;
        }
        
        public void SetMaxUnitCount(int num)
        {
            maxunitCount = num;
        }
        
        public int GetMaxUnitCount()
        {
            return maxunitCount;
        }

        public void TakeMaxUnitCount(int num)
        {
            SetMaxUnitCount(Mathf.Max(0, GetMaxUnitCount() - num));
        }
        
        public void AddMaxUnitCount(int num)
        {
            SetMaxUnitCount(Mathf.Min(GetMaxUnitCount() + num));
        }

        public bool CanSpawnUnit()
        {
            return units.Count < GetMaxUnitCount();
        }
        #endregion
        
        #region 팀
        private int team = -1;

        public void SetTeam(int num)
        {
            team = num;
            GameManager.Instance.Team().GetTeams()[num].members.Add();
        }
        
        public int GetTeam()
        {
            return team;
        }
        #endregion
        
    }
    
    public class Player:MonoBehaviour
    {
        #region 플레이어 데이터
        private PlayerData pData = new PlayerData();

        public PlayerData GetPlayerData()
        {
            return pData;
        }
        #endregion
        
        #region 유니티 기본 함수

        private void Awake()
        {
            GameManager.Instance.AddPlayer(this);
        }

        #endregion
    }
}
