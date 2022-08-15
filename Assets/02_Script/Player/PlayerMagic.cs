using System;
using System.Collections;
using System.Collections.Generic;
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
        Lightning
    }

    // 현재 마법 속성
    public ElementType CurrentElement { get; private set; } = ElementType.Fire;

    [SerializeField] 
    private Projectile fireballPrefab;
    [SerializeField]
    private Projectile iceArrowPrefab;
    // 라이트닝 볼트

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
        poolSystem.InitPool(iceArrowPrefab, 15);   // 아이스 애로우는 몇 발 쏠지 모르겠음
    }
    
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
                ShootLightningBolt(direction);
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
        // 샷건마냥 흩뿌리기
    }

    private IEnumerator IEShootIceArrow(Vector3 direction)
    {
        yield return null;
    }

    private void ShootLightningBolt(Vector3 direction)
    {
        // 히트 스캔 방식
        // 해당 방향을 기준으로 Raycast해서 마법 쏘기
    }
    #endregion

    #region Charge Magic

    // 마법 충전이 완료됐을 때 이펙트
    // 

    #endregion

    #region Focusing Magic

    // 누르고 있는 동안 마법이 지속 시전됨
    // 얼음 검의 경우 

    public void TurnOnCharge()
    {
        // 
    }

    public void TurnOffCharge()
    {

    }

    #endregion
}
