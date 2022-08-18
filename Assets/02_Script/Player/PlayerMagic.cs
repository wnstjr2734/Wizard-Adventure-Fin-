using System;
using System.Collections;
using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using UnityEngine;

/// <summary>
/// 속성별 마법을 구현하는 클래스
/// 주의 : 속성 마법만 포함하고, 쉴드/텔레포트 등은 다른 클래스에서 처리
/// 작성자 - 차영철
/// </summary>
public class PlayerMagic : MonoBehaviour
{
    public enum ElementType
    {
        Fire,
        Ice,
        Lightning,
        Count
    }

    // 현재 마법 속성
    public ElementType CurrentElement { get; private set; } = ElementType.Fire;

    [Header("Base Magic")]
    [SerializeField] 
    private Projectile fireballPrefab;
    [SerializeField]
    private Projectile iceArrowPrefab;
    [SerializeField, Tooltip("흩뿌리는 개수")] 
    private int iceArrowShootCount = 5;
    [SerializeField]
    private LightningBolt lightningBolt;
    [SerializeField, Tooltip("라이트닝 볼트 히트시킬 대상")] 
    private LayerMask lightningLayerMask;

    [Header("Grip")] 
    [SerializeField] 
    private IceSword iceSword;

    [SerializeField, Tooltip("마법이 발사되는 위치")]
    private Transform MagicFirePositionTr;

    private PoolSystem poolSystem;

    private void Awake()
    {
        
    }

    private void Start()
    {
        poolSystem = PoolSystem.Instance;
        Debug.Assert(poolSystem, "Error : Pool System is not created");

        // 발사할 마법의 개수를 미리 지정해놓는다
        poolSystem.InitPool(fireballPrefab, 3);
        poolSystem.InitPool(iceArrowPrefab, 3 * iceArrowShootCount);   // 아이스 애로우는 몇 발 쏠지 모르겠음
    }

    #region Change Element

    /// <summary>
    /// 
    /// </summary>
    /// <param name="changeNextElement">
    /// 다음 원소로 바꿀지 이전 원소로 바꿀지 선택
    /// ex) changeNextElement = true : 불 -> 얼음 -> 전기 -> 불
    /// ex) changeNextElement = false : 불 -> 전기 -> 얼음 -> 불
    /// </param>
    public void ChangeElement(bool changeNextElement)
    {
        int addedElementIndex = changeNextElement ? 1 : -1;
        CurrentElement = (ElementType)(((int)CurrentElement + addedElementIndex) % (int)ElementType.Count);

        // 속성 바꾸면 그립 풀림
        // 속성 바꾸면 차지 풀림
    }

    #endregion

    #region Base Magic
    // 기본 마법 발사
    public void ShootMagic(Vector3 position, Vector3 direction)
    {
        // 쿨타임이면 무시

        // 지정된 속성의 마법을 발사
        switch (CurrentElement)
        {
            case ElementType.Fire:
                ShootFireball(position, direction);
                break;
            case ElementType.Ice:
                ShootIceArrow(position, direction);
                break;
            case ElementType.Lightning:
                ShootLightningBolt(position, direction);
                break;
            default:
                break;
        }
    }
    
    private void ShootFireball(Vector3 position, Vector3 direction)
    {
        var projectile = poolSystem.GetInstance<Projectile>(fireballPrefab);
        projectile.Shoot(position, direction);
    }

    private void ShootIceArrow(Vector3 position, Vector3 direction)
    {
        StartCoroutine(IEShootIceArrow(position, direction));
    }

    private IEnumerator IEShootIceArrow(Vector3 position, Vector3 direction)
    {
        // 샷건마냥 흩뿌리기
        for (int i = 0; i < iceArrowShootCount; i++)
        {
            var projectile = poolSystem.GetInstance<Projectile>(iceArrowPrefab);

            var newDirection = (direction + UnityEngine.Random.onUnitSphere * 0.2f).normalized;
            projectile.Shoot(position, newDirection);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void ShootLightningBolt(Vector3 position, Vector3 direction)
    {
        bool rayHit = Physics.Raycast(position, direction, out var hit, 20, lightningLayerMask);
        Collider hitEnemy;
        // 히트 스캔 방식 - 조준 보정 필요
        if (rayHit)
        {
            // 라이트닝 볼트 위치 지정
            hitEnemy = hit.collider;
        }
        // 직선 거리에 없을 땐 가장 가까운 녀석을 기준으로 함
        else
        {
            Vector3 endPosition = position + 20 * direction;
            // 적만 맞추기
            var enemies = Physics.OverlapCapsule(position, endPosition, 3, 
                1 << LayerMask.NameToLayer("Enemy"));
            if (enemies.Length == 0)
            {
                return;
            }
            
            // 가장 가까운 적을 맞춤
            float minDistance = Single.MaxValue;
            hitEnemy = enemies[0];
            foreach (var enemy in enemies)
            {
                float distance = Vector3.Distance(position, enemy.transform.position);
                if (distance < minDistance)
                {
                    hitEnemy = enemy;
                    minDistance = distance;
                }
            }
        }


        lightningBolt.EndObject.transform.position = hitEnemy.transform.position;
        StopCoroutine(nameof(IEShootLightningBolt));
        StartCoroutine(nameof(IEShootLightningBolt));

        var status = hitEnemy.GetComponent<CharacterStatus>();
        if (status)
        {
            status.TakeDamage(lightningBolt.elementDamage);
        }
    }

    private IEnumerator IEShootLightningBolt()
    {
        lightningBolt.gameObject.SetActive(true);
        lightningBolt.Duration = 0.1f;
        yield return new WaitForSeconds(0.1f);
        lightningBolt.gameObject.SetActive(false);
    }
    #endregion

    #region Charge Magic

    // 마법 충전이 완료됐을 때 이펙트
    // 

    #endregion

    #region Grip Magic

    // 누르고 있는 동안 마법이 지속 시전됨

    public void TurnOnGrip()
    {
        // 마나 닳게 처리

        switch (CurrentElement)
        {
            case ElementType.Fire:

                break;
            case ElementType.Ice:
                iceSword.TurnOn();
                break;
            case ElementType.Lightning:

                break;
            default:
                break;
        }
    }

    public void TurnOffGrip()
    {

        switch (CurrentElement)
        {
            case ElementType.Fire:

                break;
            case ElementType.Ice:
                iceSword.TurnOff();
                break;
            case ElementType.Lightning:

                break;
            default:
                break;
        }
    }

    #endregion
}
