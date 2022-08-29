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
    // 몬스터는 Player Layer만 공격한다
    protected static int playerLayer;
    protected enum EnemyState
    {
        Idle,
        Move,
        Attack,
        //Damaged,
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
    public CharacterStatus charStatus;
    public ElementDamage elementDamage;
    private bool moveLock;
    private bool checkDead = false;

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        charStatus = GetComponent<CharacterStatus>();

        charStatus.onSpeedChenge += OnFreeze;
        charStatus.onShocked += OnShocked;
        charStatus.onDead += OnDead;
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
        if(state == EnemyState.Attack)
        {
            var targetPos = attackTarget.position;
            targetPos.y = transform.position.y;
            transform.forward = (targetPos - transform.position).normalized;
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
        // Enemy와 Player의 거리를 측정하고, 추격 거리 이내면 Move State로 전환한다.
        if (dist <= chaseDistance)
        {
            state = EnemyState.Move;
        }
    }

    void Move()
    {
        if(dist > attackDistance && moveLock == false)
        {
            agent.isStopped = false;
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

    // 몬스터가 냉기 피해를 입었을 때 모든 애니메이션 재생 속도를 느려지게 만들고 싶다.
    public void OnFreeze(float freezeSpeed)               
    {
        animator.speed = freezeSpeed;
        
    }

    public void OnShocked()
    {
        print("Shock Animation");
        moveLock = true;
        if(checkDead == true)
        {
            state = EnemyState.Die;
        }
        else
        {
            animator.SetTrigger("isShocked");
            StartCoroutine(ShockMoveLock());
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
        // 죽음상태
        checkDead = true;
        state = EnemyState.Die;
        animator.SetTrigger("isDie");
        // 충돌체를 off하고싶다.
        GetComponent<Collider>().enabled = false;
    }

    public virtual void OnDeathFinished()
    {
        //print("Death Finished");
        agent.isStopped = true;
        gameObject.SetActive(false);
    }

    private IEnumerator ShockMoveLock()
    {
        yield return new WaitForSeconds(3.0f);
        moveLock = false;
    }

}
