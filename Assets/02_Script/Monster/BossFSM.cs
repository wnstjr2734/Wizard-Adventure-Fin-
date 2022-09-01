using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 보스 1페이즈 FSM
/// 보스는 직접 공격하지 않고 패턴들을 확률적으로 사용해서 공격한다
/// (각 패턴들 사이엔 쿨타임이 있음)
/// 작성자 - 차영철
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

    // 보스 처음 시작 - Idle
    // 보스가 각 패턴을 랜덤 및 조건부로 쓴다
    // 쿨타임 큐 방식
    // - 패턴
    // 각 패턴 후에는 행동 딜레이가 있음 (딜 타임)
    // 

    protected BossState state;

    // 공격대상: Player
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
    [Tooltip("플레이어를 향해 달려오면서 내는 소리")]
    public AudioClip chaseGrowl;
    public AudioClip attack;
    [Tooltip("공격하면서 내는 소리")]
    public AudioClip attackGrowl;
    [Tooltip("활시위 당길 때 나는 소리")]
    public AudioClip arrowCharge;
    [Tooltip("경직상태에서 내는 소리")]
    public AudioClip shocked;
    [Tooltip("몬스터가 죽으면서 내는 소리")]
    public AudioClip deadGrowl;
    [Tooltip("몬스터가 죽으면서 나는 뼈부숴지는 소리")]
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
    // 근접몬스터의(footman, warlord) 공격 애니메이션 키프레임에 도달했을 때 플레이어에게 데미지를 입히도록 하고 싶다.
    {
        if (agent.stoppingDistance >= dist)
        {
            // 플레이어에게 damage를 입힌다.
            targetStatus.TakeDamage(elementDamage);
            // 그리고 피격사운드를 재생한다.
            AttackSound();
        }
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
        // 패턴을 취소한다
    }
    #endregion

    protected virtual void OnDead()
    {
        // 죽음상태
        // 2페이즈로 전환
    }

    public virtual void OnDeathFinished()
    {
        //print("Death Finished");
        agent.isStopped = true;
        gameObject.SetActive(false);
    }

    // 경직상태에서 몬스터가 플레이어에게 다가오지 못 하도록 막아줄 필요가 있음.
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
