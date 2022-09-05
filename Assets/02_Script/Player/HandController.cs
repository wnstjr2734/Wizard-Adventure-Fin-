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
    [SerializeField] private GameObject ShootDetector;
    [SerializeField] private GameObject MagicWand;
    [SerializeField] private GameObject Firebullet;
    
    private GameObject bulletFactory;

    private void Start()
    {       
        ShootDetector = transform.FindChildRecursive("ShootDetector").gameObject;
        MagicWand = transform.FindChildRecursive("Magic_wand_06").gameObject;
        Firebullet.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    private void Update()
    {
        ShootDetectorSwitch(PlayerController.Instance.CanControlPlayer);            
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            if (basePos != null)
            {
                bulletFactory = Instantiate(Firebullet);
                bulletFactory.transform.position = basePos.position;
                bulletFactory.transform.rotation = Quaternion.LookRotation(basePos.forward);
                bulletFactory.transform.position += bulletFactory.transform.forward * 3.0f * Time.deltaTime;
                Destroy(bulletFactory, 3.0f);
            }
           
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

    void ShootDetectorSwitch(bool ActionMap)
    {
        //Player Action Map 이 Player 면 True, UI 면 False
        bool defultMap = ActionMap ? true : false;
       RightAction ra = ActionMap ? RightAction.WandGrip : RightAction.UiSelect;

        ShootDetector.SetActive(defultMap);
        MagicWand.SetActive(defultMap);
        SetRightHandAction(ra);
    }

}
