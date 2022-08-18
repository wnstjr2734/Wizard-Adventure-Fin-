using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// ���� ����Ʈ ��� ����
/// �ۼ��� - ����ö
/// </summary>
public class ChargeEffect : MonoBehaviour
{
    // ���� ����Ʈ�� 3�ܰ�� ����
    // 1�ܰ�(ó�� ���� ��) - 

    [SerializeField, Tooltip("�ܰ躰 ���� ����")]
    private Vector3 chargeScale = new Vector3(0.2f, 0.5f, 1.0f);

    private ParticleSystem effect;
    [SerializeField, Tooltip("2�ܰ� Ring Particle")]
    private ParticleSystem ringEffect;
    [SerializeField, Tooltip("3�ܰ� Glow Particle")]
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
