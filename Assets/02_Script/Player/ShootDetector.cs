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

    [SerializeField, Tooltip("������ Transform")]
    private Transform rightHandTr;

    private void OnTriggerEnter(Collider other)
    {
        // �浹 ���� - ������ �ٶ󺸴� �������� ���� ������ �� �浹 �����Ѵ�
        if (other.CompareTag("Right Hand"))
        {
            // ���� ������ �������� ��� ������ �ǵ�ġ ���� �������� ���� �� �ִ�
            // ��� ���� ������ �������� �� �����Ѵ�
            var localEuler = rightHandTr.localEulerAngles;
            localEuler.z = 0;

            var worldRot = rightHandTr.parent.rotation * Quaternion.Euler(localEuler);
            var revisedForward = worldRot * Vector3.forward;

            playerMagic.ShootMagic(other.transform.position, revisedForward);
        }
        //playerMagic.ShootMagic(other.transform.position, transform.forward);
    }
}
