using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1. ���� �÷��̾ Ʈ���ſ� ����
//2. Boss Spawn ��ġ�� ��ġ�� �ٲ۴�.
/// <summary>
/// ������������ �Ա��� Ʈ���Ÿ� �ΰ� ���� ���ͷ� �̵� ��Ű��
/// ȿ�� �߰�
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
