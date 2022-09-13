using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


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
/// 0912 �߰� : audioSource �� ��Ż Ż�� fadeInOut UI����
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

    [SerializeField, Tooltip("��Ż Ż �� ����")]
    private AudioSource portalSound;

    [SerializeField, Tooltip("Ȱ��ȭ�� ���� ��")]
    private GameObject nextRoom;
    [SerializeField, Tooltip("��Ȱ��ȭ�� ���� ��")]
    private GameObject currentRoom;

    [Tooltip("��Ż Ż �� fade In Out ����")]
    public GameObject fadeImage;
    public CanvasGroup canvasGroup; //���̵� �ξƿ� ĵ����

    private void Awake()
    {
        initPoses = new Vector3[targets.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            initPoses[i] = targets[i].transform.position;
        }

        //��Ż ����
        portalSound = GetComponent<AudioSource>();
        portalCol = GetComponent<BoxCollider>();
        portalCol.enabled = false;
        portalSound.enabled = false;
        canUse = false;
        portal = transform.GetChild(0).gameObject;
        portal.SetActive(false);

        //���̵��ξƿ� ������Ʈ �ʱ� ��Ȱ��ȭ
        fadeImage.SetActive(false);

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

    //���̵� �ξƿ� ���� �Լ�
    private void Fade()
    {
        fadeImage.SetActive(true);

        DOTween.Sequence()
            .Append(canvasGroup.DOFade(1.0f, 3f))   //���� ����� ���̵�ƿ�
            .Append(canvasGroup.DOFade(0.0f, 3f))   //���� ����� ���̵���
            .OnComplete(() =>                       //�Ϸ� �Ŀ��� canvasgroup, Canvas ������Ʈ ��Ȱ��ȭ
            {
                fadeImage.SetActive(false);
            });
    }

    private IEnumerator IEUsePortal()
    {
        //��Ż ���� �۵� - �ۼ��� ���ؼ�
        portalSound.enabled = true;
        yield return new WaitForSeconds(0.1f);
        Fade();
        yield return new WaitForSeconds(5f);
        // ��Ż �̵� ����
        nextRoom.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        // �� �̵�
        player.GetComponent<PlayerMoveRotate>().SetPos(portalPoint.transform.position, portalPoint.transform.forward);
        // ���� ���� SetActive(false) ó��

        // ��Ż Ÿ�� �� ���� Fade Out �Ǹ鼭 ���� �� �Ѿ�� ����
        yield return new WaitForSeconds(0.5f);
        currentRoom.SetActive(false);
    }
}