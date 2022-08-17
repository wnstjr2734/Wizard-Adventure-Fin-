using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.WSA;

/// <summary>
/// ����ü ������ �����ؼ� ���� ���
/// �ۼ��� - ����ö
/// </summary>
public class MagicShield : MonoBehaviour
{
    [SerializeField, Tooltip("���и� ���� �� ��Ÿ���� ����Ʈ")] 
    private ParticleSystem shieldEffect;
    [SerializeField, Tooltip("����ü�� ������ �� ����Ʈ")]
    private ParticleSystem blockEffectPrefab;

    [SerializeField, Tooltip("����ü�� ������ �� �Ҹ�")]
    private AudioClip blockSound;

    private void Start()
    {
        // �ִ� 3�� ���� �����Ŷ� ����
        //PoolSystem.Instance.InitPool(blockEffectPrefab, 3);
    }

    private void OnCollisionEnter(Collision collision)
    {
        print($"Collision Enter - {collision.gameObject.name}");
        //collision.contacts[0].normal
        //CreateBlockEffect();
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Trigger Enter");
        // ��� ����Ʈ
        CreateBlockEffect(other.transform);
    }

    // �ǵ� �ѱ�/����
    public void ActiveShield(bool active)
    {
        shieldEffect.gameObject.SetActive(active);
        // ���� ų �� ��ƼŬ �ý��� �ٽ� ������ 
    }

    private void CreateBlockEffect(Transform hitTr)
    {
        //var blockEffect = PoolSystem.Instance.GetInstance<ParticleSystem>(blockEffectPrefab);
        //blockEffect.transform.position = hitTr.position;
    }
}
