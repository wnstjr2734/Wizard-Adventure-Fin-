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
    private int[] pp_index = { 0, 1, -1 };                //�Ӽ� �ε���
    private float speed = 1.0f;
    private float currentAngle = 0.0f;
    private int pp_Angle = 0;
    private float moveAngle = -120f, reversAngle = 120f;    
    private int horizontal;    
    private bool isRotate = false;
    private PlayerInput pi;
    ElementType[] et = { ElementType.Fire, ElementType.Ice, ElementType.Lightning };  //�Ӽ� ����
    Vector3 angle;
    Ease ease;
    #endregion

    // Start is called before the first frame update
    private void Start()
    {
        //ȸ�� �ӵ��� ��ȭ
        ease = Ease.InOutCubic;
        pi = PlayerController.Instance.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    private void Update()
    {
        //�¿� �Է°��� -1,0,1 �� ����      
        //horizontal = (int)Input.GetAxisRaw("Horizontal");
        OnChangeElement();
        StartCoroutine(nameof(IEBaseRotate));
        InProperties(pp_Angle);
    }
    
    public void OnChangeElement()
    {
        horizontal = Mathf.RoundToInt(pi.actions["Change Element"].ReadValue<float>());
        print("���� �Է� �� : " + horizontal);
    }


    //Axis Ű ���� ������ Circle�� ȸ��
    IEnumerator IEBaseRotate()
    {       
        float delay = 1.05f;

        if (horizontal == -1 && !isRotate) //���� ȸ��
        {
            LeftMove();
            pp_Angle += horizontal;
            yield return new WaitForSeconds(delay); //ȸ���ϸ� delay �ð� ���� �Է� ����
            isRotate = false;
        }
        else if (horizontal == 1 && !isRotate) //������ ȸ��
        {
            RightMove();
            pp_Angle += horizontal; 
            yield return new WaitForSeconds(delay);
            isRotate = false;
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
    public ElementType InProperties(int index)
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
        //print("���� �Ӽ� : " + et[pp_num]);
        return et[pp_num];        
    }

}
