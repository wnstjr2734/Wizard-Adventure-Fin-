using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Video;

public class Intro : MonoBehaviour
{
    public VideoPlayer vid;
    public GameObject rawImage;
    private CharacterController cc;
    private GameObject player;
    [Tooltip("인트로가 끝난 후 플레이어가 이동할 위치")]
    public Transform changePos;


    // Start is called before the first frame update
    void Start()
    {
        vid.loopPointReached += CheckOver;
        player = GameObject.FindGameObjectWithTag("Player");
        cc = player.GetComponent<CharacterController>();
    }

    private void FadeIn()
    {
        GetComponent<CanvasGroup>().DOFade(0, 2f);
    }

    private void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        //print("Video Is Over");
        rawImage.SetActive(false);
        StartCoroutine(nameof(IEPlayerChangePos));
        FadeIn();
    }

    private void PlayerChangePos()
    {
        player.transform.position = changePos.transform.position;
    }

    IEnumerator IEPlayerChangePos()
    {
        Debug.Log("StartCoroutine");
        yield return new WaitForSeconds(2.0f);
        Debug.Log("StartCoroutineAfter2.0Sec");
        PlayerChangePos();
    }
}