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
    private static WaitForSeconds slowTime;

    [SerializeField, Tooltip("최대 체력")] 
    private int maxHp = 100;
    private int currentHp;

    [SerializeField, Tooltip("공격 속도")] 
    private float attackSpeed = 1.0f;
    [SerializeField, Tooltip("이동 속도")]
    private float moveSpeed = 1.0f;
    
    public event Action<float> onHpChange;
    public event Action onDead;
    // 경직
    public event Action onShocked;

    public int CurrentHp
    {
        get => currentHp;
        private set
        {
            currentHp = value;
            onHpChange?.Invoke((float)currentHp / maxHp);
            if (currentHp <= 0)
            {
                currentHp = 0;
                onDead?.Invoke();
            }
        }
    }

    // 히트 게이지 적용
    // 히트 게이지가 꽉차면 경직을 주고 리셋


    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        CurrentHp = maxHp;
    }

    private void Start()
    {
        if (slowTime != null)
        {
            slowTime = new WaitForSeconds(ElementInfo.Instance.IceInfo.duration);
        }
    }

    /// <summary>
    /// 데미지 및 속성 적용
    /// </summary>
    public void TakeDamage(ElementDamage elementDamage)
    {
        // 데미지 정보 적용
        CurrentHp -= elementDamage.damage;

        // 속성에 따라 이펙트 적용
        switch (elementDamage.elementType)
        {
            case ElementType.Fire:
                break;
            case ElementType.Ice:
                break;
            case ElementType.Lightning:
                break;
        }
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

    private IEnumerator IESlow()
    {
        // 슬로우 효과 적용

        // 이펙트 적용

        yield return slowTime;

        // 속도 리셋
        ClearSlow();
        // 이펙트 해제
    }

    private void ApplySlow()
    {

    }

    private void ClearSlow()
    {
        attackSpeed = 1.0f;
        moveSpeed = 1.0f;
    }
}
