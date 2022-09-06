using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.InputSystem;

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

    [SerializeField]
    private TutorialWindow _tutorialWindow;

    public static TutorialWindow tutorialWindow { get; private set; }

   
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject gameOver;
    public GameObject Loading;
    private CanvasGroup cg;

    [SerializeField] private PlayerInput playerInput;

    protected override void OnAwake()
    {
        base.OnAwake();
        tutorialWindow = _tutorialWindow;
    }

    void Start()
    {
        Debug.Log("Window System Activate");
        DOTween.defaultTimeScaleIndependent = true;
        DOTween.timeScale = 1.0f;               
    }

    private void Update()
    {
        #region �����
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
        #endregion

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
        // UI ������ ���� ������ �������
        Time.timeScale = display ? 0 : 1;

        // UI ������ ���� �̵�, ȸ��, ������ ����ϸ� �� ��
        var actionMap = display ? "UI" : "Player";
        playerInput.SwitchCurrentActionMap(actionMap);
    }
   
}
