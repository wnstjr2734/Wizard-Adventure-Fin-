using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainTitle : MonoBehaviour
{
    //게임이 실행되면 가장 먼저 뜨는 화면임        

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            OnStart();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnContinue();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnQuit();
        }


    }
    //스타트 버튼 클릭시 로딩화면 씬으로 이동
    public void OnStart()
    {
        print("게임 시작");
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
        print("게임 종료");
        //Application.Quit();
    }

    private void OnTriggerEnter(Collider other)
    {
       
    }

    //3초 후에 씬 이동
    IEnumerator IESceneChange()
    {
        WindowSystem.Instance.BackFade(false);
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(1);
    }

}
