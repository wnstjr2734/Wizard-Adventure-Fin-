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
    [SerializeField] private GameObject healpack;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject bossSpawnObj;
    [SerializeField] private Transform bossSpawn;
    //private Transform bossSpawn;

    private void Start()
    {
        healpack = GameObject.Find("Heal Pack");
        boss = GameObject.FindGameObjectWithTag("Boss");
        bossSpawnObj = GameObject.Find("Boss Spawn");
        bossSpawn = bossSpawnObj.GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Contains("Player"))
        {
            boss.transform.position = bossSpawn.position;
            Destroy(healpack);
        }
    }
}