using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ���� 1������ FSM
/// ������ ���� �������� �ʰ� ���ϵ��� Ȯ�������� ����ؼ� �����Ѵ�
/// (�� ���ϵ� ���̿� ��Ÿ���� ����)
/// �ۼ��� - ����ö
/// </summary>
public class BossFSM : MonoBehaviour
{
    protected enum BossState
    {
        Idle,
        Move,
        Attack,
        //Damaged,
        Die
    }

    public enum BossSounds
    {

    }

    // ���� ó�� ���� - Idle
    // ������ �� ������ ���� �� ���Ǻη� ����
    // ��Ÿ�� ť ���
    // - ����
    // �� ���� �Ŀ��� �ൿ �����̰� ���� (�� Ÿ��)
    // 

    protected BossState state;

    // ���ݴ��: Player
    public Transform attackTarget;
    private CharacterStatus targetStatus;
    


    private float dist;
    public float chaseDistance;
    public float attackDistance;
    protected NavMeshAgent agent;
    protected Animator animator;
    private CharacterStatus charStatus;
    public ElementDamage elementDamage;
    private bool moveLock;
    private bool checkDead = false;

    #region(AudioClip)
    public AudioSource audioSource;
    [Header("AudioClip")]
    public AudioClip footStep_1;
    public AudioClip footStep_2;
    [Tooltip("�÷��̾ ���� �޷����鼭 ���� �Ҹ�")]
    public AudioClip chaseGrowl;
    public AudioClip attack;
    [Tooltip("�����ϸ鼭 ���� �Ҹ�")]
    public AudioClip attackGrowl;
    [Tooltip("Ȱ���� ��� �� ���� �Ҹ�")]
    public AudioClip arrowCharge;
    [Tooltip("�������¿��� ���� �Ҹ�")]
    public AudioClip shocked;
    [Tooltip("���Ͱ� �����鼭 ���� �Ҹ�")]
    public AudioClip deadGrowl;
    [Tooltip("���Ͱ� �����鼭 ���� ���ν����� �Ҹ�")]
    public AudioClip dead;
    #endregion

    private static readonly int isPlayerDeadID = Animator.StringToHash("isPlayerDead");

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        charStatus = GetComponent<CharacterStatus>();

        attackTarget = GameObject.FindGameObjectWithTag("Player").transform;
        targetStatus = attackTarget.GetComponent<CharacterStatus>();
        audioSource = GetComponent<AudioSource>();

        charStatus.onSpeedChenge += OnFreeze;
        charStatus.onShocked += OnShocked;
        charStatus.onDead += OnDead;
    }

    private void OnEnable()
    {
        
    }

    private void Update()
    {
        
    }

    public void ResetFSM()
    {
        animator.SetBool(isPlayerDeadID, false);
    }

    void Idle()
    {
        
    }

    void Move()
    {
        
    }

    #region Attack
    void Attack()
    {
        
    }

    public virtual void OnAttackHit()
    // ����������(footman, warlord) ���� �ִϸ��̼� Ű�����ӿ� �������� �� �÷��̾�� �������� �������� �ϰ� �ʹ�.
    {
        if (agent.stoppingDistance >= dist)
        {
            // �÷��̾�� damage�� ������.
            targetStatus.TakeDamage(elementDamage);
            // �׸��� �ǰݻ��带 ����Ѵ�.
            AttackSound();
        }
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
        // ������ ����Ѵ�
    }
    #endregion

    protected virtual void OnDead()
    {
        // ��������
        // 2������� ��ȯ
    }

    public virtual void OnDeathFinished()
    {
        //print("Death Finished");
        agent.isStopped = true;
        gameObject.SetActive(false);
    }

    // �������¿��� ���Ͱ� �÷��̾�� �ٰ����� �� �ϵ��� ������ �ʿ䰡 ����.
    private IEnumerator ShockMoveLock()
    {
        yield return new WaitForSeconds(3.0f);
        moveLock = false;
    }

    #region SFX

    public void FootStep_1()
    {
        audioSource.PlayOneShot(footStep_1);
    }

    public void FootStep_2()
    {
        audioSource.PlayOneShot(footStep_2);
    }

    public void ChaseGrowl()
    {
        audioSource.PlayOneShot(chaseGrowl);
    }

    public void AttackSound()
    {
        audioSource.PlayOneShot(attack);
        audioSource.PlayOneShot(attackGrowl);

    }

    public void ArrowCharge()
    {
        audioSource.PlayOneShot(arrowCharge);
    }

    public void ShockedSound()
    {
        audioSource.PlayOneShot(shocked);
    }

    public void DeadGrowl()
    {
        audioSource.PlayOneShot(deadGrowl);
    }

    public void DeadSound()
    {
        audioSource.PlayOneShot(dead);
    }

    #endregion

}
