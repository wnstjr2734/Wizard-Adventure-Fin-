using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 얼음검 효과 및 이펙트 구현
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class IceSword : GripMagic
{
    [SerializeField] 
    private Vector2 cutoffHeight = new Vector2(0.0f, 3.5f);
    
    [SerializeField, Tooltip("얼음검 생성 시간")] 
    private float createTime = 1.0f;
    private float createSpeed;

    [SerializeField, Tooltip("데미지 판정 기준 속도")] 
    private float judgementSpeed = 15f;
    private float speed;
    private Vector3 previousPos;

    [SerializeField] 
    private ElementDamage elementDamage;

    [SerializeField, Tooltip("검으로 데미지 줬을 때 나오는 이펙트")] 
    private GameObject hitEffectPrefab;

    [SerializeField, Tooltip("서리 이펙트")] 
    private ParticleSystem iceFogEffect;
    [SerializeField, Tooltip("눈 휘날리는 이펙트")]
    private ParticleSystem snowEffect;

    private Collider hitBox;
    private Rigidbody rigidBody;
    private Material material;

    private readonly int cutoffHeightID = Shader.PropertyToID("_Cutoff_Height");
    
    private void Awake()
    {
        hitBox = GetComponent<BoxCollider>();
        rigidBody = GetComponent<Rigidbody>();
        material = GetComponent<MeshRenderer>().material;

        createSpeed = (cutoffHeight.y - cutoffHeight.x) / createTime;
    }

    private void Start()
    {
        previousPos = transform.position;

        PoolSystem.Instance.InitPool(hitEffectPrefab, 4);
    }

    private void Update()
    {
        // 검 속도 계산
        Vector3 currentPos = transform.position;
        speed = Vector3.Distance(currentPos, previousPos) / Time.deltaTime;
        previousPos = currentPos;
    }

    public override void TurnOn()
    {
        StopCoroutine(nameof(IETurnOffSword));
        StartCoroutine(nameof(IETurnOnSword));
    }

    private IEnumerator IETurnOnSword()
    {
        float currentHeight = material.GetFloat(cutoffHeightID);
        while (currentHeight < cutoffHeight.y)
        {
            currentHeight += Time.deltaTime * createSpeed;
            material.SetFloat(cutoffHeightID, currentHeight);
            yield return null;
        }

        hitBox.enabled = true;
        iceFogEffect.gameObject.SetActive(true);
        snowEffect.gameObject.SetActive(true);
    }

    public override void TurnOff()
    {
        StopCoroutine(nameof(IETurnOnSword));
        StartCoroutine(nameof(IETurnOffSword));
    }

    private IEnumerator IETurnOffSword()
    {
        hitBox.enabled = false;
        iceFogEffect.gameObject.SetActive(false);
        snowEffect.gameObject.SetActive(false);

        float currentHeight = material.GetFloat(cutoffHeightID);
        while (currentHeight > cutoffHeight.x)
        {
            currentHeight -= Time.deltaTime * createSpeed;
            material.SetFloat(cutoffHeightID, currentHeight);
            yield return null;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        // 속도를 기준으로 히트 판정
        if (speed > judgementSpeed)
        {
            var hitEffect = PoolSystem.Instance.GetInstance<GameObject>(hitEffectPrefab);
            hitEffect.transform.position = collision.contacts[0].point + collision.contacts[0].normal * 0.1f;
            hitEffect.transform.forward = collision.contacts[0].normal;

            // 적이면 데미지 주기
            var status = collision.collider.GetComponent<CharacterStatus>();
            if (status)
            {
                status.TakeDamage(elementDamage);
            }
        }
    }
    
}
