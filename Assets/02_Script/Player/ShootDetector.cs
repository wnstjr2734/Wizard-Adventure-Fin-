using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 마법을 쏘는 행동(손을 뻗음)을 했는지 감지
/// 작성자 - 차영철
/// </summary>
public class ShootDetector : MonoBehaviour
{
    [SerializeField, Tooltip("발사할 기본 마법 정보를 담은 클래스")] 
    private PlayerMagic playerMagic;

    private void OnTriggerEnter(Collider other)
    {
        // 충돌 감지
        print($"Shoot Magic - {other.name}");
        playerMagic.ShootMagic(other.transform.position, other.transform.forward);
    }
}
