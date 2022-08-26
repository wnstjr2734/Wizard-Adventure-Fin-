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
    
    [SerializeField, Tooltip("왼손 애니메이션 컨트롤러")]
    private Animator leftHandAnimator;
    // 오른손은 사용 안 함

    // 애니메이션 스트링 해쉬 코드
    private readonly int animNumHash = Animator.StringToHash("AnimNum");

    public void SetLeftHandAction(LeftAction action)
    {
        leftHandAnimator.SetInteger(animNumHash, (int)action);
    }
}
