using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

/// <summary>
/// ����ü ������ �����ؼ� ���� 
/// �ۼ��� - ����ö
/// </summary>
public class MagicShield : MonoBehaviour
{
    [SerializeField, Tooltip("���и� ���� �� ��Ÿ���� ����Ʈ")] 
    private ParticleSystem shieldEffect;
    [SerializeField, Tooltip("����ü�� ������ �� ����Ʈ")]
    private ParticleSystem blockEffect;

    [SerializeField, Tooltip("����ü�� ������ �� �Ҹ�")]
    private AudioClip blockSound;
    

    private void OnTriggerEnter(Collider other)
    {
        // ��� ����Ʈ
        CreateBlockEffect();
    }

    // �ǵ� �ѱ�/����
    public void ActiveShield(bool active)
    {
        shieldEffect.gameObject.SetActive(active);
        // ���� ų �� ��ƼŬ �ý��� �ٽ� ������ 
    }

    private void CreateBlockEffect()
    {

    }
}
