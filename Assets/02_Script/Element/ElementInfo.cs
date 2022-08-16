using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    None,
    Fire,
    Ice,
    Lightning
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

    [Serializable]
    public class Ice
    {
        [Tooltip("�ִ� ����")]
        public int maxStack;
        [Tooltip("���� �ð�")]
        public float duration;
        [Tooltip("�ʱ� ���ο� ����")]
        public float initSlowRate;
        [Tooltip("�߰� ���ο� ����")] 
        public float stackBonusSlow;
    }

    [Serializable]
    public class Lightning
    {
        // ���� ���ʽ�
        //public float 
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

    private void OnEnable()
    {
        Instance = this;
        Debug.Log("Element Info Init");
    }
}