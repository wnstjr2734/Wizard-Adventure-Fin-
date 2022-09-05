using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using DG.Tweening;
using UnityEngine.SceneManagement;

/// <summary>
/// ���� ���� ��� �� Ÿ��Ʋ�� ���ư��� ���
/// �ۼ��� - ����ö
/// </summary>
public class Outro : MonoBehaviour
{

    [SerializeField, Tooltip("���� ���� ��� ����")]
    private VideoPlayer endingPlayer;
    public GameObject rawImage;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        Sequence s = DOTween.Sequence();
        s.Append(canvasGroup.DOFade(1, 2.0f));
        s.onComplete = () => {
            endingPlayer.Play();
        };

        endingPlayer.loopPointReached += ReturnTitle;
        var controller = GameManager.Controller;
        // ������ �������� ���� �Ұ���
        controller.ActiveController(false);
    }

    private void ReturnTitle(VideoPlayer vp)
    {
        SceneManager.LoadScene(0);
    }
}
