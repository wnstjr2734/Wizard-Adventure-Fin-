using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Video;

public class Intro : MonoBehaviour
{
    [SerializeField, Tooltip("ȭ�� ���� �г�")]
    private GameObject panel;
    public VideoPlayer vid;
    public GameObject rawImage;
    [Tooltip("��Ʈ�ΰ� ���� �� �÷��̾ �̵��� ��ġ")]
    public Transform changePos;

    IEnumerator Start()
    {
        vid.loopPointReached += CheckOver;
        // Ÿ Ŭ���� �ʱ�ȭ�� ���� ���
        yield return null;
        WindowSystem.Instance.OpenWindow(panel, true);
    }

    private void FadeIn()
    {
        Sequence s = DOTween.Sequence();
        s.SetUpdate(true);
        s.Append(GetComponent<CanvasGroup>().DOFade(0, 2f));
        s.onComplete = () =>
        {
            WindowSystem.Instance.CloseWindow(true);
            PlayerChangePos();
        };
    }

    private void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        //print("Video Is Over");
        rawImage.SetActive(false);
        FadeIn();
    }

    private void PlayerChangePos()
    {
        var playerMove = GameManager.player.GetComponent<PlayerMoveRotate>();
        playerMove.SetPos(changePos.transform.position);
    }
}