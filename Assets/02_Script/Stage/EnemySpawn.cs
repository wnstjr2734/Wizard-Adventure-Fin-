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

    [SerializeField]
    private TutorialWindow window;
    [SerializeField, Tooltip("�� �̵����� �� ��� Ʃ�丮�� â ����")]
    private TutorialExplainData tutorialData;
    [SerializeField, Tooltip("��� ���")]
    private PlayerController.MagicAbility[] learnAbility;

    private void OnTriggerEnter(Collider other)
    {
        if(!isTriggered && other.name.Contains("Player"))
        {
            portal.StartRoom();
            isTriggered = true;

            GameManager.Instance.SetCheckPoint(transform.position, portal);
            // Ʃ�丮�� Ȱ��ȭ
            if (tutorialData != null)
            {
                WindowSystem.Instance.OpenWindow(window.gameObject, true);
                window.Open(tutorialData);
            }
        }
    }
}