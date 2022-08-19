using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���� â ��� ����
/// �ۼ��� - ����ö
/// </summary>
public class HelpWindow : MonoBehaviour
{
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;

    [SerializeField] private TutorialViewer currentViewer;
    [SerializeField] private TutorialViewer nextViewer;

    [SerializeField] private TutorialExplainData[] datas;
    private int currentIndex = 0;

    [Header("Animation Setting")] 
    [SerializeField] private float animTime = 0.5f;
    [SerializeField] private float movePosX = 1024f;

    private bool isPlayingAnimation = false;

    // �ʱ�ȭ
    private void OnEnable()
    {
        // ���� ������ ���� �� ����
        currentViewer.SetContext(datas[currentIndex]);
        // Fade In Animation?
        currentViewer.Play();
        
        // ���� �������� ���� �������� �̵��� �� �ִ��� Ȯ��
        prevButton.interactable = currentIndex != 0;
        nextButton.interactable = currentIndex != datas.Length - 1;
    }

    private void OnDisable()
    {
        currentViewer.Stop();
    }

    private void Update()
    {
        // ����Ű�� �̿��� ����
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LoadPrevTutorial();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            LoadNextTutorial();
        }
    }

    public void LoadPrevTutorial()
    {
        // Ű����(XR Controller)�� ������ ��ư�� ���Ҿ �Լ��� ������ ���ɼ��� ����
        // ��� Index == 0�̸� �Լ� ������ ���ƾ� �Ѵ�
        if (currentIndex == 0 || isPlayingAnimation)
        {
            return;
        }

        // ���� ���� ���� ��� �۵� ����
        nextViewer.gameObject.SetActive(true);     
        nextViewer.SetContext(datas[currentIndex - 1]);
        currentViewer.Stop();
        nextViewer.Stop();

        // ���� ��� ��ġ ���� �� �����̵� �ִϸ��̼� 
        ChangeViewer(-movePosX);
        SetCurrentIndex(currentIndex - 1);
    }

    public void LoadNextTutorial()
    {
        // Ű����(XR Controller)�� ������ ��ư�� ���Ҿ �Լ��� ������ ���ɼ��� ����
        // ��� Index == 0�̸� �Լ� ������ ���ƾ� �Ѵ�
        if (currentIndex == datas.Length - 1 || isPlayingAnimation)
        {
            return;
        }

        // ���� ���� ���� ��� �۵� ���� �� �ʱ�ȭ
        nextViewer.gameObject.SetActive(true);
        nextViewer.SetContext(datas[currentIndex + 1]);
        currentViewer.Stop();
        nextViewer.Stop();

        ChangeViewer(movePosX);

        SetCurrentIndex(currentIndex + 1);
    }

    private void ChangeViewer(float translatePosX)
    {
        isPlayingAnimation = true;

        DOTween.defaultTimeScaleIndependent = true;
        DOTween.timeScale = 1.0f;

        // ���� ��� ��ġ ���� �� �����̵� �ִϸ��̼�
        Sequence s = DOTween.Sequence();
        s.timeScale = 1.0f;
        nextViewer.transform.localPosition = Vector3.right * translatePosX;
        s.Append(nextViewer.transform.DOLocalMoveX(0, animTime));
        s.Join(currentViewer.transform.DOLocalMove(Vector3.left * translatePosX, animTime));
        s.Join(nextViewer.GetComponent<CanvasGroup>().DOFade(1,1));
        s.Join(currentViewer.GetComponent<CanvasGroup>().DOFade(0, 1));
        s.OnComplete(() => {
            SwapCurrentAndNext();
            currentViewer.Play();
            nextViewer.Stop();
            isPlayingAnimation = false;
        });
        //s.Play();
        s.SetUpdate(true);
    }

    private void SwapCurrentAndNext()
    {
        (currentViewer, nextViewer) = (nextViewer, currentViewer);
    }

    private void SetCurrentIndex(int value)
    {
        currentIndex = value;

        prevButton.interactable = currentIndex != 0;
        nextButton.interactable = currentIndex != datas.Length - 1;
    }
}
