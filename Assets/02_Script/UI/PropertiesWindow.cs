using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

/// <summary>
/// �Ӽ� ������ ���� Ŭ����
/// �Ӽ� ���� â�� ȸ�����Ѽ� ���ϴ� �Ӽ��� �����ϰԲ� ����
/// </summary>
public class PropertiesWindow : MonoBehaviour
{
    #region ����
    public GameObject pp_base;                           // �Ӽ� ������ ���� ȸ���� UI
    private int[] pp_index = { 0, 1, -1 };                // �Ӽ� �ε���
    private float speed = 0.5f;
    private float currentAngle = 0.0f;
    private int pp_Angle = 0;    
    private float moveAngle = -120f, reversAngle = 120f;    
    private int horizontal;    
    private bool isRotate = false;   
    private CanvasGroup cg;
    ElementType[] et = { ElementType.Fire, ElementType.Ice, ElementType.Lightning };  //�Ӽ� ����    
    PlayerMagic magic;
    ElementType pp;
    Vector3 angle;
    Ease ease;
    #endregion

    // Start is called before the first frame update
    private void Start()
    {
        //ȸ�� �ӵ��� ��ȭ
        ease = Ease.InOutCubic;
        cg = this.GetComponent<CanvasGroup>();        
        //magic.onChangeElement += ChangeElementAnimation;       
    }

    // Update is called once per frame
    private void Update()
    {
        //�¿� �Է°��� -1,0,1 �� ����                 
        StartCoroutine(nameof(IEBaseRotate));        
        
    }

    private void ChangeElementAnimation(ElementType element)
    {
        Debug.Log("�Ӽ� ����");
    }
    
    public void OnChangeElement(int input)
    {
        print(input);
        horizontal = input;
    }


    //Axis Ű ���� ������ Circle�� ȸ��
    IEnumerator IEBaseRotate()
    {       
        float delay = 1.0f;

        if (horizontal == -1 && !isRotate) //���� ȸ��
        {
            LeftMove();
            pp_Angle += horizontal;
            yield return new WaitForSeconds(delay); //ȸ���ϸ� delay �ð� ���� �Է� ����
            isRotate = false;            
            OnPropertise(0);
        }
        else if (horizontal == 1 && !isRotate) //������ ȸ��
        {
            RightMove();
            pp_Angle += horizontal; 
            yield return new WaitForSeconds(delay);
            isRotate = false;            
            OnPropertise(0);
        }
        //�ѹ��� ���� �Ӽ��� �⺻�Ӽ����� �ʱ�ȭ
        if (pp_Angle > 2 || pp_Angle < -2)
        {
            pp_Angle = 0;
        }

    }

    //�Ӽ� ���� â�� �������� ȸ��
    private void LeftMove()
    {                
        isRotate = true;
        currentAngle += moveAngle;                                                   //���� ������ �����ϰ� ȸ�������� ���ϹǷ�  Circle�� ȸ����Ŵ
        angle = new Vector3(0, 0, currentAngle);
        pp_base.transform.DOLocalRotate(angle, speed).SetEase(ease);
        if (currentAngle <= -360f) { currentAngle = 0.0f; }           //ȸ���� �� ���� ���� ���� �ʱ�ȭ
    }

    //�Ӽ� ���� â�� ���������� ȸ��
    private void RightMove()
    {        
        isRotate = true;
        currentAngle += reversAngle; 
        angle = new Vector3(0, 0, currentAngle);
        pp_base.transform.DOLocalRotate(angle, speed).SetEase(ease);
        if (currentAngle >= 360f) { currentAngle = 0.0f; }       
    }

    //�Ӽ� ���� �Լ�
    //ȸ�� ���� �޾Ƽ� ElementType���� ��ȯ
    private void InProperties(int index)
    {
        int pp_num = 0;

        for (int i = 0; i < pp_index.Length; i++)
        {
            if (index != pp_index[i])
            {
                if (index == 2)
                {
                    pp_num = 2;              //�Ӽ��� ����Ʈ������ ����      
                }
                else if (index == -2)
                {
                    pp_num = 1;              //�Ӽ��� ���̽��� ����      
                }
            }
            else
            {
                pp_num = i;
            }
        }
        ChangeElementAnimation(et[pp_num]);
        //print("���� �Ӽ� : " + et[pp_num]);
    }  
    
    public void OnPropertise(int alpha)
    {
        cg.DOFade(alpha, 1.0f);
    }

}
