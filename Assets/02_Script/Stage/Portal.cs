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
    protected GameObject player;
    [SerializeField, Tooltip("�濡 �ִ� ���͵�")]
    protected CharacterStatus[] targets;
    protected Vector3[] initPoses;
    protected int remainCount;    //���� �� ��
    protected bool canUse = false;     //Ʈ���� ����ġ

    [Header("��Ż / �÷��̾� ���� ��ġ")]
    public GameObject portal;
    protected Collider portalCol;
    public EnemySpawn portalPoint;   //��Ż ź �� �÷��̾� ���� ��ġ

    [SerializeField, Tooltip("Ȱ��ȭ�� ���� ��")] 
    private GameObject nextRoom;
    [SerializeField, Tooltip("��Ȱ��ȭ�� ���� ��")]
    private GameObject currentRoom;

    private void Awake()
    {
        initPoses = new Vector3[targets.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            initPoses[i] = targets[i].transform.position;
        }

        portalCol = GetComponent<BoxCollider>();
        portalCol.enabled = false;
        canUse = false;
        portal = transform.GetChild(0).gameObject;
        portal.SetActive(false);

        var room = transform.parent;
        currentRoom = room.gameObject;

        var stage = room.parent;
        int sibilingIndex = room.GetSiblingIndex();
        if (sibilingIndex < stage.childCount - 1)
        {
            nextRoom = stage.GetChild(sibilingIndex + 1).gameObject;
        }
    }

    protected void Start()
    {
        player = GameManager.player;
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
            targets[i].transform.position = initPoses[i];
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
        CheckEnd();
    }

    private void CheckEnd()
    {
        if (remainCount <= 0)
        {
            canUse = true;
            portalCol.enabled = true;
            // ��Ż Ȱ��ȭ
            portal.SetActive(true);
        }
    }
    
    protected void OnTriggerEnter(Collider other)
    {
        if (canUse)
        {
            // ��Ż �ߺ� ��� �� �ǰ� ����
            canUse = false;
            OnUsePortal();
        }
    }

    /// <summary>
    /// ���� ��Ż ��� ��� (����׿�)
    /// </summary>
    public void UsePortal()
    {
        print("Debug : Forced Use Portal");
        OnUsePortal();
    }

    protected virtual void OnUsePortal()
    {
        StartCoroutine(IEUsePortal());
    }

    private IEnumerator IEUsePortal()
    {
        // ��Ż �̵� ����
        nextRoom.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        // �� �̵�
        player.GetComponent<PlayerMoveRotate>().SetPos(portalPoint.transform.position);
        // ���� ���� SetActive(false) ó��

        // ��Ż Ÿ�� �� ���� Fade Out �Ǹ鼭 ���� �� �Ѿ�� ����
        yield return new WaitForSeconds(0.5f);
        currentRoom.SetActive(false);
    }
}