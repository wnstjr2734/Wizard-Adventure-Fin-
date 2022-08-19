using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 차지 이펙트 기능 구현
/// 작성자 - 차영철
/// </summary>
public class ChargeEffect : MonoBehaviour
{
    // 차지 이펙트는 3단계로 구성
    // 1단계(처음 켰을 때) - 
    private static readonly int ringIndex = 1;
    private static readonly int glowIndex = 2;

    [SerializeField, Tooltip("단계별 차지 정도")]
    private Vector3 chargeScale = new Vector3(0.2f, 0.5f, 1.0f);

    private ParticleSystem[] particleSystems;
    private Light light;

    [SerializeField, Tooltip("불, 얼음, 전기 속성 색상")] 
    private Color[] elementColor = new Color[] {Color.red, Color.cyan, Color.yellow};

    [SerializeField, Tooltip("차지에 필요한 시간")]
    private float chargeTime = 5.0f;

    private void Awake()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        light = GetComponentInChildren<Light>();
        print("particleSystems count : " + particleSystems.Length);
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.one * chargeScale.x;
        particleSystems[ringIndex].gameObject.SetActive(false);
        particleSystems[glowIndex].gameObject.SetActive(false);
        SetColor(PlayerMagic.ElementType.Fire);
        StartCoroutine(nameof(IEChargeGauge));
    }

    private IEnumerator IEChargeGauge()
    {
        float chargePercent = 0.0f;
        float chargeSpeed = 1.0f / chargeTime;

        while (chargePercent < 0.5f)
        {
            chargePercent += Time.deltaTime * chargeSpeed;
            yield return null;
        }
        particleSystems[ringIndex].gameObject.SetActive(true);
        transform.DOScale(Vector3.one * chargeScale.y, 0.5f);

        while (chargePercent < 1.0f)
        {
            chargePercent += Time.deltaTime * chargeSpeed;
            yield return null;
        }
        particleSystems[glowIndex].gameObject.SetActive(true);
        transform.DOScale(Vector3.one * chargeScale.z, 0.5f);
    }

    private void OnDisable()
    {
        
    }

    public void SetColor(PlayerMagic.ElementType element)
    {
        Color particleColor = elementColor[(int) element];

        foreach (var system in particleSystems)
        {
            var main = system.main;
            main.startColor = particleColor;
        }

        light.color = particleColor;
    }
}
