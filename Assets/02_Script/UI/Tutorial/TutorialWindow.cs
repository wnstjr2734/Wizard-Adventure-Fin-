using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// Ʃ�丮�� â�� ��� �����ִ� ������
/// </summary>
public class TutorialWindow : Singleton<TutorialWindow>
{
    [SerializeField] private TutorialViewer viewer;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TutorialExplainData data;


    private void OnEnable()
    {
        viewer.SetContext(data);
        // Fade In
        canvasGroup.alpha = 0.0f;
        //Sequence s
        DOTween.defaultTimeScaleIndependent = true;
        DOTween.timeScale = 1.0f;
        Sequence s = DOTween.Sequence();
        s.Append(canvasGroup.DOFade(1.0f, 0.5f));
        s.onComplete = () => viewer.Play();
    }

    private void OnDisable()
    {
        viewer.Stop();
    }
}
