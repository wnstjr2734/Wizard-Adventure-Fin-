using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Enemy 체력바 슬라이더로 표시
/// 작성자 - 성종현
/// </summary>
public class HP_Slider : MonoBehaviour
{
    public int maxHP = 2;
    int hp;
    public Slider sliderHP;

    public int HP
    {
        get { return hp; }
        set
        {
            hp = value;
            sliderHP.value = hp;
        }
    }
    void Start()
    {
        sliderHP.maxValue = maxHP;
        HP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
