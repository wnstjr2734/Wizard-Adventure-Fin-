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
    private int remainCount;    //���� �� ��
    private bool istrigger;     //Ʈ���� ����ġ

    [Header("��Ż / �÷��̾� ���� ��ġ")]
    public GameObject portal;
    public Collider portalCol;
    public Transform portalPoint;   //��Ż ź �� �÷��̾� ���� ��ġ

    [SerializeField, Tooltip("Ȱ��ȭ�� ���� ��")] 
    private GameObject nextRoom;
    [SerializeField, Tooltip("��Ȱ��ȭ�� ���� ��")]
    private GameObject currentRoom;

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
            targets[i].GetComponent<EnemyFSM>().ResetFSM();
            
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
        StartCoroutine(IEUsePortal());
    }

    private IEnumerator IEUsePortal()
    {
        // ��Ż �̵� ����
        nextRoom.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        // �� �̵�
        player.GetComponent<PlayerMoveRotate>().SetPos(portalPoint.position);
        // ���� ���� SetActive(false) ó��

        // ��Ż Ÿ�� �� ���� Fade Out �Ǹ鼭 ���� �� �Ѿ�� ����
        yield return new WaitForSeconds(0.5f);
        currentRoom.SetActive(false);
    }
}