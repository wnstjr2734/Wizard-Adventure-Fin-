using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1. 만약 플레이어가 트리거에 들어가면
//2. Boss Spawn 위치로 위치를 바꾼다.
/// <summary>
/// 보스스테이지 입구에 트리거를 두고 보스 센터로 이동 시키기
/// 효과 추가
/// </summary>
public class BossEntrance : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject bossSpawnObj;
    [SerializeField] private Transform bossSpawn;
    //private Transform bossSpawn;


    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        bossSpawnObj = GameObject.Find("Boss Spawn");
        bossSpawn = bossSpawnObj.GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Contains("Player") == true)
        {
            boss.transform.position = bossSpawn.position;
        }
    }
}
