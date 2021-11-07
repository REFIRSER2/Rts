using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartySystem : MonoBehaviour
{
    #region 싱글톤
    public static PartySystem Instance;
    #endregion

    private List<ulong> partyMembers = new List<ulong>();

    public List<ulong> GetPartyMembers()
    {
        return partyMembers;
    }

    public void CreateParty(ulong id)
    {
        partyMembers.Clear();
        partyMembers.Add(id);

        FindObjectOfType<MainMenu_UI>().RefreshParty();
    }

    public void JoinParty(List<ulong> members)
    {
        partyMembers = members;
    }
    
    #region 유니티 기본 함수
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
    #endregion
}
