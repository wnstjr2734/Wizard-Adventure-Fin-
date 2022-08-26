using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �Ӽ� ������ ���� Ŭ����
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
        //ȸ�� �ӵ��� ��ȭ
        ease = Ease.InOutCubic; 
    }

    // Update is called once per frame
    private void Update()
    {
        //�¿� �Է°��� -1,0,1 �� ����
        horizontal = Input.GetAxisRaw("Horizontal");
        StartCoroutine(nameof(IEBaseRotate));  
    }

    //Axis Ű ���� ������ Circle�� ȸ��
    IEnumerator IEBaseRotate()
    {       
        float delay = 1.05f;

        if (horizontal == -1 && !isRotate)
        {
            LeftMove();
            yield return new WaitForSeconds(delay); //ȸ���ϸ� delay �ð� ���� �Է� ����
            isRotate = false;
        }
        else if (horizontal == 1 && !isRotate)
        {
            RightMove();
            yield return new WaitForSeconds(delay);
            isRotate = false;
        }
    }

    //�Ӽ� ���� â�� �������� ȸ��
    private void LeftMove()
    {        
        print("����");
        isRotate = true;
        currentAngle += moveAngle;                                                         //���� ������ �����ϰ� ȸ�������� ���ϹǷ�  Circle�� ȸ����Ŵ
        angle = new Vector3(0, 0, currentAngle);
        pp_base.transform.DOLocalRotate(angle, speed).SetEase(ease);
        if (currentAngle <= -360f) { currentAngle = 0.0f; }                 //ȸ���� �� ���� ���� ���� �ʱ�ȭ
    }

    //�Ӽ� ���� â�� ���������� ȸ��
    private void RightMove()
    {
        print("������");
        isRotate = true;
        currentAngle += reversAngle; 
        angle = new Vector3(0, 0, currentAngle);
        pp_base.transform.DOLocalRotate(angle, speed).SetEase(ease);
        if (currentAngle >= 360f) { currentAngle = 0.0f; }
       
    }

}
