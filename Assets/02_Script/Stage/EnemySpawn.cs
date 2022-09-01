using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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

    [SerializeField]
    private TutorialWindow window;
    [SerializeField, Tooltip("�� �̵����� �� ��� Ʃ�丮�� â ����")]
    private TutorialExplainData[] tutorialDatas;
    [SerializeField, Tooltip("��� ���")]
    private PlayerController.MagicAbility[] learnAbilities;

    private void OnTriggerEnter(Collider other)
    {
        if(!isTriggered && other.name.Contains("Player"))
        {
            portal.StartRoom();
            isTriggered = true;

            GameManager.Instance.SetCheckPoint(transform.position, portal);
            // Ʃ�丮�� Ȱ��ȭ
            if (tutorialDatas != null && tutorialDatas.Length != 0)
            {
                WindowSystem.Instance.OpenWindow(window.gameObject, true);
                window.Open(tutorialDatas);

                var playerController = GameManager.player.GetComponent<PlayerController>();
                for (int i = 0; i < learnAbilities.Length; i++)
                {
                    playerController.LearnAbility(learnAbilities[i]);
                }
            }
        }
    }
}