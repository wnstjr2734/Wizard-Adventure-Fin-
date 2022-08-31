using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 스폰 트리거 되면
//적 공장에서 적 생성
//적 소서러, 궁수, 근접 랜덤 생성
//적 수

/// <summary>
/// 텔레포트 되면 적 활성화
/// </summary>
public class EnemySpawn : MonoBehaviour
{
    public Portal portal;
    private bool isTriggered = false;

    [SerializeField]
    private TutorialWindow window;
    [SerializeField, Tooltip("방 이동했을 때 띄울 튜토리얼 창 정보")]
    private TutorialExplainData tutorialData;
    [SerializeField, Tooltip("배울 기능")]
    private PlayerController.MagicAbility[] learnAbility;

    private void OnTriggerEnter(Collider other)
    {
        if(!isTriggered && other.name.Contains("Player"))
        {
            portal.StartRoom();
            isTriggered = true;

            GameManager.Instance.SetCheckPoint(transform.position, portal);
            // 튜토리얼 활성화
            if (tutorialData != null)
            {
                WindowSystem.Instance.OpenWindow(window.gameObject, true);
                window.Open(tutorialData);
            }
        }
    }
}