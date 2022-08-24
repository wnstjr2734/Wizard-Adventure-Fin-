using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 로딩 화면 동작 클래스
/// </summary>
public class LoadingWindow : MonoBehaviour
{
    public static LoadingWindow instance;

    private void Awake()    { instance = this; }

    public TextMeshProUGUI loading_text;
    public GameObject circle;
    private float rotate;
    private string[] now;
    private int index;
    private bool loading_Comp = false;
    

    // Start is called before the first frame update
    void Start()
    {       
        now = new string[] { "",".", "..", "..." };
        CircleRotate();
        StartCoroutine(nameof(IETextCycle));
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    IEnumerator IETextCycle()
    {
        while (index < now.Length)
        {
            TextChange(index);
            yield return new WaitForSeconds(1.0f);
            index++;
            if (index >= now.Length)  {  index = 0;  }
            if (loading_Comp) { break; }
        }

    }


    private void TextChange(int num)
    {
        loading_text.text = "Now Loading" + now[num];     
    }

    private void CircleRotate()
    {
        float speed = 3f;
        Vector3 rot = new Vector3(0, 0, -360);
        RotateMode rotMode = RotateMode.FastBeyond360;
        Ease ease = Ease.Linear;
        LoopType loop = LoopType.Incremental;
        circle.transform.DOLocalRotate(rot, speed, rotMode).SetEase(ease).SetLoops(-1, loop);
    }


}
