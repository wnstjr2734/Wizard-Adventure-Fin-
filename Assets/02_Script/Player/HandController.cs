using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 입력에 따라 손 동작 조절 클래스
/// 작성자 - 차영철
/// </summary>
public class HandController : MonoBehaviour
{
    /// <summary>
    /// 왼손 조작
    /// </summary>
    public enum LeftAction
    {
        Default, // 디폴트, 막기
        Teleport, // 텔레포트
    }

    public enum RightAction
    {
        Default,    // 주먹
        UiSelect,   // UI 선택 시 가리키기 액션
        WandGrip,   // 평소 완드 집었을 때
        SwordGrip,  // 얼음검
    }
    
    [SerializeField, Tooltip("왼손 애니메이션 컨트롤러")]
    private Animator leftHandAnimator;
    [SerializeField, Tooltip("오른손 애니메이션 컨트롤러")]
    private Animator rightHandAnimator;

    [Header("플레이어 UI 조작")]
    [SerializeField] private LineRenderer line;  
    [SerializeField] private Transform basePos;    
    [SerializeField] private float maxDis = 30f;
    Ray ray = new Ray();
    

    private RaycastHit hit;

    private void Start()
    {
        ray.origin = basePos.localPosition;
        ray.direction = basePos.forward;        
    }

    private void Update()
    {
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
        {
            Debug.Log(hit.collider.gameObject.name);
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
            print("레이 시작 위치 : " +ray.origin);
            print("기준점 위치 : " + basePos.position);
            line.SetPosition(0, ray.origin);
            line.SetPosition(1, hit.point);
            
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            line.gameObject.SetActive(true);
            print("버튼 클릭");
            
        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            print("버튼 실행");
            line.gameObject.SetActive(false);
        }

    }



    // 애니메이션 스트링 해쉬 코드
    private readonly int animNumHash = Animator.StringToHash("AnimNum");

    public void SetLeftHandAction(LeftAction action)
    {
        leftHandAnimator.SetInteger(animNumHash, (int)action);
    }

    public void SetRightHandAction(RightAction action)
    {
        rightHandAnimator.SetInteger(animNumHash, (int)action);
    }
}
