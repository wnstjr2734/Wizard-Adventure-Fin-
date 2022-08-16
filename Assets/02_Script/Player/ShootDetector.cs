using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ��� �ൿ(���� ����)�� �ߴ��� ����
/// �ۼ��� - ����ö
/// </summary>
public class ShootDetector : MonoBehaviour
{
    [SerializeField, Tooltip("�߻��� �⺻ ���� ������ ���� Ŭ����")] 
    private PlayerMagic playerMagic;

    private void OnTriggerEnter(Collider other)
    {
        // �浹 ����
        print($"Shoot Magic - {other.name}");
        playerMagic.ShootMagic(other.transform.position, other.transform.forward);
    }
}
