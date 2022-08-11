using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;
using TMPro;

/// <summary>
/// 튜토리얼 설명 페이지 동작 클래스
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
        // Tutorial Data로부터 정보 가져와서 적용하기
        videoPlayer.clip = explainData.clip;
        explainText.text = explainData.explain;
        controllerImage.sprite = explainData.controllerImage;

        // 영상 처음부터 재생
        videoPlayer.frame = 0;
        videoPlayer.Play();
    }

    public void Stop()
    {
        // 영상 정지
        videoPlayer.Stop();
    }

    public void Close()
    {
        // 영상 정지
        videoPlayer.Stop();

        // Fade Out
        canvasGroup.DOFade(0.0f, 0.5f);
    }
}
