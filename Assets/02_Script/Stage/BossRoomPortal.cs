using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ��Ż ��ũ��Ʈ
/// �ۼ��� - ����ö
/// </summary>
public class BossRoomPortal : Portal
{
    [Header("Outro")]
    [SerializeField, Tooltip("�ƿ�Ʈ�� ����")] 
    private GameObject outro;

    protected override void OnUsePortal()
    {
        // �ƿ�Ʈ�� Ʋ��
        outro.SetActive(true);
    }
}
