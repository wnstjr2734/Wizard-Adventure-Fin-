using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 스폰 트리거 되면
//적 공장에서 적 생성
//적 소서러, 궁수, 근접 랜덤 생성
//적 수

/// <summary>
/// 텔레포트 되면 적 활성화
/// </summary>
public class EnemySpawn : MonoBehaviour
{
    public Portal portal;
    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!isTriggered && other.name.Contains("Player"))
        {
            portal.StartRoom();
            isTriggered = true;
        }
    }
}