using System;
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
public partial class BossFSM : MonoBehaviour
{
    protected enum BossState
    {
        Idle,
        Move,
        Attack,
        //Damaged,
        Die
    }

    /// <summary>
    /// 보스 스킬 정의 정보 구조체
    /// </summary>
    [System.Serializable]
    public class BossSkillData
    {
        [SerializeField, Tooltip("스킬 최소 쿨타임 / 최대 쿨타임")]
        private Vector2 cooldown = new Vector2(15, 30);
        [SerializeField, Tooltip("초기 스킬 쿨다운")]
        private float initCooldown = 0;
        [Tooltip("현재 쿨다운")]
        private float currentCooldown = 0;

        [SerializeField, Tooltip("다음 행동까지 기다리는 시간")]
        private float nextActionDelay = 1.5f;
        
        public Vector2 Cooldown => cooldown;
        public float InitCooldown => initCooldown;
        
        public float CurrentCooldown
        {
            get => currentCooldown;
            set => currentCooldown = value;
        }
        public float NextActionDelay => nextActionDelay;
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
    private Transform attackTarget;
    


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
    #endregion

    private static readonly float actionTime = 0.1f;
    private static readonly WaitForSeconds actionWS = new WaitForSeconds(actionTime);

    private static readonly int isPlayerDeadID = Animator.StringToHash("isPlayerDead");
    private static readonly int phaseID = Animator.StringToHash("Phase");
    private static readonly int skillStateID = Animator.StringToHash("SkillState");
    private static readonly int isActionDelayID = Animator.StringToHash("IsActionDelay");

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        charStatus = GetComponent<CharacterStatus>();

        audioSource = GetComponent<AudioSource>();

        charStatus.onSpeedChenge += OnSpeedChange;
        charStatus.onShocked += OnShocked;
        charStatus.onDead += OnDead;
    }

    private void Start()
    {
        attackTarget = GameManager.player.transform;

        StartCoroutine(IEPhase1_DecreaseCooldown());
    }

    public void ResetFSM()
    {
        animator.SetBool(isPlayerDeadID, false);
    }

    #region Hit Reaction

    // 몬스터가 냉기 피해를 입었을 때 애니메이션 속도 감속 및 속도 복구
    public void OnSpeedChange(float freezeSpeed)
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
    

}
