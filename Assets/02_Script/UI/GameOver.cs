using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// ���� ���� â Ȱ�� Ŭ����
/// </summary>
public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject gameOver;
    [SerializeField] Image back_img, frame_img, text_img;
    [SerializeField] float playTime, delayTime;
    [SerializeField]  bool isPlayerdead = false;
    Color[] clr;

    // Start is called before the first frame update
    void Start()
    {
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

    //�ִϸ��̼� �Լ�
    //�������� ����Ͽ� �����ϰ� ���İ��� ���ÿ� ����
    //�÷��̾ ������ ��׶��� �̹����� ���̵� ������ ��Ÿ��
    //�� �̹����� �� ��Ÿ���� ������ �������� 0.5���� 1�� ����Ǹ鼭 ���̵� ���ϸ鼭 ��Ÿ��
    void OnActive()
    {
        print("���");
        Vector3 textScale = new Vector3(1.0f, 1.0f, 1.0f);
        Sequence suq = DOTween.Sequence();
        suq.Prepend(back_img.DOFade(0.9f, playTime));
        suq.Insert(delayTime-1.5f, frame_img.DOFade(1, playTime));
        suq.Insert(delayTime, text_img.DOFade(1, playTime+2.0f));
        suq.Join(text_img.transform.DOScale(textScale, playTime));
       
    }

    //����� ��׶���� ������ ���İ��� 0���� ����
    //������ �������� 0.5�� ����
    //isPlayerDead �� �ް� ����
    void Alpha0(bool dead)
    {
        if (!dead)
        {
            clr = new Color[3];
            clr[0] = back_img.GetComponent<Image>().color;
            clr[1] = frame_img.GetComponent<Image>().color;
            clr[2] = text_img.GetComponent<Image>().color;
            clr[0].a = clr[1].a = clr[2].a = 0.0f;
            back_img.color = clr[0];
            frame_img.color = clr[1];
            text_img.color = clr[2];
            text_img.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
       
    }

}
