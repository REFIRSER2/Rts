using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public enum UnitState
{
    IDLE,
    MOVE,
    ATTACK,
    DEATH,
}

public class UnitBase : MonoBehaviour
{
    #region 유니티 기본 함수
    private void Awake()
    {
        
    }

    private void Update()
    {
        if (moveTarget != null)
        {
            aiPath.destination = moveTarget.transform.position;
            SetState(UnitState.MOVE);
        }   
    }

    private void FixedUpdate()
    {
        
    }
    #endregion
    
    #region FSM
    private UnitState state = UnitState.IDLE;
    public void SetState(UnitState st)
    {
        state = st;
        onEnterState(st);
    }

    public virtual void onEnterState(UnitState st)
    {
        switch (st)
        {
            
        }
    }
    #endregion
    
    #region 컨트롤

    [SerializeField] private GameObject selectedRing;
    [SerializeField] private AIPath aiPath;
    [SerializeField] private UnitBase moveTarget;
    
    public void Select()
    {
        selectedRing.SetActive(true);
        selectedRing.GetComponent<ParticleSystem>().Play();
    }

    public void UnSelect()
    {
        selectedRing.SetActive(false);
        selectedRing.GetComponent<ParticleSystem>().Stop();
    }
    
    public void MoveTarget(UnitBase target)
    {
        moveTarget = target;
        SetState(UnitState.MOVE);
    }

    public void MovePos(Vector3 pos)
    {
        moveTarget = null;
        aiPath.destination = pos;
        SetState(UnitState.MOVE);
    }
    #endregion
    
    #region 공격
    public void Attack()
    {
        
    }
    #endregion

    #region 프로퍼티
    private int team = -1;

    //팀
    public int GetTeam()
    {
        return team;
    }

    public void SetTeam(int num)
    {
        team = num;
    }
    //
    
    
    #endregion
}
