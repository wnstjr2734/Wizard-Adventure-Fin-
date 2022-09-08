using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 속성 변환 시 사운드 변경
/// 작성자 - 김도영
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
