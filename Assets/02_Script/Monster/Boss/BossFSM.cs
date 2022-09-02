using System;
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
    /// ���� ��ų ���� ���� ����ü
    /// </summary>
    [System.Serializable]
    public class BossSkillData
    {
        [SerializeField, Tooltip("��ų �ּ� ��Ÿ�� / �ִ� ��Ÿ��")]
        private Vector2 cooldown = new Vector2(15, 30);
        [SerializeField, Tooltip("�ʱ� ��ų ��ٿ�")]
        private float initCooldown = 0;
        [Tooltip("���� ��ٿ�")]
        private float currentCooldown = 0;

        [SerializeField, Tooltip("���� �ൿ���� ��ٸ��� �ð�")]
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

    // ���� ó�� ���� - Idle
    // ������ �� ������ ���� �� ���Ǻη� ����
    // ��Ÿ�� ť ���
    // - ����
    // �� ���� �Ŀ��� �ൿ �����̰� ���� (�� Ÿ��)
    // 

    protected BossState state;

    // ���ݴ��: Player
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

    // ���Ͱ� �ñ� ���ظ� �Ծ��� �� �ִϸ��̼� �ӵ� ���� �� �ӵ� ����
    public void OnSpeedChange(float freezeSpeed)
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
    

}
