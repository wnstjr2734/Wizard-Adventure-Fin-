using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// �̸��� State ���� �� �ִϸ��̼� �۵�
/// �ۼ��� - ������
/// </summary>

public class EnemyFSM : MonoBehaviour
{
    // ���ʹ� Player Layer�� �����Ѵ�
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
    // ���ݴ��: Player
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
        // Enemy�� Player�� �Ÿ��� �����ϰ�, �߰� �Ÿ� �̳��� Move State�� ��ȯ�Ѵ�.
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
        // ����������(footman, warlord) ���� �ִϸ��̼� Ű�����ӿ� �������� �� �÷��̾�� �������� �������� �ϰ� �ʹ�.
    {
       if(agent.stoppingDistance >= dist)
        {
            charStatus.TakeDamage(elementDamage);
            // �÷��̾�� damage�� ������.
            // �׸��� �ǰݻ��带 ����Ѵ�.
        }
    }

    public virtual void OnAttackFinished()
    {
      
    }
    #endregion

    #region Hit Reaction

    // ���Ͱ� �ñ� ���ظ� �Ծ��� �� ��� �ִϸ��̼� ��� �ӵ��� �������� ����� �ʹ�.
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
    /// ������ Ÿ�ݵǼ��� �� �ǰ�, ����� �Ѵ�
    /// </summary>
    
    protected virtual void OnDead()
    {
        // ��������
        checkDead = true;
        state = EnemyState.Die;
        animator.SetTrigger("isDie");
        // �浹ü�� off�ϰ�ʹ�.
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
