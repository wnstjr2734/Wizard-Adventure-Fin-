using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Ӽ� ��ȯ �� ���� ����
/// �ۼ��� - �赵��
/// </summary>

public class ECS_Changer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] elementSound;
    PropertiesWindow pw;
    ElementType et;
    // Start is called before the first frame update
    void Start()
    {
        pw = GetComponent<PropertiesWindow>();
        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SoundChange(ElementType elementType)
    {
        if ((int)elementType == 1)
        {

        }
        
    }

}
