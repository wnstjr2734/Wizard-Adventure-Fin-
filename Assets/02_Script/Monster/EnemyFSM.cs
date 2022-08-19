using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyFSM : MonoBehaviour
{
    private int enemyHP = 3;
    // 몬스터는 Player Layer만 공격한다
    protected static int playerLayer;
    protected enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Damaged,
        Die
    }

    protected EnemyState state;

    
    public Transform attackTarget;
    // 공격대상: Player
    private float dist;
    public float chaseDistance;
    public float attackDistance;
    protected NavMeshAgent agent;
    protected Animator animator;

   
    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        StartCoroutine(UpdateState());
        state = EnemyState.Idle;
        agent.isStopped = false;
    }

    private void Update()
    {
        dist = Vector3.Distance(this.transform.position, attackTarget.transform.position);
        //print(dist);
        if(state == EnemyState.Attack)
        {
            this.transform.LookAt(attackTarget);
        }
    }

    IEnumerator UpdateState()
    {
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            switch (state)
            {
                case EnemyState.Idle:
                    Idle();
                    break;
                case EnemyState.Move:
                    Move();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Idle()
    {
        // Enemy와 Player의 거리를 측정하고, 추격 거리 이내면 Player를 향해 다가온다.
        if (dist <= chaseDistance)
        {
            state = EnemyState.Move;
        }
    }

    void Move()
    {
        agent.isStopped = false;
        if(dist > attackDistance)
        {
            animator.SetBool("isMove", true);
            agent.SetDestination(attackTarget.transform.position);
        }
        else
        {
            animator.SetBool("isMove", false);
            state = EnemyState.Attack;
        }
    }

    #region Attack
    void Attack()
    {
        agent.isStopped = true;
        //this.transform.LookAt(attackTarget);
        animator.SetBool("isAttack", true);
        if (dist>attackDistance)
        {
            animator.SetBool("isAttack", false);
            state = EnemyState.Move;
        }
    }

    public virtual void OnAttackHit()
    {
       if(agent.stoppingDistance >= dist)
        {
            // 플레이어에게 damage를 입힌다.
            // 그리고 피격사운드를 재생한다.
        }
    }

    public virtual void OnAttackFinished()
    {
      
    }
    #endregion

    #region Hit Reaction

   
    public void OnDamaged(int amount)
    {
        if (state == EnemyState.Die)
        {
            return;
        }
        //damage만큼 체력을 감소시키고싶다.

        enemyHP -= amount;
        //NavMeshAgent를 멈추고싶다.
        agent.isStopped = true;
        //만약 체력이 0이하라면
        if (enemyHP <= 0)
        {
            // 죽음상태
            state = EnemyState.Die;
            animator.SetTrigger("isDie");
            // 충돌체를 off하고싶다.
            GetComponent<Collider>().enabled = false;
        }

        else
        {
            // 리액션상태
            state = EnemyState.Damaged;
            animator.SetTrigger("isDamaged");
        }
    }

    public virtual void OnReactFinished()
    {
       
    }
    #endregion

    /// <summary>
    /// 죽으면 타격되서도 안 되고, 멈춰야 한다
    /// </summary>
    
    protected virtual void OnDead()
    {
        
    }

    public virtual void OnDeathFinished()
    {
        //print("Death Finished");
        agent.isStopped = false;
        gameObject.SetActive(false);
    }
}
