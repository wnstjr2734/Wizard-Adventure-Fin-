using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 해당 방향으로 나아가는 투사체형 오브젝트
/// 작성자 - 차영철
/// </summary>
public class Projectile : MonoBehaviour
{
    [SerializeField, Tooltip("투사체 이동속도")]
    private float moveSpeed = 4.5f;
    [SerializeField, Tooltip("투사체 사정거리")]
    private float range = 5f;
    // 투사체가 얼마나 오래 유지되는지
    private float lifetime;

    // 데미지 등등 정보 설정

    [Header("VFX")]
    [SerializeField, Tooltip("투사체 맞았을 때 효과")]
    private ParticleSystem hitEffectPrefab;

    [Header("SFX")]
    [SerializeField, Tooltip("발사할 때 나는 소리")]
    private AudioClip shootSound;
    [SerializeField, Tooltip("맞았을 때 나는 소리")]
    private AudioClip hitSound;

    private Rigidbody rb;


    // 초기 방향 설정
    private Vector3 direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        lifetime = range / moveSpeed;
    }

    /// <summary>
    /// Projectile 초기화 시 속도(방향, 속력)를 지정하는 함수
    /// </summary>
    public void Shoot(Vector3 position, Vector3 direction)
    {
        transform.position = position;
        this.direction = direction;
        transform.forward = this.direction;
        rb.velocity = this.direction * moveSpeed;
        rb.angularVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        // 수명 제어
        lifetime -= Time.fixedDeltaTime;
        if (lifetime < 0)
        {
            Destroy();
        }

        // 중력 받는 경우 투사체 공격도 회전시켜야함
        if (rb.useGravity)
        {
            transform.forward = rb.velocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 데미지 등 정보 주기
        // 캐릭터 스탯이 있는 경우에만 준다
        // 삭제
        Destroy();
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }
}
