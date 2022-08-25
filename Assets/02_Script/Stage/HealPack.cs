using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 메모장
// 1. 트리거 name.Contain이 player면 Destroy


/// <summary>
/// 작성자 - 이준석
/// 힐팩 구현 - 플레이어가 힐팩을 먹으면 Destroy
/// </summary>
public class HealPack : MonoBehaviour
{
    [SerializeField]
    private GameObject healpack;

    private void Start()
    {
        healpack = GameObject.Find("Heal Pack");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Contains("Player"))
        {
            Destroy(healpack);
        }
    }
}