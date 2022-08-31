using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 피격 시 피해받는 연출
/// 작성자 - 차영철
/// </summary>
public class PlayerHitManager : MonoBehaviour
{
    [SerializeField, Tooltip("대상 캐릭터")]
    private CharacterStatus player;
    [SerializeField, Tooltip("플레이어 피 상태 보여주는 UI")]
    private Material bloodEffectMat;

    [SerializeField, Tooltip("피 이펙트 최소치 / 최대치")]
    private Vector2 bloodBlendAmounts = new Vector2(0.25f, 0.5f);

    [SerializeField, Tooltip("피 이펙트 보여지는 시간\n" +
                             "각각 이펙트 그리는 시간 / 최대")]
    private Vector3 bloodEffectTimes = new Vector3(0.2f, 0.1f, 0.5f);

    [SerializeField, Range(0, 1)] 
    [Tooltip("피가 얼마 이하로 남았을 때 위급 판정을 할지")]
    private float crisisHpPercent = 0.3f;

    [SerializeField]
    private GameOver gameOver;

    private Coroutine onHitCoroutine;
    private Coroutine onCrisisCoroutine;

    private static readonly int blendAmountID = Shader.PropertyToID("_BlendAmount");

    private void Start()
    {
        player.onHpChange += OnHit;
        player.onDead += OnDead;
    }

    private void OnHit(float percent)
    {
        if (onHitCoroutine != null)
        {
            StopCoroutine(onHitCoroutine);
        }
        onHitCoroutine = StartCoroutine(IEOnHit(percent));
    }

    private IEnumerator IEOnHit(float percent)
    {
        // 위급 상태는 Hit Animation 이후에 작동되어야 한다
        if (onCrisisCoroutine != null)
        {
            StopCoroutine(onCrisisCoroutine);
        }
        onCrisisCoroutine = null;

        float targetBlendAmount = Mathf.Lerp(bloodBlendAmounts.x, bloodBlendAmounts.y, 1 - percent);

        // Fade In
        yield return IEBloodEffectAnim(bloodBlendAmounts.x, targetBlendAmount, bloodEffectTimes.x);

        yield return bloodEffectTimes.y;
        
        yield return IEBloodEffectAnim(targetBlendAmount, bloodBlendAmounts.x, bloodEffectTimes.z);

        onHitCoroutine = null;
        
        if (percent < crisisHpPercent)
        {
            onCrisisCoroutine = StartCoroutine(IEOnCrisis(targetBlendAmount));
        }
    }

    private IEnumerator IEOnCrisis(float targetBlendAmount)
    {
        while (true)
        {
            yield return IEBloodEffectAnim(bloodBlendAmounts.x, targetBlendAmount, bloodEffectTimes.x);
            yield return IEBloodEffectAnim(targetBlendAmount, bloodBlendAmounts.x, bloodEffectTimes.x);
        }
    }

    private IEnumerator IEBloodEffectAnim(float start, float end, float time)
    {
        float timeInv = 1 / time;
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            float blendAmount = Mathf.Lerp(start, end, t * timeInv);
            bloodEffectMat.SetFloat(blendAmountID, blendAmount);
            yield return null;
        }
    }

    private void OnDead()
    {
        if (onHitCoroutine != null)
        {
            StopCoroutine(onHitCoroutine);
        }
        onHitCoroutine = null;

        if (onCrisisCoroutine != null)
        {
            StopCoroutine(onCrisisCoroutine);
        }
        onCrisisCoroutine = null;

        gameOver.OnActive();
    }
}
