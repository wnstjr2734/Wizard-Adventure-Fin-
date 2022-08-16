using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �� ����ü ������ �����Ͽ� TimeScale�� �����ϴ� Ŭ����
/// </summary>
public class ProjectileDetector : MonoBehaviour
{
    [SerializeField, Tooltip("�� ����ü�� ������ �� �������� �ð�")] 
    private float slowTimeScale = 0.5f;

    // ���� ���� ���� ���� �����ִ� ����ü
    private int remainProjectileCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        // ����ü���� Ȯ���ϰ� ����
        if (other.CompareTag(""))
        {
            remainProjectileCount++;
            Debug.Assert(remainProjectileCount > 0, "Error : remain Projectile Count can't lower than 0");
            Time.timeScale = slowTimeScale;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �۵� �Ǵ��� Ȯ���� ��
        if (other.CompareTag(""))
        {
            remainProjectileCount--;
            if (remainProjectileCount == 0)
            {
                Time.timeScale = 1.0f;
            }
        }
    }
}
