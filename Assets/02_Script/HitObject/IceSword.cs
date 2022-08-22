using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// ������ ȿ�� �� ����Ʈ ����
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class IceSword : GripMagic
{
    [SerializeField] 
    private Vector2 cutoffHeight = new Vector2(0.0f, 3.5f);
    
    [SerializeField, Tooltip("������ ���� �ð�")] 
    private float createTime = 1.0f;
    private float createSpeed;

    [SerializeField, Tooltip("������ ���� ���� �ӵ�")] 
    private float judgementSpeed = 15f;
    private float speed;
    private Vector3 previousPos;

    [SerializeField] 
    private ElementDamage elementDamage;

    [SerializeField, Tooltip("������ ������ ���� �� ������ ����Ʈ")] 
    private GameObject hitEffectPrefab;

    [SerializeField, Tooltip("���� ����Ʈ")] 
    private ParticleSystem iceFogEffect;
    [SerializeField, Tooltip("�� �ֳ����� ����Ʈ")]
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
        // �� �ӵ� ���
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
        // �ӵ��� �������� ��Ʈ ����
        if (speed > judgementSpeed)
        {
            var hitEffect = PoolSystem.Instance.GetInstance<GameObject>(hitEffectPrefab);
            hitEffect.transform.position = collision.contacts[0].point + collision.contacts[0].normal * 0.1f;
            hitEffect.transform.forward = collision.contacts[0].normal;

            // ���̸� ������ �ֱ�
            var status = collision.collider.GetComponent<CharacterStatus>();
            if (status)
            {
                status.TakeDamage(elementDamage);
            }
        }
    }
    
}
