using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾� �ǰ� �� ���ع޴� ����
/// �ۼ��� - ����ö
/// </summary>
public class PlayerHitManager : MonoBehaviour
{
    [SerializeField, Tooltip("��� ĳ����")]
    private CharacterStatus player;
    [SerializeField, Tooltip("�÷��̾� �� ���� �����ִ� UI")]
    private Material bloodEffectMat;

    [SerializeField, Tooltip("�� ����Ʈ �ּ�ġ / �ִ�ġ")]
    private Vector2 bloodBlendAmounts = new Vector2(0.25f, 0.5f);

    [SerializeField, Tooltip("�� ����Ʈ �������� �ð�\n" +
                             "���� ����Ʈ �׸��� �ð� / �ִ�")]
    private Vector3 bloodEffectTimes = new Vector3(0.2f, 0.1f, 0.5f);

    [SerializeField, Range(0, 1)] 
    [Tooltip("�ǰ� �� ���Ϸ� ������ �� ���� ������ ����")]
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
        // ���� ���´� Hit Animation ���Ŀ� �۵��Ǿ�� �Ѵ�
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
