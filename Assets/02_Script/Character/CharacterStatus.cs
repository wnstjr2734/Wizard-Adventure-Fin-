using System;
using System.Collections;
using System.Collections.Generic;
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
    
    public event Action<float> onHpChange;
    public event Action onDead;
    // ����
    public event Action onShocked;

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
                break;
            case ElementType.Ice:
                break;
            case ElementType.Lightning:
                break;
        }
    }

    private IEnumerator IEApplyBurn()
    {
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
                interval = fireInfo.interval;
            }

            yield return checkTime;
        }
    }

    private IEnumerator IESlow()
    {
        // ���ο� ȿ�� ����

        // ����Ʈ ����

        yield return slowTime;

        // �ӵ� ����
        ClearSlow();
        // ����Ʈ ����
    }

    private void ApplySlow()
    {

    }

    private void ClearSlow()
    {
        attackSpeed = 1.0f;
        moveSpeed = 1.0f;
    }
}
