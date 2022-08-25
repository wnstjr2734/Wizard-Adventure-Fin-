using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 투사체 공격을 감지해서 막는 기능
/// 작성자 - 차영철
/// </summary>
public class MagicShield : MonoBehaviour
{
    [SerializeField, Tooltip("투사체를 막았을 때 이펙트")]
    private GameObject blockEffectPrefab;
    
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

    private void CreateBlockEffect(Vector3 position, Vector3 normal)
    {
        var blockEffect = PoolSystem.Instance.GetInstance<GameObject>(blockEffectPrefab);
        blockEffect.transform.position = position;
        blockEffect.transform.forward = normal;
    }
}
