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
    [SerializeField, Tooltip("방에 있는 몬스터들")]
    private CharacterStatus[] targets;
    private Vector3[] initPos;
    [SerializeField]
    public int remainCount;    //남은 적 수
    private bool istrigger;     //트리거 스위치

    [Header("포탈 / 플레이어 스폰 위치")]
    public GameObject portal;
    public Collider portalCol;
    public Transform portalPoint;   //포탈 탄 후 플레이어 스폰 위치

    private void Awake()
    {
        initPos = new Vector3[targets.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            initPos[i] = targets[i].transform.position;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        portalCol = GetComponent<BoxCollider>();
        portalCol.enabled = false;
        cc = player.GetComponent<CharacterController>();
        istrigger = false;
        portal.SetActive(false);
    }

    /// <summary>
    /// 방을 초기화한다
    /// </summary>
    public void ResetRoom()
    {
        remainCount = targets.Length;
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].gameObject.SetActive(true);

            // 몬스터 상태 리셋
            targets[i].ResetStatus();
            
            // 몬스터 위치 리셋
            targets[i].transform.position = initPos[i];
        }

        // 플레이어와 몬스터가 같이 전멸했을 때 포탈 열림
        // 열린 포탈도 리셋해야한다
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
        Debug.Log("남은적" + remainCount);
        CheckEnd();
    }

    private void CheckEnd()
    {
        if (remainCount <= 0)
        {
            istrigger = true;
            portalCol.enabled = true;
            Debug.Log(istrigger);
            // 포탈 활성화
            portal.SetActive(true);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(CcOnOff());
    }

    private IEnumerator CcOnOff()
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