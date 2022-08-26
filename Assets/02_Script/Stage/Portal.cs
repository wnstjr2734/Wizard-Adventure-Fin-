using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//메모장
//해당 위치로 플레이어 이동
//만약 튜토리얼 스테이지 미 클리어 상태라면
//해당 위치로 이동 불가

//코루틴
//화면 페이드아웃
//해당 위치로 플레이어 이동
//화면 페이드 인
/// <summary>
/// 각 스테이지 적 생성 및 적 수에 따른 포탈 활성화 관리
/// 플레이어 포탈이동
/// 작성자 - 이준석
/// </summary>
public class Portal : MonoBehaviour
{

    public CharacterController cc;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private CharacterStatus[] targets;
    [SerializeField]
    public int remainCount;    //남은 적 수
    private bool istrigger;     //트리거 스위치

    [Header("포탈 / 플레이어 스폰 위치")]
    public GameObject portal;
    public Transform portalPoint;   //포탈 탄 후 플레이어 스폰 위치
    
   
    // Start is called before the first frame update
    void Start()
    {
        cc = player.GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player");
        istrigger = false;
        portal.SetActive(false);
        StartRoom();
    }

    // 포탈 시작
    public void StartRoom()
    {
        foreach (var target in targets)
        {
            target.gameObject.SetActive(true);
            target.onDead += DecreaseCount;
        }
        // 남은 몬스터 개수
        remainCount = targets.Length;
        CheckEnd();
    }

    private void DecreaseCount()
    {
        remainCount--;
        CheckEnd();
    }

    private void CheckEnd()
    {
        if (remainCount <= 0)
        {
            Debug.Log("남은적" + remainCount);
            istrigger = true;
            Debug.Log(istrigger);
            // 포탈 활성화
            portal.SetActive(true);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(CcOnOff());
    }

    IEnumerator CcOnOff()
    {
        cc.enabled = false;
        yield return new WaitForSeconds(0.5f);
        if(!cc.enabled)
        {
            player.transform.position = portalPoint.position;
        }
        yield return new WaitForSeconds(0.5f);
        cc.enabled = true;
    }
}
