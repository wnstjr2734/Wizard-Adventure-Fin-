using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ش� ��ġ�� �÷��̾� �̵�
//���� Ʃ�丮�� �������� �� Ŭ���� ���¶��
//�ش� ��ġ�� �̵� �Ұ�

//�ڷ�ƾ
//ȭ�� ���̵�ƿ�
//�ش� ��ġ�� �÷��̾� �̵�
//ȭ�� ���̵� ��
public class Portal : MonoBehaviour
{
    
    [SerializeField] private GameObject player;
    public Transform portalPoint;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player.transform.position = portalPoint.position;
        }
        
    }
}
