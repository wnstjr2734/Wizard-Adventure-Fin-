using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �÷��̾� �ǰ� �� ���ع޴� ����
/// �ۼ��� - ����ö
/// </summary>
public class PlayerHitManager : MonoBehaviour
{
    [SerializeField]
    private CharacterStatus player;
    [SerializeField]
    private Image bloodImage;
    [SerializeField]
    private float deadlyBloodAlpha = 0.5f;

    [SerializeField]
    private GameOver gameOver;

    private void Start()
    {
        player.onHpChange += OnHit;
        player.onDead += OnDead;
    }

    private void OnHit(float percent)
    {
        var color = bloodImage.color;
        color.a = deadlyBloodAlpha * percent;
        bloodImage.color = color;

        bloodImage.DOFade(1, 0.2f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
    }

    private void OnDead()
    {
        gameOver.OnActive();
    }
}
