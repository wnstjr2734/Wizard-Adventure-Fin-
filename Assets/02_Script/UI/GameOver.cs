using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 게임 오버 창 활성 클래스
/// </summary>
public class GameOver : MonoBehaviour
{    
    [SerializeField] Image back_img, frame_img, text_img;
    public bool isPlayerdead = false;
    float playTime, delayTime;
    Color[] clr;

    // Start is called before the first frame update
    void Start()
    {
        playTime = delayTime = 3.0f;
        //gameOver.SetActive(false);
        Alpha0(isPlayerdead);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            isPlayerdead = true;
        }

        if (isPlayerdead)
        {
           // gameOver.SetActive(true);
            OnActive();
            isPlayerdead = false;
        }
        if (!isPlayerdead)
        {
            //Alpha0();
        }
        
    }

    //애니메이션 함수
    //시퀸스를 사용하여 스케일과 알파값을 동시에 조절
    //플레이어가 죽으면 백그라운드 이미지가 페이드 인으로 나타남
    //백 이미지가 다 나타나면 문구의 스케일을 0.5에서 1로 변경되면서 페이드 인하면서 나타남
    void OnActive()
    {
        print("재생");
        float fgoal = 0.45f;
        Vector3 textScale = new Vector3(fgoal, fgoal, fgoal);
        Sequence suq = DOTween.Sequence();
        suq.Prepend(back_img.DOFade(0.95f, playTime));
        suq.Insert(delayTime-1.5f, frame_img.DOFade(1, 3.0f));
        suq.Insert(delayTime, text_img.DOFade(1, playTime+2.0f));
        suq.Join(text_img.transform.DOScale(textScale, playTime));
       
    }

    //실행시 백그라운드와 문구는 알파값이 0으로 설정
    //문구의 스케일은 0.5로 설정
    //isPlayerDead 값 받고 실행
    void Alpha0(bool dead)
    {
        if (!dead)
        {
            float dgoal = 0.2f;
            clr = new Color[3];
            clr[0] = back_img.GetComponent<Image>().color;
            clr[1] = frame_img.GetComponent<Image>().color;
            clr[2] = text_img.GetComponent<Image>().color;
            clr[0].a = clr[1].a = clr[2].a = 0.0f;
            back_img.color = clr[0];
            frame_img.color = clr[1];
            text_img.color = clr[2];
            text_img.transform.localScale = new Vector3(dgoal, dgoal, dgoal);
        }
       
    }

}
