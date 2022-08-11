using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

/// <summary>
/// 튜토리얼 설명 페이지 동작 클래스
/// </summary>
public class TutorialViewer : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private TextMeshProUGUI explainText;
    [SerializeField] private Image controllerImage;

    public void SetContext(TutorialExplainData explainData)
    {
        // Tutorial Data로부터 정보 가져와서 적용하기
        videoPlayer.clip = explainData.clip;
        explainText.text = explainData.explain;
        controllerImage.sprite = explainData.controllerImage;
    }

    public void Play()
    {
        // 영상 처음부터 재생
        videoPlayer.frame = 0;
        videoPlayer.Play();
    }

    public void Stop()
    {
        // 영상 정지
        videoPlayer.Stop();
    }
}
