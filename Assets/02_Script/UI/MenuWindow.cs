using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// �޴� ��� ���� Ŭ����
/// �ۼ��� - ����ö
/// </summary>
public class MenuWindow : MonoBehaviour
{
    [SerializeField] private HelpWindow helpWindow;

    public void OnLoad()
    {
        print("���̺� ����Ʈ�� �̵�");
    }

    public void OpenHelp()
    {
        WindowSystem.Instance.OpenWindow(helpWindow.gameObject, true);
    }
    
    public void CloseMenu()
    {
        // WindowSystem�� ȣ���� â�� �ݴ´�
        WindowSystem.Instance.CloseWindow(true);
    }

    public void OnMainMenu()
    {
        print("���� �޴��� �̵�");
        //SceneManager.LoadScene("MainMenu");
    }


}
