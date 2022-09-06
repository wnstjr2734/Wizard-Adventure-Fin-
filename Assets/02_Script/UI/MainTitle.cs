using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MainTitle : MonoBehaviour
{
    //������ ����Ǹ� ���� ���� �ߴ� ȭ����           
    [SerializeField] private GameObject back;
    private CanvasGroup cg;


    // Start is called before the first frame update
    void Start()
    {
        cg = back.GetComponent<CanvasGroup>();
        cg.alpha = 1;
        BackFade(true);
    }

    //��ŸƮ ��ư Ŭ���� �ε�ȭ�� ������ �̵�
    public void OnStart()
    {        
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
        Application.Quit();
    }
  

    //3�� �Ŀ� �� �̵�
    IEnumerator IESceneChange()
    {
        BackFade(false);
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(1);
    }
  

    //�� �̵��� ���̵� ȿ��
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
            cg.DOKill(); //�� �̵� �� Dotween ���� ����
        }

    }





}
