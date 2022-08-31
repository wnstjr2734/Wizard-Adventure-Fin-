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
    [SerializeField, Tooltip("�÷��̾� �� ���� �����ִ� UI")]
    private Material bloodEffectMat;
    [SerializeField]
    private float deadlyBloodAlpha = 0.5f;

    [SerializeField, Tooltip("�� ����Ʈ �������� �ð�")]
    private float bloodEffectTime = 0.8f;

    // �ǰ� 30% ���Ϸ� ������ ��, 
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
