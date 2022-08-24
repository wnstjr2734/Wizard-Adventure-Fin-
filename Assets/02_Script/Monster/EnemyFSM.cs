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
    private int currentHP = 3; // ���߿� CharacterStatus�� ���ս�ų ��.
    // ���ʹ� Player Layer�� �����Ѵ�
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
    // ���ݴ��: Player
    private float dist;
    public float chaseDistance;
    public float attackDistance;
    public float freezeSpeed;           // �ñ� ���� �Ծ��� �� �ִϸ��̼� ��� �ӵ�
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
        //charStatus.onDead += �׾��� �� �Լ�;
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
        OnFreeze();                 // �ñ����ظ� �Ծ��� �� �ִϸ��̼� �ӵ� ����
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
        // Enemy�� Player�� �Ÿ��� �����ϰ�, �߰� �Ÿ� �̳��� Player�� ���� �ٰ��´�.
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

    public void OnFreeze()               // ���Ͱ� �ñ� ���ظ� �Ծ��� �� ��� �ִϸ��̼� ��� �ӵ��� �������� ����� �ʹ�.
    {
        //if(�ñ����ظ� �Ծ��ٸ�)
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
        //damage��ŭ ü���� ���ҽ�Ű��ʹ�.
        currentHP -= amount;
        //NavMeshAgent�� ���߰�ʹ�.
        agent.isStopped = true;
        //���� ü���� 0���϶��
        if (currentHP <= 0)
        {
            // ��������
            state = EnemyState.Die;
            animator.SetTrigger("isDie");
            // �浹ü�� off�ϰ�ʹ�.
            GetComponent<Collider>().enabled = false;
        }

        else
        {
            // ���׼ǻ���
            state = EnemyState.Damaged;
            animator.SetTrigger("isDamaged");
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
        
    }

    public virtual void OnDeathFinished()
    {
        //print("Death Finished");
        agent.isStopped = false;
        gameObject.SetActive(false);
    }
}
