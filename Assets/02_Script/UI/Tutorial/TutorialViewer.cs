using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

/// <summary>
/// Ʃ�丮�� ���� ������ ���� Ŭ����
/// </summary>
public class TutorialViewer : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private TextMeshProUGUI explainText;
    [SerializeField] private Image controllerImage;

    public void SetContext(TutorialExplainData explainData)
    {
        // Tutorial Data�κ��� ���� �����ͼ� �����ϱ�
        videoPlayer.clip = explainData.clip;
        explainText.text = explainData.explain;
        controllerImage.sprite = explainData.controllerImage;
    }

    public void Play()
    {
        // ���� ó������ ���
        videoPlayer.frame = 0;
        videoPlayer.Play();
    }

    public void Stop()
    {
        // ���� ����
        videoPlayer.Stop();
    }
}
