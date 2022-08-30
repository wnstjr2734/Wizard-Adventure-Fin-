using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainTitle : MonoBehaviour
{
    //������ ����Ǹ� ���� ���� �ߴ� ȭ����        

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
    //��ŸƮ ��ư Ŭ���� �ε�ȭ�� ������ �̵�
    public void OnStart()
    {
        print("���� ����");
        StartCoroutine(nameof(IESceneChange));
        
    }
    //����ϱ⸦ �ϸ� ���̺� ����Ʈ�� �̵�
    public void OnContinue()
    {
        print("���̺� ����Ʈ�� �̵�");
    }
    //�����ϱ⸦ ������ ������ �����
    public void OnQuit()
    {
        print("���� ����");
        //Application.Quit();
    }

    private void OnTriggerEnter(Collider other)
    {
       
    }

    //3�� �Ŀ� �� �̵�
    IEnumerator IESceneChange()
    {
        WindowSystem.Instance.BackFade(false);
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(1);
    }

}
