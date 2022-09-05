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
    [SerializeField] private GameObject ShootDetector;
    [SerializeField] private GameObject MagicWand;
    [SerializeField] private GameObject Firebullet;
    
    private GameObject bulletFactory;

    private void Start()
    {       
        ShootDetector = transform.FindChildRecursive("ShootDetector").gameObject;
        MagicWand = transform.FindChildRecursive("Magic_wand_06").gameObject;
        Firebullet.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    private void Update()
    {
        ShootDetectorSwitch(PlayerController.Instance.CanControlPlayer);            
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            if (basePos != null)
            {
                bulletFactory = Instantiate(Firebullet);
                bulletFactory.transform.position = basePos.position;
                bulletFactory.transform.rotation = Quaternion.LookRotation(basePos.forward);
                bulletFactory.transform.position += bulletFactory.transform.forward * 3.0f * Time.deltaTime;
                Destroy(bulletFactory, 3.0f);
            }
           
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

    void ShootDetectorSwitch(bool ActionMap)
    {
        //Player Action Map �� Player �� True, UI �� False
        bool defultMap = ActionMap ? true : false;
       RightAction ra = ActionMap ? RightAction.WandGrip : RightAction.UiSelect;

        ShootDetector.SetActive(defultMap);
        MagicWand.SetActive(defultMap);
        SetRightHandAction(ra);
    }

}
