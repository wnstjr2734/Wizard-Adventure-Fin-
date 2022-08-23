using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// ���� ����Ʈ ��� ����
/// �ۼ��� - ����ö
/// </summary>
public class ChargeEffect : MonoBehaviour
{
    // ���� ����Ʈ�� 3�ܰ�� ����
    // 1�ܰ�(ó�� ���� ��) - 
    private static readonly int ringIndex = 1;
    private static readonly int glowIndex = 2;

    [SerializeField, Tooltip("�ܰ躰 ���� ����")]
    private Vector3 chargeScale = new Vector3(0.2f, 0.5f, 1.0f);

    [SerializeField, Tooltip("���� �ܰ� �ö󰡴� �ִϸ��̼� �����ִ� �ð�")] 
    private float chargeScaleUpTime = 0.5f;

    private ParticleSystem[] particleSystems;
    private LightFlicker lightFlicker;

    [SerializeField, Tooltip("��, ����, ���� �Ӽ� ����")] 
    private Color[] elementColor = new Color[] {Color.red, Color.cyan, Color.yellow};

    [SerializeField, Tooltip("������ �ʿ��� �ð�")]
    private float chargeTime = 5.0f;

    public bool ChargeCompleted { get; private set; }

    private void Awake()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        lightFlicker = GetComponentInChildren<LightFlicker>();
        print("particleSystems count : " + particleSystems.Length);
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.one * chargeScale.x;
        particleSystems[ringIndex].gameObject.SetActive(false);
        particleSystems[glowIndex].gameObject.SetActive(false);
        ChargeCompleted = false;

        SetColor(ElementType.Fire);
        StartCoroutine(nameof(IEChargeGauge));
    }

    private IEnumerator IEChargeGauge()
    {
        float chargePercent = 0.0f;
        float chargeSpeed = 1.0f / chargeTime;

        while (chargePercent < 0.5f)
        {
            chargePercent += Time.deltaTime * chargeSpeed;
            yield return null;
        }
        particleSystems[ringIndex].gameObject.SetActive(true);
        transform.DOScale(Vector3.one * chargeScale.y, 0.5f);

        while (chargePercent < 1.0f)
        {
            chargePercent += Time.deltaTime * chargeSpeed;
            yield return null;
        }
        particleSystems[glowIndex].gameObject.SetActive(true);
        transform.DOScale(Vector3.one * chargeScale.z, 0.5f);

        ChargeCompleted = true;
    }

    private void OnDisable()
    {
        
    }

    public void SetColor(ElementType element)
    {
        Color particleColor = elementColor[(int) element];

        foreach (var system in particleSystems)
        {
            var main = system.main;
            main.startColor = particleColor;
        }

        lightFlicker.UpdateColor(particleColor);
    }
}
