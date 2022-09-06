using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MainTitle : MonoBehaviour
{
    //게임이 실행되면 가장 먼저 뜨는 화면임           
    [SerializeField] private GameObject back;
    private CanvasGroup cg;


    // Start is called before the first frame update
    void Start()
    {
        cg = back.GetComponent<CanvasGroup>();
        cg.alpha = 1;
        BackFade(true);
    }

    //스타트 버튼 클릭시 로딩화면 씬으로 이동
    public void OnStart()
    {        
        StartCoroutine(nameof(IESceneChange));
    }
    //계속하기를 하면 세이브 포인트로 이동
    public void OnContinue()
    {
        print("세이브 포인트로 이동");
    }
    //종료하기를 누르면 게임이 종료됨
    public void OnQuit()
    {        
        Application.Quit();
    }
  

    //3초 후에 씬 이동
    IEnumerator IESceneChange()
    {
        BackFade(false);
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(1);
    }
  

    //씬 이동시 페이드 효과
    private void BackFade(bool Load)
    {
        int num;
        if (cg != null)
        {
            num = Load ? 0 : 1;
            cg.DOFade(num, 3.0f);

        }
        else
        {
            cg.DOKill(); //씬 이동 시 Dotween 실행 종료
        }

    }





}
