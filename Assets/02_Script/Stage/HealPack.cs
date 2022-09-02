using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �޸���
// 1. Ʈ���� name.Contain�� player�� Destroy


/// <summary>
/// �ۼ��� - ���ؼ�
/// ���� ���� - �÷��̾ ������ ������ Destroy
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