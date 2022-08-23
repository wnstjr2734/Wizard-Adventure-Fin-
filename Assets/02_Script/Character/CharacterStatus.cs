using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 캐릭터(플레이어, 적) 능력치(체력, 공속, 이속 등) 정보
/// 작성자 - 차영철
/// </summary>
[RequireComponent(typeof(Collider))]
public class CharacterStatus : MonoBehaviour
{
    // float는 부동소수점이므로 연산 오차가 발생할 수 있음
    // (1에서 0.1f를 10번 빼도 정확히 0이 되지 않음)
    // 고로 지속시간이 의도치 않게 길어지는 문제를 방지하기 위해 validTime을 정의
    private static readonly float validTime = 0.01f;
    private static readonly float checkTime = 0.1f;
    private static WaitForSeconds ws = new WaitForSeconds(checkTime);
    private static WaitForSeconds slowTime;

    // Event
    public event Action<float> onHpChange;      // 체력 변경
    public event Action onDead;                 // 죽었을 때
    public event Action<float> onSpeedChenge;   // 슬로우
    public event Action onShocked;              // 경직

    [SerializeField, Tooltip("최대 체력")] 
    private int maxHp = 100;
    private int currentHp;
    // 죽은 이후에도 화상 등으로 데미지를 입어 다시 죽는 일을 방지
    private bool isDead = false;


    [SerializeField, Tooltip("속도 계수(공속, 이속)")] 
    private float speedMultiplier = 1.0f;

    // 각 지속효과 별 스택
    private int burnStack = 0;  // 화상
    private int slowStack = 0;  // 슬로우

    private Coroutine burnCoroutine;
    private Coroutine slowCoroutine;


    private GameObject burnEffect;
    private GameObject frozenEffect;
    private GameObject shockEffect;

    private Collider hitCollider;

    #region Properties
    public int CurrentHp
    {
        get => currentHp;
        private set
        {
            if (isDead)
            {
                return;
            }

            currentHp = value;
            onHpChange?.Invoke((float)currentHp / maxHp);
            if (currentHp <= 0)
            {
                currentHp = 0;
                isDead = true;
                onDead?.Invoke();
                hitCollider.enabled = false;
            }
        }
    }

    // 경직 게이지 적용
    // 경직 게이지가 꽉차면 경직을 주고 리셋
    

    public int BurnStack
    {
        get => burnStack;
        private set
        {
            var fireInfo = ElementInfo.Instance.FireInfo;
            burnStack = math.min(value, fireInfo.maxStack);
            // 이펙트
        }
    }

    public int SlowStack
    {
        get => slowStack;
        private set
        {
            var iceInfo = ElementInfo.Instance.IceInfo;
            slowStack = math.min(value, iceInfo.maxStack);

            // 속도 변경
            speedMultiplier = 1 - (iceInfo.initSlowPercent + iceInfo.stackBonusSlowPercent * slowStack) * 0.01f;
            onSpeedChenge?.Invoke(speedMultiplier);

            // 이펙트 변경
            
        }
    }

    #endregion


    private void Awake()
    {
        hitCollider = GetComponent<Collider>();
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
                if (burnCoroutine != null)
                {
                    StopCoroutine(burnCoroutine);
                }
                burnCoroutine = StartCoroutine(IEApplyBurn(elementDamage.stack));
                break;
            case ElementType.Ice:
                if (slowCoroutine != null)
                {
                    StopCoroutine(slowCoroutine);
                }
                slowCoroutine = StartCoroutine(IESlow(elementDamage.stack));
                break;
            case ElementType.Lightning:
                ApplyHitGaugeBonus(elementDamage.damage);
                break;
        }
    }

    private IEnumerator IEApplyBurn(int addedStack)
    {
        BurnStack = burnStack + addedStack;

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
                CurrentHp -= fireInfo.initDamage + fireInfo.stackBonusDamage * burnStack;
                interval = fireInfo.interval;
            }

            yield return checkTime;
        }

        BurnStack = 0;
        burnCoroutine = null;
    }

    private IEnumerator IESlow(int addedStack)
    {
        SlowStack = slowStack + addedStack;
        yield return slowTime;
        
        // 슬로우 스택 리셋
        slowStack = 0;
        slowCoroutine = null;
    }

    private void ApplyHitGaugeBonus(int damage)
    {
        
    }
}
