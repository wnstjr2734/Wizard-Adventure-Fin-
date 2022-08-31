using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 플레이어 피격 시 피해받는 연출
/// 작성자 - 차영철
/// </summary>
public class PlayerHitManager : MonoBehaviour
{
    [SerializeField]
    private CharacterStatus player;
    [SerializeField, Tooltip("플레이어 피 상태 보여주는 UI")]
    private Material bloodEffectMat;
    [SerializeField]
    private float deadlyBloodAlpha = 0.5f;

    [SerializeField, Tooltip("피 이펙트 보여지는 시간")]
    private float bloodEffectTime = 0.8f;

    // 피가 30% 이하로 남았을 때, 
    private bool isCrisis = false;

    [SerializeField]
    private GameOver gameOver;

    private Sequence bloodEffectSeq;

    private static readonly int blendAmountID = Shader.PropertyToID("_BlendAmount");

    private void Start()
    {
        player.onHpChange += OnHit;
        player.onDead += OnDead;

        bloodEffectSeq = DOTween.Sequence();
    }

    private void OnHit(float percent)
    {
        var blendAmount = 1 - percent;
        bloodEffectMat.SetFloat(blendAmountID, blendAmount);
        bloodEffectMat.DOFloat(0.0f, blendAmountID, bloodEffectTime);
    }





    private void OnDead()
    {
        gameOver.OnActive();
    }
}
