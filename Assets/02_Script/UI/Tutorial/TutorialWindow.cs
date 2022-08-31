using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// 튜토리얼 창만 열어서 보여주는 윈도우
/// </summary>
public class TutorialWindow : Singleton<TutorialWindow>
{
    [SerializeField] private TutorialViewer viewer;
    [SerializeField] private CanvasGroup canvasGroup;

    public void Open(TutorialExplainData data)
    {
        if (Time.timeScale < 0.01f)
        {
            print("game stopped");
        }
        viewer.SetContext(data);
        // Fade In
        canvasGroup.alpha = 0.0f;
        
        Sequence s = DOTween.Sequence();
        s.Append(canvasGroup.DOFade(1.0f, 0.5f)).SetUpdate(true);
        s.onComplete = () => viewer.Play();
    }

    public void Close()
    {
        viewer.Stop();
        WindowSystem.Instance.CloseWindow(true);
    }
}
