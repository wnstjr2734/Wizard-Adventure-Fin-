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

    [SerializeField] private float playerMoveTime = 3.0f;
    [SerializeField] private Transform playerMovePos;

    [SerializeField] private ParticleSystem teleportEffect;
    
    [SerializeField] private GameObject bossHp;

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
            print("Trigger Boss Entrance");
            StartCoroutine(IEBossIntro());
        }
    }

    private IEnumerator IEBossIntro()
    {
        //플레이어 보스 방 진입
        //    -플레이어가 피 채우는 곳까지 이동
        //    (텔레포트 존으로 강제 이동은 보류)
        //-플레이어 조작 막아놓기
        //    -> 플레이어 강제 이동 - 제단 ? 까지
        //플레이어가 제단 도착하면 넉백 시키기
        //    -가능하면 힐팩 있던 곳까지 넉백. 데미지는 안 입힘
        var player = GameManager.player;
        var playerController = player.GetComponent<PlayerController>();
        var playerMoveRotate = player.GetComponent<PlayerMoveRotate>();

        print(Time.timeScale);

        playerController.ActiveController(false);
        playerMoveRotate.ToMove(playerMovePos.position, playerMoveTime);

        yield return new WaitForSeconds(playerMoveTime);

        // 보스 등장
        teleportEffect.Play(true);
        yield return new WaitForSeconds(0.2f);
        boss.transform.position = bossSpawn.position;

        // 패턴 실행
        var bossAnimator = boss.GetComponent<Animator>();
        bossAnimator.SetInteger("SkillState", 1);
        bossAnimator.SetInteger("Phase", 0);

        yield return new WaitForSeconds(2.0f);
        // 체력 바 보여주기
        bossHp.SetActive(true);

        playerController.ActiveController(true);
    }
}
