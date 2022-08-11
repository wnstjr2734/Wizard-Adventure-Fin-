using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;
using TMPro;

/// <summary>
/// Ʃ�丮�� ���� ������ ���� Ŭ����
/// </summary>
public class TutorialViewer : MonoBehaviour
{
    [SerializeField] private TutorialExplainData data;

    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private TextMeshProUGUI explainText;
    [SerializeField] private Image controllerImage;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Open()
    {
        // Fade In
        canvasGroup.alpha = 0.0f;
        canvasGroup.DOFade(1.0f, 0.5f);

    }

    public void Play(TutorialExplainData explainData)
    {
        // Tutorial Data�κ��� ���� �����ͼ� �����ϱ�
        videoPlayer.clip = explainData.clip;
        explainText.text = explainData.explain;
        controllerImage.sprite = explainData.controllerImage;

        // ���� ó������ ���
        videoPlayer.frame = 0;
        videoPlayer.Play();
    }

    public void Stop()
    {
        // ���� ����
        videoPlayer.Stop();
    }

    public void Close()
    {
        // ���� ����
        videoPlayer.Stop();

        // Fade Out
        canvasGroup.DOFade(0.0f, 0.5f);
    }
}
