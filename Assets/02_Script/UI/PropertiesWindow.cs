using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 속성 페이지 동작 클래스
/// </summary>
public class PropertiesWindow : MonoBehaviour
{

    public GameObject pp_base;
    private float speed = 1.0f;
    private float currentAngle = 0.0f;
    private float moveAngle = -120f, reversAngle = 120f;
    private float horizontal;
    private bool isRotate = false;    
    Vector3 angle;
    Ease ease;

    // Start is called before the first frame update
    private void Start()
    {
        //회전 속도의 변화
        ease = Ease.InOutCubic; 
    }

    // Update is called once per frame
    private void Update()
    {
        //좌우 입력값을 -1,0,1 로 받음
        horizontal = Input.GetAxisRaw("Horizontal");
        StartCoroutine(nameof(IEBaseRotate));  
    }

    //Axis 키 값을 받으면 Circle이 회전
    IEnumerator IEBaseRotate()
    {       
        float delay = 1.05f;

        if (horizontal == -1 && !isRotate)
        {
            LeftMove();
            yield return new WaitForSeconds(delay); //회전하면 delay 시간 동안 입력 막음
            isRotate = false;
        }
        else if (horizontal == 1 && !isRotate)
        {
            RightMove();
            yield return new WaitForSeconds(delay);
            isRotate = false;
        }
    }

    //속성 선택 창이 왼쪽으로 회전
    private void LeftMove()
    {        
        print("왼쪽");
        isRotate = true;
        currentAngle += moveAngle;                                                         //현재 각도를 저장하고 회전각도를 더하므로  Circle을 회전시킴
        angle = new Vector3(0, 0, currentAngle);
        pp_base.transform.DOLocalRotate(angle, speed).SetEase(ease);
        if (currentAngle <= -360f) { currentAngle = 0.0f; }                 //회전을 한 바퀴 돌면 각도 초기화
    }

    //속성 선택 창이 오른쪽으로 회전
    private void RightMove()
    {
        print("오른쪽");
        isRotate = true;
        currentAngle += reversAngle; 
        angle = new Vector3(0, 0, currentAngle);
        pp_base.transform.DOLocalRotate(angle, speed).SetEase(ease);
        if (currentAngle >= 360f) { currentAngle = 0.0f; }
       
    }

}
