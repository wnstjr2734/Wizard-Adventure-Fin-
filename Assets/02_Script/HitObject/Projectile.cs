using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ش� �������� ���ư��� ����ü�� ������Ʈ
/// �ۼ��� - ����ö
/// </summary>
public class Projectile : MonoBehaviour
{
    [SerializeField, Tooltip("����ü �̵��ӵ�")]
    private float moveSpeed = 4.5f;
    [SerializeField, Tooltip("����ü �����Ÿ�")]
    private float range = 5f;
    // ����ü�� �󸶳� ���� �����Ǵ���
    private float lifetime;

    // ������ ��� ���� ����

    [Header("VFX")]
    [SerializeField, Tooltip("����ü �¾��� �� ȿ��")]
    private ParticleSystem hitEffectPrefab;

    [Header("SFX")]
    [SerializeField, Tooltip("�߻��� �� ���� �Ҹ�")]
    private AudioClip shootSound;
    [SerializeField, Tooltip("�¾��� �� ���� �Ҹ�")]
    private AudioClip hitSound;

    private Rigidbody rb;


    // �ʱ� ���� ����
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
    /// Projectile �ʱ�ȭ �� �ӵ�(����, �ӷ�)�� �����ϴ� �Լ�
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
        // ���� ����
        lifetime -= Time.fixedDeltaTime;
        if (lifetime < 0)
        {
            Destroy();
        }

        // �߷� �޴� ��� ����ü ���ݵ� ȸ�����Ѿ���
        if (rb.useGravity)
        {
            transform.forward = rb.velocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ������ �� ���� �ֱ�
        // ĳ���� ������ �ִ� ��쿡�� �ش�
        // ����
        Destroy();
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }
}
