using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적이 쏜 투사체 공격을 감지하여 TimeScale을 조정하는 클래스
/// </summary>
public class ProjectileDetector : MonoBehaviour
{
    [SerializeField, Tooltip("적 투사체가 들어왔을 때 느려지는 시간")] 
    private float slowTimeScale = 0.5f;

    // 현재 판정 범위 내에 들어와있는 투사체
    private int remainProjectileCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        print($"Detected Enter - {other.tag}");
        // 투사체인지 확인하고 막기
        if (other.CompareTag("Projectile"))
        {
            remainProjectileCount++;
            Debug.Assert(remainProjectileCount > 0, "Error : remain Projectile Count can't lower than 0");
            Time.timeScale = slowTimeScale;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print("Detected Out");
        // 작동 되는지 확인할 것
        if (other.CompareTag("Projectile"))
        {
            remainProjectileCount--;
            if (remainProjectileCount == 0)
            {
                Time.timeScale = 1.0f;
            }
        }
    }
}
