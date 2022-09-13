using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


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
/// 0912 추가 : audioSource 및 포탈 탈때 fadeInOut UI관리
/// </summary>
public class Portal : MonoBehaviour
{
    protected GameObject player;
    [SerializeField, Tooltip("방에 있는 몬스터들")]
    protected CharacterStatus[] targets;
    protected Vector3[] initPoses;
    protected int remainCount;    //남은 적 수
    protected bool canUse = false;     //트리거 스위치

    [Header("포탈 / 플레이어 스폰 위치")]
    public GameObject portal;
    protected Collider portalCol;
    public EnemySpawn portalPoint;   //포탈 탄 후 플레이어 스폰 위치

    [SerializeField, Tooltip("포탈 탈 때 사운드")]
    private AudioSource portalSound;

    [SerializeField, Tooltip("활성화할 다음 방")]
    private GameObject nextRoom;
    [SerializeField, Tooltip("비활성화할 이전 방")]
    private GameObject currentRoom;

    [Tooltip("포탈 탈 때 fade In Out 관리")]
    public GameObject fadeImage;
    public CanvasGroup canvasGroup; //페이드 인아웃 캔버스

    private void Awake()
    {
        initPoses = new Vector3[targets.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            initPoses[i] = targets[i].transform.position;
        }

        //포탈 사운드
        portalSound = GetComponent<AudioSource>();
        portalCol = GetComponent<BoxCollider>();
        portalCol.enabled = false;
        portalSound.enabled = false;
        canUse = false;
        portal = transform.GetChild(0).gameObject;
        portal.SetActive(false);

        //페이드인아웃 오브젝트 초기 비활성화
        fadeImage.SetActive(false);

        var room = transform.parent;
        currentRoom = room.gameObject;

        var stage = room.parent;
        int sibilingIndex = room.GetSiblingIndex();
        if (sibilingIndex < stage.childCount - 1)
        {
            nextRoom = stage.GetChild(sibilingIndex + 1).gameObject;
        }
    }

    protected void Start()
    {
        player = GameManager.player;
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
            targets[i].GetComponent<EnemyFSM>().ResetFSM();
            
            // 몬스터 위치 리셋
            targets[i].transform.position = initPoses[i];
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
        CheckEnd();
    }

    private void CheckEnd()
    {
        if (remainCount <= 0)
        {
            canUse = true;
            portalCol.enabled = true;
            // 포탈 활성화
            portal.SetActive(true);
        }
    }
    
    protected void OnTriggerEnter(Collider other)
    {
        if (canUse)
        {
            // 포탈 중복 사용 안 되게 막기
            canUse = false;
            OnUsePortal();
        }
    }

    /// <summary>
    /// 강제 포탈 사용 기능 (디버그용)
    /// </summary>
    public void UsePortal()
    {
        print("Debug : Forced Use Portal");
        OnUsePortal();
    }

    protected virtual void OnUsePortal()
    {
        StartCoroutine(IEUsePortal());
    }

    //페이드 인아웃 관리 함수
    private void Fade()
    {
        fadeImage.SetActive(true);

        DOTween.Sequence()
            .Append(canvasGroup.DOFade(1.0f, 3f))   //점점 노랗게 페이드아웃
            .Append(canvasGroup.DOFade(0.0f, 3f))   //점점 밝아짐 페이드인
            .OnComplete(() =>                       //완료 후에는 canvasgroup, Canvas 오브젝트 비활성화
            {
                fadeImage.SetActive(false);
            });
    }

    private IEnumerator IEUsePortal()
    {
        //포탈 사운드 작동 - 작성자 이준석
        portalSound.enabled = true;
        yield return new WaitForSeconds(0.1f);
        Fade();
        yield return new WaitForSeconds(5f);
        // 포탈 이동 연출
        nextRoom.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        // 방 이동
        player.GetComponent<PlayerMoveRotate>().SetPos(portalPoint.transform.position, portalPoint.transform.forward);
        // 이전 방은 SetActive(false) 처리

        // 포탈 타고 난 이후 Fade Out 되면서 다음 방 넘어가는 연출
        yield return new WaitForSeconds(0.5f);
        currentRoom.SetActive(false);
    }
}