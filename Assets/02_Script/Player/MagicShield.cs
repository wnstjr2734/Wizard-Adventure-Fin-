using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

/// <summary>
/// 투사체 공격을 감지해서 막는 
/// 작성자 - 차영철
/// </summary>
public class MagicShield : MonoBehaviour
{
    [SerializeField, Tooltip("방패를 켰을 때 나타나는 이펙트")] 
    private ParticleSystem shieldEffect;
    [SerializeField, Tooltip("투사체를 막았을 때 이펙트")]
    private ParticleSystem blockEffect;

    [SerializeField, Tooltip("투사체를 막았을 때 소리")]
    private AudioClip blockSound;
    

    private void OnTriggerEnter(Collider other)
    {
        // 방어 이펙트
        CreateBlockEffect();
    }

    // 실드 켜기/끄기
    public void ActiveShield(bool active)
    {
        shieldEffect.gameObject.SetActive(active);
        // 껐다 킬 때 파티클 시스템 다시 켜지면 
    }

    private void CreateBlockEffect()
    {

    }
}
