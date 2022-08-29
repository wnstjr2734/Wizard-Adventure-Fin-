using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

/// <summary>
/// 전체 Window를 관리하는 클래스로, 모든 Window는 이 클래스를 거쳐서 켜야한다
/// Window - 게임을 중단하고 열리는 UI
/// 작성자 - 차영철
/// </summary>
[DisallowMultipleComponent]
public class WindowSystem : Singleton<WindowSystem>
{
    class WindowClass
    {
        public GameObject windowObject;
        public bool isUserExitable;

        public WindowClass(GameObject windowObject, bool isUserExitable)
        {
            this.windowObject = windowObject;
            this.isUserExitable = isUserExitable;
        }
    }

    // 최상단 Window를 구분
    private Stack<WindowClass> windowStack = new Stack<WindowClass>();

    [SerializeField] private GameObject mainTitle;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject gameOver;
    public GameObject Loading;
    private CanvasGroup cg;

    void Start()
    {
        Debug.Log("Window System Activate");
        DOTween.defaultTimeScaleIndependent = true;
        DOTween.timeScale = 1.0f;       
       SceneCheck();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            // 창 닫을거 없으면 메뉴 켜기
            if (windowStack.Count == 0)
            {
                OpenWindow(menu, true);
            }
            // 창 닫기
            else
            {
                CloseWindow(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            LoadingWindow.Instance.LoadScene("SampleMap_LJS Test 1");
            DOTween.KillAll();
        }

    }

    public void OpenWindow(GameObject windowObject, bool isUserExitable)
    {
        if (windowStack.Count == 0)
        {
            SetWindowMode(true);
        }
        windowObject.SetActive(true);
        windowStack.Push(new WindowClass(windowObject, isUserExitable));
    }

    public bool CloseWindow(bool isUserExit)
    {
        // 더 이상 윈도우가 켜져 있지 않을 땐 윈도우를 끌 수 없다
        if (windowStack.Count == 0)
        {
            return false;
        }

        var wc = windowStack.Pop();
        // 종료할 수 있는 Window인지 확인한다
        if (isUserExit && !wc.isUserExitable)
        {
            windowStack.Push(wc);
            return false;
        }

        // 윈도우 종료
        wc.windowObject.SetActive(false);

        // 윈도우를 전부 종료했다면 커서를 띄운다
        if (windowStack.Count == 0)
        {
            SetWindowMode(false);
        }

        return true;
    }

    public void SetWindowMode(bool display)
    {
        Time.timeScale = display ? 0 : 1;
    }

    public void BackFade(bool Load)
    {

        if (cg != null)
        {
            if (!Load)
            {
                cg.DOFade(1, 3.0f);
            }
            if (Load)
            {
                cg.DOFade(0, 3.0f);
            }
        }
        else
        {
            cg.DOKill();
        }

       
        
    }

    void SceneCheck()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (mainTitle != null)
            {
                cg = mainTitle.GetComponentInChildren<CanvasGroup>();
                cg.alpha = 0.0f;
            }
        }


        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (Loading != null)
            {
                cg = Loading.GetComponent<CanvasGroup>();
                cg.alpha = 1.0f;
            }
            LoadingWindow.Instance.LoadScene("SampleMap_LJS Test 1");
            DOTween.KillAll();

        }
    }

}
