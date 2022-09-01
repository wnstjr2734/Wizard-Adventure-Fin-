using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ʃ�丮�� â�� ��� �����ִ� ������
/// </summary>
public class TutorialWindow : HelpWindow
{
    [SerializeField] private CanvasGroup canvasGroup;

    public void Open(TutorialExplainData[] datas)
    {
        contents = datas;

        SetCurrentIndex(0);
        // ���� ������ ����
        currentViewer.SetContext(datas[currentIndex]);

        // Ʃ�丮�� â�� ���� �� index 0���� �����ؾ� �Ѵ�
        // ���� �������� ���� �������� �̵��� �� �ִ��� Ȯ��
        // Fade In
        canvasGroup.alpha = 0.0f;
        
        Sequence s = DOTween.Sequence();
        s.Append(canvasGroup.DOFade(1.0f, 0.5f)).SetUpdate(true);
        s.onComplete = () => currentViewer.Play();
    }

    public void Close()
    {
        currentViewer.Stop();
        WindowSystem.Instance.CloseWindow(true);
    }
}
