using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�޸���
//�ش� ��ġ�� �÷��̾� �̵�
//���� Ʃ�丮�� �������� �� Ŭ���� ���¶��
//�ش� ��ġ�� �̵� �Ұ�

//�ڷ�ƾ
//ȭ�� ���̵�ƿ�
//�ش� ��ġ�� �÷��̾� �̵�
//ȭ�� ���̵� ��
/// <summary>
/// �� �������� �� ���� �� �� ���� ���� ��Ż Ȱ��ȭ ����
/// �÷��̾� ��Ż�̵�
/// �ۼ��� - ���ؼ�
/// </summary>
public class Portal : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private CharacterStatus[] targets;
    [SerializeField]
    public int remainCount;    //���� �� ��
    private bool istrigger;     //Ʈ���� ����ġ

    [Header("��Ż / �÷��̾� ���� ��ġ")]
    public GameObject portal;
    public Transform portalPoint;   //��Ż ź �� �÷��̾� ���� ��ġ
    
   
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        istrigger = false;
        portal.SetActive(false);
        StartRoom();
    }

    // ��Ż ����
    public void StartRoom()
    {
        foreach (var target in targets)
        {
            target.gameObject.SetActive(true);
            target.onDead += DecreaseCount;
        }
        // ���� ���� ����
        remainCount = targets.Length;
        CheckEnd();
    }

    private void DecreaseCount()
    {
        remainCount--;
        CheckEnd();
    }

    private void CheckEnd()
    {
        if (remainCount <= 0)
        {
            Debug.Log("������" + remainCount);
            istrigger = true;
            Debug.Log(istrigger);
            // ��Ż Ȱ��ȭ
            portal.SetActive(true);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && istrigger == true)
        {
            player.transform.position = portalPoint.position;
            Debug.Log("��Ż �̵�");
        }

        //if(other.name.Contains("Enemy Test") == true)
        //{
        //    StartRoom();
        //}
    }
}
