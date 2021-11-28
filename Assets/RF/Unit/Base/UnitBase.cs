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
        onStateBehaviour(state);
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
        /*
        switch (st)
        {
            
        }*/
    }

    public virtual void onStateBehaviour(UnitState st)
    {

        if (moveTarget != null)
        {
            //moveTarget = fov.nearEnemys.Contains(moveTarget) ? moveTarget : null;
        }

        switch (st)
        {
            case UnitState.IDLE:
                if (moveTarget != null || (aiPath.destination - transform.position).magnitude > 0)
                {
                    Move();
                }
                
                break;
            case UnitState.MOVE:
                if (moveTarget != null)
                {
                    aiPath.destination = transform.position;

                    if (Vector3.Distance(moveTarget.transform.position, transform.position) <= GetAttackDist())
                    {
                        Attack();
                    }
                    else
                    {
                        aiPath.destination = moveTarget.transform.position;
                    }
                    
                    transform.LookAt(moveTarget.transform);
                }
                break;
            case UnitState.ATTACK:
                break;
            case UnitState.DEATH:
                break;
        }
    }
    #endregion
    
    #region 컨트롤

    [SerializeField] private GameObject selectedRing;
    [SerializeField] private AIPath aiPath;
    [SerializeField] private UnitBase moveTarget;
    [SerializeField] private FieldOfView fov;
    
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

    public void Move()
    {
        //Debug.Log("Move");
        SetState(UnitState.MOVE);
    }
    #endregion
    
    #region 공격

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shotTransform;
    
    private bool delayCheck = true;
    public void Attack()
    {
        //Debug.Log("attack");
        if (delayCheck)
        {
            delayCheck = false;
            StartCoroutine("AttackTimer");
            
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = shotTransform.position;
            Projectile proj = bullet.GetComponent<Projectile>();
            proj.SetTarget(moveTarget.transform);
            proj.SetSpeed(0.1F);
            proj.SetOwner(this.gameObject);
            
            
            //moveTarget.TakeHP(1);
            
            SetState(UnitState.ATTACK);
        }

        SetState(UnitState.IDLE);
    }

    IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(attackDelay);
        delayCheck = true;
    }
    #endregion

    #region 비전
    [SerializeField] private int range;

    public int GetRange()
    {
        return range;
    }
    #endregion
    
    #region 프로퍼티
    [SerializeField]private int team = -1;

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
    
    //공격 딜레이
    private float attackDelay = 5;

    public float GetAttackDelay()
    {
        return attackDelay;
    }

    public void SetAttackDelay(float delay)
    {
        attackDelay = delay;
    }
    //
    
    //리치
    private float attackDist = 3;

    public float GetAttackDist()
    {
        return attackDist;
    }

    public void SetAttackDist(float dist)
    {
        attackDist = dist;
    }
    //

    //체력
    private int hp = 100;

    public int GetHP()
    {
        return hp;
    }

    public void SetHP(int num)
    {
        hp = num;
        hp = Mathf.Min(num, GetMaxHP());
        
        Debug.Log("HP : " + num);
    }

    public void TakeHP(int num)
    {
        SetHP(GetHP() - num);
    }

    public void AddHP(int num)
    {
        SetHP(GetHP() + num);
    }
    //
    
    //최대 체력
    private int maxHP = 100;

    public int GetMaxHP()
    {
        return maxHP;
    }

    public void SetMaxHP(int num)
    {
        maxHP = num;
    }
    //

    #endregion
}
