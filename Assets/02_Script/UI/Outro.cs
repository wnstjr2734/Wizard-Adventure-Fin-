using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using DG.Tweening;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임 엔딩 재생 및 타이틀로 돌아가는 기능
/// 작성자 - 차영철
/// </summary>
public class Outro : MonoBehaviour
{

    [SerializeField, Tooltip("엔딩 영상 출력 비디오")]
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
        // 게임이 끝났으면 조작 불가능
        controller.ActiveController(false);
    }

    private void ReturnTitle(VideoPlayer vp)
    {
        SceneManager.LoadScene(0);
    }
}
