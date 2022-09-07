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
        [SerializeField, Tooltip("���� ��ٿ�(���⿡ �����ϸ� �ʱ� ��ٿ�)")]
        private float currentCooldown = 0;

        [SerializeField, Tooltip("���� �ൿ���� ��ٸ��� �ð�")]
        private float nextActionDelay = 1.5f;
        
        public Vector2 Cooldown => cooldown;
        
        public float CurrentCooldown
        {
            get => currentCooldown;
            set => currentCooldown = value;
        }
        public float NextActionDelay => nextActionDelay;
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
    

    protected Animator animator;
    private CharacterStatus charStatus;
    private AudioSource audioSource;

    private int phase = 0;
    public int Phase
    {
        get => phase;
        private set
        {
            phase = value;
            animator.SetInteger(phaseID, phase);
        }
    }

    [Header("Common Sound")] 
    [SerializeField] private AudioClip damagedSound;
    [SerializeField] private AudioClip deathSound;

    [Header("Entrance Sound")]
    [SerializeField] private AudioClip appearanceSound;
    [SerializeField] private AudioClip enteredSound;
    [SerializeField] private AudioClip patternStartSound;

    private static readonly float actionTime = 0.1f;
    private static readonly WaitForSeconds actionWS = new WaitForSeconds(actionTime);

    private static readonly int isPlayerDeadID = Animator.StringToHash("isPlayerDead");
    private static readonly int phaseID = Animator.StringToHash("Phase");
    private static readonly int skillStateID = Animator.StringToHash("SkillState");
    private static readonly int isActionDelayID = Animator.StringToHash("IsActionDelay");
    private static readonly int isShockedID = Animator.StringToHash("isShocked");
    private static readonly int isDieID = Animator.StringToHash("isDie");

    protected void Awake()
    {
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
    }

    public void Appearance()
    {
        audioSource.PlayOneShot(appearanceSound);
    }

    public void StartKnockback()
    {
        animator.SetInteger(skillStateID, -1);
        audioSource.PlayOneShot(enteredSound);
    }

    public void StartFSM()
    {
        Phase = 1;
        audioSource.PlayOneShot(patternStartSound);
    }



    private void Update()
    {
        if (phase == 1)
        {
            DecreaseCooldown();
        }
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
        animator.SetTrigger(isShockedID);
        audioSource.Stop();
        audioSource.PlayOneShot(damagedSound);
        StopAllCoroutines();

        phase1Skill5.chargingEffect.gameObject.SetActive(false);
        phase1Skill5.chargingSphere.gameObject.SetActive(false);
        charStatus.ShockResistPercent = 999;
    }
    #endregion

    protected virtual void OnDead()
    {
        // ��������
        print("Boss Dead");
        animator.SetTrigger(isDieID);
        audioSource.PlayOneShot(deathSound);
    }

    public virtual void OnDeathFinished()
    {
        //print("Death Finished");
        gameObject.SetActive(false);
    }
}
