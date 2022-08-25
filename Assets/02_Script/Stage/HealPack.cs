using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �޸���
// 1. Ʈ���� name.Contain�� player�� Destroy


/// <summary>
/// �ۼ��� - ���ؼ�
/// ���� ���� - �÷��̾ ������ ������ Destroy
/// </summary>
public class HealPack : MonoBehaviour
{
    [SerializeField]
    private GameObject healpack;

    private void Start()
    {
        healpack = GameObject.Find("Heal Pack");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Contains("Player"))
        {
            Destroy(healpack);
        }
    }
}