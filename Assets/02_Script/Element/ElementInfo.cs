using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    Fire,
    Ice,
    Lightning,
    None,   // None�� Count ������ �ϱ⵵ ��
}

/// <summary>
/// �Ӽ� ȿ�� ���� Ŭ����
/// �Ӽ��� �ʿ��� ������ Singleton �������� �����Ͽ� ������ ���
/// �ۼ��� - ����ö
/// </summary>
[CreateAssetMenu(fileName = "Element Info", menuName = "Game Data/Element Info")]
public class ElementInfo : ScriptableObject
{
    /// <summary>
    /// �� �Ӽ��� ȭ�� �������� �߰��� ����
    /// ȭ�� ������ - �ֱ������� ������ ������
    /// </summary>
    [Serializable]
    public class Fire
    {
        [Tooltip("�ִ� ����")]
        public int maxStack;
        [Tooltip("���� �ð�")]
        public float duration;
        [Tooltip("�ʱ� ȭ�� ������")]
        public int initDamage;
        [Tooltip("���� �� �߰� ȭ�� ������")]
        public int stackBonusDamage;
        [Tooltip("ȭ�� �� �ʿ� �� �� ������")]
        public float interval;

        public int BurnDamage(int stack)
        {
            return initDamage + (stack - 1) * stackBonusDamage;
        }
    }

    /// <summary>
    /// ���� �Ӽ� ������ ���� ��ġ�� ���� �����ð����� ���ο�(����, �̼�)�� �ش�
    /// </summary>
    [Serializable]
    public class Ice
    {
        [Tooltip("�ִ� ����")]
        public int maxStack = 5;
        [Tooltip("���� �ð�")]
        public float duration = 3.0f;
        [Tooltip("�ʱ� ���ο� ����")] 
        public float initSlowPercent = 25f;
        [Tooltip("�߰� ���ο� ����")] 
        public float stackBonusSlowPercent = 5f;
    }

    /// <summary>
    /// ���� �Ӽ��� �߰� ���� ���ʽ��� �ش�
    /// </summary>
    [Serializable]
    public class Lightning
    {
        // ���� ���ʽ�
        public float hitGaugeMultiplier = 2.5f;
    }

    public static ElementInfo Instance { get; private set; }

    [SerializeField] 
    private Fire fireInfo;
    [SerializeField]
    private Ice iceInfo;
    [SerializeField]
    private Lightning lightningInfo;

    // �ܺο��� �б⸸ �����ϵ��� getter ���� ���
    public Fire FireInfo => fireInfo;
    public Ice IceInfo => iceInfo;
    public Lightning LightningInfo => lightningInfo;

    private void Awake()
    {
        Instance = this;
        Debug.Log("Element Info Init");
    }
}