using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// 얼음검 효과 및 이펙트 구현
/// </summary>
public class IceSword : MonoBehaviour
{
    [SerializeField] 
    private Vector2 cutoffHeight = new Vector2(0.0f, 3.5f);
    
    [SerializeField, Tooltip("얼음검 생성 속도")] 
    private float createTime = 1.0f;

    private float speed;
    private Vector3 previousPos;

    private Collider hitBox;
    private Material material;

    private int cutoffHeightID;
    
    private void Awake()
    {
        cutoffHeightID = Shader.PropertyToID("Cutoff Height");
        hitBox = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        previousPos = transform.position;
    }

    private void Update()
    {
        // 검 속도 계산
        Vector3 currentPos = transform.position;
        speed = Vector3.Distance(currentPos, previousPos) / Time.deltaTime;
        previousPos = currentPos;
    }

    public void TurnOn()
    {
        StopCoroutine(nameof(IETurnOffSword));
        StartCoroutine(nameof(IETurnOnSword));
    }

    private IEnumerator IETurnOnSword()
    {
        float currentHeight = material.GetFloat(cutoffHeightID);
        float speed = cutoffHeight.y / createTime;
        while (currentHeight < cutoffHeight.y)
        {
            currentHeight += Time.deltaTime * speed;
            material.SetFloat(cutoffHeightID, currentHeight);
            yield return null;
        }

        hitBox.enabled = true;
        // 파티클 이펙트 켜기
    }

    public void TurnOff()
    {
        StopCoroutine(nameof(IETurnOnSword));
        StartCoroutine(nameof(IETurnOffSword));
    }

    private IEnumerator IETurnOffSword()
    {
        hitBox.enabled = false;
        // 파티클 이펙트 끄기 

        float currentHeight = material.GetFloat(cutoffHeightID);
        float speed = cutoffHeight.y / createTime;
        while (currentHeight < cutoffHeight.y)
        {
            currentHeight += Time.deltaTime * speed;
            material.SetFloat(cutoffHeightID, currentHeight);
            yield return null;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        // 속도를 기준으로 데미지를 입힌다
    }
    
}
