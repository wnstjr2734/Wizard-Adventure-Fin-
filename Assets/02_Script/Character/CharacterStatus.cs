using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// ĳ����(�÷��̾�, ��) �ɷ�ġ(ü��, ����, �̼� ��) ����
/// �ۼ��� - ����ö
/// </summary>
public class CharacterStatus : MonoBehaviour
{
    // float�� �ε��Ҽ����̹Ƿ� ���� ������ �߻��� �� ����
    // (1���� 0.1f�� 10�� ���� ��Ȯ�� 0�� ���� ����)
    // ��� ���ӽð��� �ǵ�ġ �ʰ� ������� ������ �����ϱ� ���� validTime�� ����
    private static readonly float validTime = 0.01f;
    private static readonly float checkTime = 0.1f;
    private static WaitForSeconds ws = new WaitForSeconds(checkTime);
    private static WaitForSeconds slowTime;

    [SerializeField, Tooltip("�ִ� ü��")] 
    private int maxHp = 100;
    private int currentHp;



    [SerializeField, Tooltip("���� �ӵ�")] 
    private float attackSpeed = 1.0f;
    [SerializeField, Tooltip("�̵� �ӵ�")]
    private float moveSpeed = 1.0f;

    // �� ����ȿ�� �� ����
    private int burnStack = 0;  // ȭ��
    private int slowStack = 0;  // ���ο�

    private Coroutine burnCoroutine;
    private Coroutine slowCoroutine;
    
    public event Action<float> onHpChange;      // ü�� ����
    public event Action onDead;                 // �׾��� ��
    public event Action<float> onSpeedChenge;   // ���ο�
    public event Action onShocked;              // ����

    #region Properties
    public int CurrentHp
    {
        get => currentHp;
        private set
        {
            currentHp = value;
            onHpChange?.Invoke((float)currentHp / maxHp);
            if (currentHp <= 0)
            {
                currentHp = 0;
                onDead?.Invoke();
            }
        }
    }

    // ��Ʈ ������ ����
    // ��Ʈ �������� ������ ������ �ְ� ����

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
            // �ӵ� ���� �˸�

            // ����Ʈ ����
        }
    }

    #endregion


    private void Awake()
    {
        
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
                StopCoroutine(burnCoroutine);
                burnCoroutine = StartCoroutine(IEApplyBurn(elementDamage.stack));
                break;
            case ElementType.Ice:
                StopCoroutine(slowCoroutine);
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
    }

    private IEnumerator IESlow(int addedStack)
    {
        SlowStack = slowStack + addedStack;
        yield return slowTime;
        
        // ���ο� ���� ����
        slowStack = 0;

    }

    private void ApplyHitGaugeBonus(int damage)
    {

    }
}
