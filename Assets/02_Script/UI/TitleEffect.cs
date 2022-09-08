using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 타이틀 문구 연출을 위한 클래스
/// 작성자 - 김도영
/// </summary>

public class TitleEffect : MonoBehaviour
{
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject button;
    [SerializeField] private Image circle;
    [SerializeField] private Image text;
    public Button[] btn = new Button[3];
    private CanvasGroup cg;
    private Sequence sequence;
    private Color color;
    private float speed;
    Vector3 rotEndv = new Vector3(0.0f, 0.0f, -180.0f);

    private void Awake()
    {
        StartEffect();
    }



    // Start is called before the first frame update
    void Start()
    {        
        TitleProduce();        
    }

    // Update is called once per frame
    void Update()
    {
        circleRotation();
    }

    //DoTween을 사용한 연출 
    void TitleProduce()
    {                        
        sequence.Prepend(text.DOFade(1, 2.0f)).SetDelay(2.0f);
        sequence.Append(cg.DOFade(1, 2.0f));
    }

    //마법진 회전 함수
    void circleRotation()
    {
        circle.transform.Rotate(rotEndv * speed * Time.deltaTime);     
        
    }

    //연출을 위한 요소
    void StartEffect()
    {
        cg = button.GetComponent<CanvasGroup>();
        sequence = DOTween.Sequence();
        circle = transform.FindChildRecursive("Title_Circle").GetComponent<Image>();
        text = transform.FindChildRecursive("Title_Text").GetComponent<Image>();
        cg.alpha = 0;
        color = text.color;
        color.a = 0;
        text.color = color;
        speed = 0.3f;
    }


}
