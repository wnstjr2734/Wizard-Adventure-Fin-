using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Է¿� ���� �� ���� ���� Ŭ����
/// �ۼ��� - ����ö
/// </summary>
public class HandController : MonoBehaviour
{
    /// <summary>
    /// �޼� ����
    /// </summary>
    public enum LeftAction
    {
        Default, // ����Ʈ, ����
        Teleport, // �ڷ���Ʈ
    }
    
    [SerializeField, Tooltip("�޼� �ִϸ��̼� ��Ʈ�ѷ�")]
    private Animator leftHandAnimator;
    // �������� ��� �� ��

    // �ִϸ��̼� ��Ʈ�� �ؽ� �ڵ�
    private readonly int animNumHash = Animator.StringToHash("AnimNum");

    public void SetLeftHandAction(LeftAction action)
    {
        leftHandAnimator.SetInteger(animNumHash, (int)action);
    }
}
