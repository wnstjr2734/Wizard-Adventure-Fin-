using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// ĳ����(�÷��̾�, ��) �ɷ�ġ(ü��, ����, �̼� ��) ����
/// �ۼ��� - ����ö
/// </summary>
[RequireComponent(typeof(Collider))]
public class CharacterStatus : MonoBehaviour
{
    // float�� �ε��Ҽ����̹Ƿ� ���� ������ �߻��� �� ����
    // (1���� 0.1f�� 10�� ���� ��Ȯ�� 0�� ���� ����)
    // ��� ���ӽð��� �ǵ�ġ �ʰ� ������� ������ �����ϱ� ���� validTime�� ����
    private static readonly float validTime = 0.01f;
    private static readonly float checkTime = 0.1f;
    private static WaitForSeconds ws = new WaitForSeconds(checkTime);
    private static WaitForSeconds slowTime;

    // Event
    public event Action<float> onHpChange;      // ü�� ����
    public event Action onDead;                 // �׾��� ��
    public event Action<float> onSpeedChenge;   // ���ο�
    public event Action onShocked;              // ����

    [SerializeField, Tooltip("�ִ� ü��")] 
    private int maxHp = 100;
    private int currentHp;
    // ���� ���Ŀ��� ȭ�� ������ �������� �Ծ� �ٽ� �״� ���� ����
    private bool isDead = false;


    [SerializeField, Tooltip("�ӵ� ���(����, �̼�)")] 
    private float speedMultiplier = 1.0f;

    // �� ����ȿ�� �� ����
    private int burnStack = 0;  // ȭ��
    private int slowStack = 0;  // ���ο�

    private Coroutine burnCoroutine;
    private Coroutine slowCoroutine;


    private GameObject burnEffect;
    private GameObject frozenEffect;
    private GameObject shockEffect;

    private Collider hitCollider;

    #region Properties
    public int CurrentHp
    {
        get => currentHp;
        private set
        {
            if (isDead)
            {
                return;
            }

            currentHp = value;
            onHpChange?.Invoke((float)currentHp / maxHp);
            if (currentHp <= 0)
            {
                currentHp = 0;
                isDead = true;
                onDead?.Invoke();
                hitCollider.enabled = false;
            }
        }
    }

    // ���� ������ ����
    // ���� �������� ������ ������ �ְ� ����
    

    public int BurnStack
    {
        get => burnStack;
        private set
        {
            var fireInfo = ElementInfo.Instance.FireInfo;
            burnStack = math.min(value, fireInfo.maxStack);
            // ����Ʈ
        }
    }

    public int SlowStack
    {
        get => slowStack;
        private set
        {
            var iceInfo = ElementInfo.Instance.IceInfo;
            slowStack = math.min(value, iceInfo.maxStack);

            // �ӵ� ����
            speedMultiplier = 1 - (iceInfo.initSlowPercent + iceInfo.stackBonusSlowPercent * slowStack) * 0.01f;
            onSpeedChenge?.Invoke(speedMultiplier);

            // ����Ʈ ����
            
        }
    }

    #endregion


    private void Awake()
    {
        hitCollider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        CurrentHp = maxHp;
    }

    private void Start()
    {
        if (slowTime != null)
        {
            slowTime = new WaitForSeconds(ElementInfo.Instance.IceInfo.duration);
        }
    }

    /// <summary>
    /// ������ �� �Ӽ� ����
    /// </summary>
    public void TakeDamage(ElementDamage elementDamage)
    {
        // ������ ���� ����
        CurrentHp -= elementDamage.damage;

        // �Ӽ��� ���� ����Ʈ ����
        switch (elementDamage.elementType)
        {
            case ElementType.Fire:
                if (burnCoroutine != null)
                {
                    StopCoroutine(burnCoroutine);
                }
                burnCoroutine = StartCoroutine(IEApplyBurn(elementDamage.stack));
                break;
            case ElementType.Ice:
                if (slowCoroutine != null)
                {
                    StopCoroutine(slowCoroutine);
                }
                slowCoroutine = StartCoroutine(IESlow(elementDamage.stack));
                break;
            case ElementType.Lightning:
                ApplyHitGaugeBonus(elementDamage.damage);
                break;
        }
    }

    private IEnumerator IEApplyBurn(int addedStack)
    {
        BurnStack = burnStack + addedStack;

        var fireInfo = ElementInfo.Instance.FireInfo;
        float remainTime = fireInfo.duration;
        float interval = fireInfo.interval;
        while (remainTime > validTime)
        {
            remainTime -= checkTime;
            interval -= checkTime;

            if (interval < validTime)
            {
                // ȭ�� ������ ������
                CurrentHp -= fireInfo.initDamage + fireInfo.stackBonusDamage * burnStack;
                interval = fireInfo.interval;
            }

            yield return checkTime;
        }

        BurnStack = 0;
        burnCoroutine = null;
    }

    private IEnumerator IESlow(int addedStack)
    {
        SlowStack = slowStack + addedStack;
        yield return slowTime;
        
        // ���ο� ���� ����
        slowStack = 0;
        slowCoroutine = null;
    }

    private void ApplyHitGaugeBonus(int damage)
    {
        
    }
}
