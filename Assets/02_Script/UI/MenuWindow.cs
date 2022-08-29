using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 메뉴 기능 구현 클래스
/// 작성자 - 차영철
/// </summary>
public class MenuWindow : MonoBehaviour
{
    [SerializeField] private HelpWindow helpWindow;

    public void OnLoad()
    {
        print("세이브 포인트로 이동");
    }

    public void OpenHelp()
    {
        WindowSystem.Instance.OpenWindow(helpWindow.gameObject, true);
    }
    
    public void CloseMenu()
    {
        // WindowSystem을 호출해 창을 닫는다
        WindowSystem.Instance.CloseWindow(true);
    }

    public void OnMainMenu()
    {
        print("메인 메뉴로 이동");
        //SceneManager.LoadScene("MainTitle_KDY");
    }


}
