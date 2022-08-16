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

    [SerializeField, Tooltip("�ִ� ü��")] 
    private float maxHp = 100;

    [SerializeField, Tooltip("")] 
    private float attackSpeed = 1.0f;
    

    // �ܺο��� ���� ������ �� �ְ�, ���� ����� ������ �� �ֵ��� property�� ����
    public float CurrentHp { get; set; }

    public event Action<float> onHpChange;


    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        CurrentHp = maxHp;
    }

    private IEnumerator IEApplySlow()
    {
        // ���ο� ȿ�� ����

        float remainTime = ElementInfo.Instance.FireInfo.duration;
        while (remainTime > validTime)
        {

            yield return checkTime;
        }

        // �ӵ� ����
    }

    private void ResetSpeed()
    {

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

    /// <summary>
    /// ������ �� �Ӽ� ����
    /// </summary>
    public void TakeDamage(ElementDamage elementDamage)
    {
        // ������ ���� ����

        // �Ӽ��� ���� ����Ʈ ����
        
    }
}
