using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Է¿� ���� �� ���� ���� Ŭ����
/// �ۼ��� - ����ö
/// </summary>
public class HandController : MonoBehaviour
{
    /// <summary>
    /// �޼� ����
    /// </summary>
    public enum LeftAction
    {
        Default, // ����Ʈ, ����
        Teleport, // �ڷ���Ʈ
    }

    public enum RightAction
    {
        Default,    // �ָ�
        UiSelect,   // UI ���� �� ����Ű�� �׼�
        WandGrip,   // ��� �ϵ� ������ ��
        SwordGrip,  // ������
    }
    
    [SerializeField, Tooltip("�޼� �ִϸ��̼� ��Ʈ�ѷ�")]
    private Animator leftHandAnimator;
    [SerializeField, Tooltip("������ �ִϸ��̼� ��Ʈ�ѷ�")]
    private Animator rightHandAnimator;

    [Header("�÷��̾� UI ����")]
    [SerializeField] private LineRenderer line;  
    [SerializeField] private Transform basePos;    
    [SerializeField] private float maxDis = 30f;
    Ray ray = new Ray();
    

    private RaycastHit hit;

    private void Start()
    {
        ray.origin = basePos.localPosition;
        ray.direction = basePos.forward;        
    }

    private void Update()
    {
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
        {
            Debug.Log(hit.collider.gameObject.name);
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
            print("���� ���� ��ġ : " +ray.origin);
            print("������ ��ġ : " + basePos.position);
            line.SetPosition(0, ray.origin);
            line.SetPosition(1, hit.point);
            
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            line.gameObject.SetActive(true);
            print("��ư Ŭ��");
            
        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            print("��ư ����");
            line.gameObject.SetActive(false);
        }

    }



    // �ִϸ��̼� ��Ʈ�� �ؽ� �ڵ�
    private readonly int animNumHash = Animator.StringToHash("AnimNum");

    public void SetLeftHandAction(LeftAction action)
    {
        leftHandAnimator.SetInteger(animNumHash, (int)action);
    }

    public void SetRightHandAction(RightAction action)
    {
        rightHandAnimator.SetInteger(animNumHash, (int)action);
    }
}
