using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾� ���� Ʈ���� �Ǹ�
//�� ���忡�� �� ����
//�� �Ҽ���, �ü�, ���� ���� ����
//�� ��

/// <summary>
/// �ڷ���Ʈ �Ǹ� �� Ȱ��ȭ
/// </summary>
public class EnemySpawn : MonoBehaviour
{
    public Portal portal;
    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!isTriggered && other.name.Contains("Player"))
        {
            portal.StartRoom();
            isTriggered = true;
        }
    }
}