using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

/// <summary>
/// 속성 페이지 동작 클래스
/// 속성 선택 창을 회전시켜서 원하는 속성을 선택하게끔 구현
/// </summary>
public class PropertiesWindow : MonoBehaviour
{
    #region 변수
    public GameObject pp_base;                           // 속성 선택을 위한 회전용 UI
    private int[] pp_index = { 0, 1, -1 };                // 속성 인덱스
    private float speed = 0.5f;
    private float currentAngle = 0.0f;
    private int pp_Angle = 0;    
    private float moveAngle = -120f, reversAngle = 120f;    
    private int horizontal;    
    private bool isRotate = false;   
    private CanvasGroup cg;
    ElementType[] et = { ElementType.Fire, ElementType.Ice, ElementType.Lightning };  //속성 마법    
    PlayerMagic magic;
    ElementType pp;
    Vector3 angle;
    Ease ease;
    #endregion

    // Start is called before the first frame update
    private void Start()
    {
        //회전 속도의 변화
        ease = Ease.InOutCubic;
        cg = this.GetComponent<CanvasGroup>();        
        //magic.onChangeElement += ChangeElementAnimation;       
    }

    // Update is called once per frame
    private void Update()
    {
        //좌우 입력값을 -1,0,1 로 받음                 
        StartCoroutine(nameof(IEBaseRotate));        
        
    }

    private void ChangeElementAnimation(ElementType element)
    {
        Debug.Log("속성 변경");
    }
    
    public void OnChangeElement(int input)
    {
        print(input);
        horizontal = input;
    }


    //Axis 키 값을 받으면 Circle이 회전
    IEnumerator IEBaseRotate()
    {       
        float delay = 1.0f;

        if (horizontal == -1 && !isRotate) //왼쪽 회전
        {
            LeftMove();
            pp_Angle += horizontal;
            yield return new WaitForSeconds(delay); //회전하면 delay 시간 동안 입력 막음
            isRotate = false;            
            OnPropertise(0);
        }
        else if (horizontal == 1 && !isRotate) //오른쪽 회전
        {
            RightMove();
            pp_Angle += horizontal; 
            yield return new WaitForSeconds(delay);
            isRotate = false;            
            OnPropertise(0);
        }
        //한바퀴 돌면 속성을 기본속성으로 초기화
        if (pp_Angle > 2 || pp_Angle < -2)
        {
            pp_Angle = 0;
        }

    }

    //속성 선택 창이 왼쪽으로 회전
    private void LeftMove()
    {                
        isRotate = true;
        currentAngle += moveAngle;                                                   //현재 각도를 저장하고 회전각도를 더하므로  Circle을 회전시킴
        angle = new Vector3(0, 0, currentAngle);
        pp_base.transform.DOLocalRotate(angle, speed).SetEase(ease);
        if (currentAngle <= -360f) { currentAngle = 0.0f; }           //회전을 한 바퀴 돌면 각도 초기화
    }

    //속성 선택 창이 오른쪽으로 회전
    private void RightMove()
    {        
        isRotate = true;
        currentAngle += reversAngle; 
        angle = new Vector3(0, 0, currentAngle);
        pp_base.transform.DOLocalRotate(angle, speed).SetEase(ease);
        if (currentAngle >= 360f) { currentAngle = 0.0f; }       
    }

    //속성 선택 함수
    //회전 값을 받아서 ElementType으로 반환
    private void InProperties(int index)
    {
        int pp_num = 0;

        for (int i = 0; i < pp_index.Length; i++)
        {
            if (index != pp_index[i])
            {
                if (index == 2)
                {
                    pp_num = 2;              //속성을 라이트닝으로 변경      
                }
                else if (index == -2)
                {
                    pp_num = 1;              //속성을 아이스로 변경      
                }
            }
            else
            {
                pp_num = i;
            }
        }
        ChangeElementAnimation(et[pp_num]);
        //print("현재 속성 : " + et[pp_num]);
    }  
    
    public void OnPropertise(int alpha)
    {
        cg.DOFade(alpha, 1.0f);
    }

}
