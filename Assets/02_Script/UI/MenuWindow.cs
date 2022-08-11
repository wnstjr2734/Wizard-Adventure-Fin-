using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �޴� ��� ���� Ŭ����
/// �ۼ��� - ����ö
/// </summary>
public class MenuWindow : MonoBehaviour
{
    [SerializeField] private HelpWindow helpWindow;

    public void OpenHelp()
    {
        WindowSystem.Instance.OpenWindow(helpWindow.gameObject, true);
    }
    
    public void CloseMenu()
    {
        // WindowSystem�� ȣ���� â�� �ݴ´�
        WindowSystem.Instance.CloseWindow(true);
    }
}
