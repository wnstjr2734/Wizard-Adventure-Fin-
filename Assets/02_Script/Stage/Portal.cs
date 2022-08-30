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
    [SerializeField, Tooltip("�濡 �ִ� ���͵�")]
    private CharacterStatus[] targets;
    private Vector3[] initPos;
    [SerializeField]
    public int remainCount;    //���� �� ��
    private bool istrigger;     //Ʈ���� ����ġ

    [Header("��Ż / �÷��̾� ���� ��ġ")]
    public GameObject portal;
    public Collider portalCol;
    public Transform portalPoint;   //��Ż ź �� �÷��̾� ���� ��ġ

    private void Awake()
    {
        initPos = new Vector3[targets.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            initPos[i] = targets[i].transform.position;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        portalCol = GetComponent<BoxCollider>();
        portalCol.enabled = false;
        cc = player.GetComponent<CharacterController>();
        istrigger = false;
        portal.SetActive(false);
    }

    /// <summary>
    /// ���� �ʱ�ȭ�Ѵ�
    /// </summary>
    public void ResetRoom()
    {
        remainCount = targets.Length;
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].gameObject.SetActive(true);

            // ���� ���� ����
            targets[i].ResetStatus();
            
            // ���� ��ġ ����
            targets[i].transform.position = initPos[i];
        }

        // �÷��̾�� ���Ͱ� ���� �������� �� ��Ż ����
        // ���� ��Ż�� �����ؾ��Ѵ�
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
        Debug.Log("������" + remainCount);
        CheckEnd();
    }

    private void CheckEnd()
    {
        if (remainCount <= 0)
        {
            istrigger = true;
            portalCol.enabled = true;
            Debug.Log(istrigger);
            // ��Ż Ȱ��ȭ
            portal.SetActive(true);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(CcOnOff());
    }

    private IEnumerator CcOnOff()
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