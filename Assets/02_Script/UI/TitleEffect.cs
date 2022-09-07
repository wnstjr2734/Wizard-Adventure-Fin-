using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class TitleEffect : MonoBehaviour
{
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject button;
    [SerializeField] private Image circle;
    [SerializeField] private Image text;
    private CanvasGroup cg;
    private Sequence sequence;
    Quaternion rotEnd = Quaternion.Euler(new Vector3(0.0f, 0.0f, -360.0f));
    Vector3 rotEndv = new Vector3(0.0f, 0.0f, -359.0f);

    private void Awake()
    {
        cg = button.GetComponent<CanvasGroup>();
        sequence = DOTween.Sequence();
        circle = transform.FindChildRecursive("Title_Circle").GetComponent<Image>();
        text = transform.FindChildRecursive("Title_Text").GetComponent<Image>();
    }



    // Start is called before the first frame update
    void Start()
    {
        cg.alpha = 0;
        // TitleProduce();
        circle.transform.DOLocalRotate(rotEndv, 3.0f).SetLoops(-1, LoopType.Incremental);
        
    }

    // Update is called once per frame
    void Update()
    {
        print("È¸Àü °ª : " + circle.transform.localRotation);
    }

    void TitleProduce()
    {                
        sequence.Prepend(circle.transform.DOLocalRotate(rotEndv, 3.0f).SetLoops(-1, LoopType.Incremental));
        sequence.Insert(2.0f, text.DOFade(1, 2.0f));
        sequence.Append(cg.DOFade(1, 2.0f));

    }

    void buttonFade()
    {
       
    }

    void circleRotation()
    {

    }

}
