using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// �� �׸� ���� - ȭ�� ��� ����
/// �ۼ��� - ����ö
/// </summary>
public class FlameThrower : GripMagic
{
    [SerializeField, Tooltip("ȭ�� ��� ������ ���� ����")]
    private float judgeAngle = 15f;
    private float judgeAngleCos;

    [SerializeField, Tooltip("������ ���� �ʱ� ������")]
    private float initDelay = 0.3f;
    [SerializeField, Tooltip("������ ���� �ֱ�")] 
    private float damageInterval = 0.3f;
    // ������ �ֱ���� ��ٸ��� �ð�
    private float waitTime;

    [SerializeField] 
    private LayerMask enemyLayerMask;

    [SerializeField]
    private ElementDamage elementDamage;

    private void FixedUpdate()
    {
        waitTime -= Time.fixedDeltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        // �ð� ����
        if (waitTime > 0.0f)
        {
            return;
        }

        // ���� ���� ����
        Vector3 to = (other.transform.position - transform.position).normalized;
        Vector3 dir = transform.forward;

        // ���� ���� �̻��̸� ������ ����
        float angleCos = Vector3.Dot(to, dir);
        if (angleCos > judgeAngleCos)
        {
            GiveDamage(other.GetComponent<CharacterStatus>());
        }

        waitTime = damageInterval;
    }

    public override void TurnOn()
    {
        waitTime = initDelay;
        judgeAngleCos = Mathf.Cos(Mathf.Deg2Rad * judgeAngle);
    }

    private void GiveDamage(CharacterStatus status)
    {
        if (status)
        {
            status.TakeDamage(elementDamage);
        }
    }

    public override void TurnOff()
    {
        gameObject.SetActive(false);
    }
}