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

    [SerializeField, Tooltip("단계별 차지 정도")]
    private Vector3 chargeScale = new Vector3(0.2f, 0.5f, 1.0f);

    private ParticleSystem effect;
    [SerializeField, Tooltip("2단계 Ring Particle")]
    private ParticleSystem ringEffect;
    [SerializeField, Tooltip("3단계 Glow Particle")]
    private ParticleSystem glowEffect;

    private void OnEnable()
    {
        transform.localScale = Vector3.one * chargeScale.x;
        ringEffect.gameObject.SetActive(false);
        glowEffect.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        
    }
}
