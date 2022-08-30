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

    //�ִϸ��̼� �Լ�
    //�������� ����Ͽ� �����ϰ� ���İ��� ���ÿ� ����
    //�÷��̾ ������ ��׶��� �̹����� ���̵� ������ ��Ÿ��
    //�� �̹����� �� ��Ÿ���� ������ �������� 0.5���� 1�� ����Ǹ鼭 ���̵� ���ϸ鼭 ��Ÿ��
    void OnActive()
    {
        print("���");
        float fgoal = 0.45f;
        Vector3 textScale = new Vector3(fgoal, fgoal, fgoal);
        Sequence suq = DOTween.Sequence();
        suq.Prepend(back_img.DOFade(0.95f, playTime));
        suq.Insert(delayTime-1.5f, frame_img.DOFade(1, 3.0f));
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
