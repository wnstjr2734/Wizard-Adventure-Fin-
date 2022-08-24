using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 쫄몹들 State 설정 및 애니메이션 작동
/// 작성자 - 성종현
/// </summary>

public class EnemyFSM : MonoBehaviour
{
    private int currentHP = 3; // 나중에 CharacterStatus에 통합시킬 것.
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
    public float freezeSpeed;           // 냉기 피해 입었을 때 애니메이션 재생 속도
    protected NavMeshAgent agent;
    protected Animator animator;
    public CharacterStatus charStatus;

    public ElementDamage elementDamage;


    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        charStatus = GetComponent<CharacterStatus>();

        //charStatus.onShocked += OnDamaged;
        //charStatus.onDead += 죽었을 때 함수;
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
        OnFreeze();                 // 냉기피해를 입었을 때 애니메이션 속도 조절
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
        animator.SetBool("isAttack", true);
        if (dist>attackDistance)
        {
            animator.SetBool("isAttack", false);
            state = EnemyState.Move;
        }
    }

    public virtual void OnAttackHit()               
        // 근접몬스터의(footman, warlord) 공격 애니메이션 키프레임에 도달했을 때 플레이어에게 데미지를 입히도록 하고 싶다.
    {
       if(agent.stoppingDistance >= dist)
        {
            charStatus.TakeDamage(elementDamage);
            // 플레이어에게 damage를 입힌다.
            // 그리고 피격사운드를 재생한다.
        }
    }

    public virtual void OnAttackFinished()
    {
      
    }
    #endregion

    #region Hit Reaction

    public void OnFreeze()               // 몬스터가 냉기 피해를 입었을 때 모든 애니메이션 재생 속도를 느려지게 만들고 싶다.
    {
        //if(냉기피해를 입었다면)
        //{
        //    animator.speed = freezeSpeed;

        //}
        //else
        //{
        //    animator.speed = 1.0f;
        //}
    }
   
    public void OnDamaged(int amount)
    {
        if (state == EnemyState.Die)
        {
            return;
        }
        //damage만큼 체력을 감소시키고싶다.
        currentHP -= amount;
        //NavMeshAgent를 멈추고싶다.
        agent.isStopped = true;
        //만약 체력이 0이하라면
        if (currentHP <= 0)
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
