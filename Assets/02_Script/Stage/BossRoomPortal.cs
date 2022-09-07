using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 보스 포탈 스크립트
/// 작성자 - 차영철
/// </summary>
public class BossRoomPortal : Portal
{
    [Header("Outro")]
    [SerializeField, Tooltip("아웃트로 영상")] 
    private GameObject outro;

    protected override void OnUsePortal()
    {
        // 아웃트로 틀기
        outro.SetActive(true);
    }
}
