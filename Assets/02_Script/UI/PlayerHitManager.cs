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
    [SerializeField]
    private Image bloodImage;

    private void Start()
    {
        player.onHpChange += OnHit;
    }

    private void OnHit(float percent)
    {
        bloodImage.DOFade(1, 0.2f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
    }

    private void OnDead()
    {

    }
}
