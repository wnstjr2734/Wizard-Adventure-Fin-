using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 불 그립 마법 - 화염 방사 구현
/// 작성자 - 차영철
/// </summary>
public class FlameThrower : GripMagic
{
    [SerializeField, Tooltip("화염 방사 데미지 판정 각도")]
    private float judgeAngle = 15f;
    private float judgeAngleCos;

    [SerializeField, Tooltip("데미지 판정 초기 딜레이")]
    private float initDelay = 0.3f;
    [SerializeField, Tooltip("데미지 판정 주기")] 
    private float damageInterval = 0.3f;
    // 데미지 주기까지 기다리는 시간
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
        // 시간 측정
        if (waitTime > 0.0f)
        {
            return;
        }

        // 원뿔 범위 보정
        Vector3 to = (other.transform.position - transform.position).normalized;
        Vector3 dir = transform.forward;

        // 일정 각도 이상이면 데미지 판정
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
