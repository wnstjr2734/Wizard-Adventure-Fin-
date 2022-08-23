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
    public float rotate;
    Text text;    
    int index;
    

    // Start is called before the first frame update
    void Start()
    {
        loading_text = GetComponent<TextMeshProUGUI>();
       
    }

    // Update is called once per frame
    void Update()
    {
        TextChange(index);
    }

    void TextChange(int num)
    {
        float time = 1f;
        string load = "Now Loading...";
        text.DOText(load, time);

    }

    void CircleRotate()
    {
        
    }


}
