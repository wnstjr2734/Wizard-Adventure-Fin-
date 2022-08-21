using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 위치로 플레이어 이동
//만약 튜토리얼 스테이지 미 클리어 상태라면
//해당 위치로 이동 불가

//코루틴
//화면 페이드아웃
//해당 위치로 플레이어 이동
//화면 페이드 인
public class Portal : MonoBehaviour
{
    
    [SerializeField] private GameObject player;
    public Transform portalPoint;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player.transform.position = portalPoint.position;
        }
        
    }
}
