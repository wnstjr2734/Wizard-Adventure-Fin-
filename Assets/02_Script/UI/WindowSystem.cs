using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    void Start()
    {
        Debug.Log("Window System Activate");
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
}
