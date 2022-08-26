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

    public CharacterController cc;
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
        cc = player.GetComponent<CharacterController>();
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
        StartCoroutine(CcOnOff());
    }

    IEnumerator CcOnOff()
    {
        cc.enabled = false;
        yield return new WaitForSeconds(0.5f);
        if(!cc.enabled)
        {
            player.transform.position = portalPoint.position;
        }
        yield return new WaitForSeconds(0.5f);
        cc.enabled = true;
    }
}
