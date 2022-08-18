using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 스킬 페이지 동작
/// </summary>
public class PropertiesWindow : MonoBehaviour
{
    public GameObject pp_base;
    public float speed = 2f;
    public float horizontal;
    public float currentAngle = 0.0f;
    Vector3 angle;
    public Ease ease;    
    float moveAngle = -120f, reversAngle = 120f;
    bool isLeftMove = false;
    bool isRightMove = false;

    // Start is called before the first frame update
    void Start()
    {       
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        BaseRotate();       
    }


    void BaseRotate()
    {
        //if (pp_base.transform.rotation.z >= angle.z) { isRotate = false; }

        if (horizontal == -1 )
        {
            if (!isLeftMove)
            {  LeftMove(); }
        }
        else if (horizontal == 1 )
        {
            if (!isRightMove)
            {  RightMove(); }
        }
        else
        {
            isLeftMove = false;
            isRightMove = false;
        }
    }

    void LeftMove()
    {        
        print("왼쪽");
        isLeftMove = true;
        currentAngle += moveAngle;
        angle = new Vector3(0, 0, currentAngle);
        pp_base.transform.DORotate(angle, speed).SetEase(ease);
        if (currentAngle <= -360f) { currentAngle = 0.0f; }
    }

    void RightMove()
    {
        print("오른쪽");
        isRightMove = true;
        currentAngle += reversAngle;
        angle = new Vector3(0, 0, currentAngle);
        pp_base.transform.DORotate(angle, speed).SetEase(ease);
        if (currentAngle >= 360f) { currentAngle = 0.0f; }
        //if (pp_base.transform.rotation.z == angle.z) { isRotate = false; }
    }

}
