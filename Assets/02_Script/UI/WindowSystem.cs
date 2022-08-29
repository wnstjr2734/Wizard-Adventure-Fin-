using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

/// <summary>
/// ��ü Window�� �����ϴ� Ŭ������, ��� Window�� �� Ŭ������ ���ļ� �Ѿ��Ѵ�
/// Window - ������ �ߴ��ϰ� ������ UI
/// �ۼ��� - ����ö
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

    // �ֻ�� Window�� ����
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
            // â ������ ������ �޴� �ѱ�
            if (windowStack.Count == 0)
            {
                OpenWindow(menu, true);
            }
            // â �ݱ�
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
        // �� �̻� �����찡 ���� ���� ���� �� �����츦 �� �� ����
        if (windowStack.Count == 0)
        {
            return false;
        }

        var wc = windowStack.Pop();
        // ������ �� �ִ� Window���� Ȯ���Ѵ�
        if (isUserExit && !wc.isUserExitable)
        {
            windowStack.Push(wc);
            return false;
        }

        // ������ ����
        wc.windowObject.SetActive(false);

        // �����츦 ���� �����ߴٸ� Ŀ���� ����
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
