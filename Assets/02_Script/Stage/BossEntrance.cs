using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1. 만약 플레이어가 트리거에 들어가면
//2. Boss Spawn 위치로 위치를 바꾼다.
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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") == true)
        {
            boss.transform.position = bossSpawn.position;
        }
    }
}
