using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 캐릭터(플레이어, 적) 능력치(체력, 공속, 이속 등) 정보
/// 작성자 - 차영철
/// </summary>
public class CharacterStatus : MonoBehaviour
{
    // float는 부동소수점이므로 연산 오차가 발생할 수 있음
    // (1에서 0.1f를 10번 빼도 정확히 0이 되지 않음)
    // 고로 지속시간이 의도치 않게 길어지는 문제를 방지하기 위해 validTime을 정의
    private static readonly float validTime = 0.01f;
    private static readonly float checkTime = 0.1f;
    private static WaitForSeconds ws = new WaitForSeconds(checkTime);

    [SerializeField, Tooltip("최대 체력")] 
    private float maxHp = 100;

    [SerializeField, Tooltip("")] 
    private float attackSpeed = 1.0f;
    

    // 외부에서 값을 수정할 수 있고, 변수 사용을 추적할 수 있도록 property를 제공
    public float CurrentHp { get; set; }

    public event Action<float> onHpChange;


    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        CurrentHp = maxHp;
    }

    private IEnumerator IEApplySlow()
    {
        // 슬로우 효과 적용

        float remainTime = ElementInfo.Instance.FireInfo.duration;
        while (remainTime > validTime)
        {

            yield return checkTime;
        }

        // 속도 리셋
    }

    private void ResetSpeed()
    {

    }

    private IEnumerator IEApplyBurn()
    {
        var fireInfo = ElementInfo.Instance.FireInfo;

        float remainTime = fireInfo.duration;
        float interval = fireInfo.interval;
        while (remainTime > validTime)
        {
            remainTime -= checkTime;
            interval -= checkTime;

            if (interval < validTime)
            {
                // 화상 데미지 입히기
                interval = fireInfo.interval;
            }

            yield return checkTime;
        }
    }

    /// <summary>
    /// 데미지 및 속성 적용
    /// </summary>
    public void TakeDamage(ElementDamage elementDamage)
    {
        // 데미지 정보 적용

        // 속성에 따라 이펙트 적용
        
    }
}
