using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.WSA;

/// <summary>
/// 투사체 공격을 감지해서 막는 기능
/// 작성자 - 차영철
/// </summary>
public class MagicShield : MonoBehaviour
{
    [SerializeField, Tooltip("방패를 켰을 때 나타나는 이펙트")] 
    private ParticleSystem shieldEffect;
    [SerializeField, Tooltip("투사체를 막았을 때 이펙트")]
    private ParticleSystem blockEffectPrefab;

    [SerializeField, Tooltip("투사체를 막았을 때 소리")]
    private AudioClip blockSound;

    private void Start()
    {
        // 최대 3개 정도 막을거라 예상
        PoolSystem.Instance.InitPool(blockEffectPrefab, 3);
    }

    private void OnCollisionEnter(Collision collision)
    {
        print($"Collision Enter - {collision.gameObject.name}");
        CreateBlockEffect(collision.contacts[0].point, collision.contacts[0].normal);
    }
    
    // 실드 켜기/끄기
    public void ActiveShield(bool active)
    {
        shieldEffect.gameObject.SetActive(active);
        // 껐다 킬 때 파티클 시스템 다시 켜지면 
    }

    private void CreateBlockEffect(Vector3 position, Vector3 normal)
    {
        var blockEffect = PoolSystem.Instance.GetInstance<ParticleSystem>(blockEffectPrefab);
        blockEffect.transform.position = position;
        blockEffect.transform.forward = normal;
    }
}
