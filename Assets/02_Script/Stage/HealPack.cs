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
    [SerializeField, Tooltip("�� ������ ��Ÿ�� ����Ʈ")]
    private ParticleSystem healEffectPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Contains("Player"))
        {
            var playerStatus = GameManager.player.GetComponent<CharacterStatus>();
            playerStatus.ResetStatus();

            var healEffect = Instantiate(healEffectPrefab, transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
    }
}