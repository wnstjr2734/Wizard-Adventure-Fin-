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

    [Header("Base Magic")] 
    [SerializeField, EnumNamedArray(typeof(ElementType))]
    [Tooltip("불/얼음/전기 속성 기본 마법")]
    private Magic[] baseMagicPrefabs = new Magic[(int)ElementType.Count];

    [Header("Grip")] 
    [SerializeField, EnumNamedArray(typeof(ElementType))]
    [Tooltip("불/얼음/전기 속성 그립 마법")] 
    private GripMagic[] gripMagics = new GripMagic[(int)ElementType.Count];

    [Header("Charge")] 
    [SerializeField] 
    private ChargeEffect chargeEffect;
    [SerializeField, EnumNamedArray(typeof(ElementType))]
    [Tooltip("불/얼음/전기 속성 차지 마법")]
    private Magic[] chargeMagicPrefabs = new Magic[(int)ElementType.Count];

    [SerializeField, Tooltip("마법이 발사되는 위치")]
    private Transform MagicFirePositionTr;

    private PoolSystem poolSystem;

    // 현재 마법 속성
    public ElementType CurrentElement { get; private set; } = ElementType.Fire;

    public event Action<ElementType> onChangeElement;

    private void Awake()
    {
        
    }

    private void Start()
    {
        poolSystem = PoolSystem.Instance;
        Debug.Assert(poolSystem, "Error : Pool System is not created");

        // 발사할 마법의 개수를 미리 지정해놓는다
        poolSystem.InitPool(baseMagicPrefabs[(int)ElementType.Fire], 3);
        poolSystem.InitPool(baseMagicPrefabs[(int)ElementType.Ice], 2);
        poolSystem.InitPool(baseMagicPrefabs[(int)ElementType.Lightning], 4);   // 번개 구름이랑 겹칠 수 있음

        poolSystem.InitPool(chargeMagicPrefabs[(int)ElementType.Ice], 1);
    }

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
        // 속성 바꾸면 그립 풀림
        // 속성 바꾸면 차지 풀림

        int addedElementIndex = changeNextElement ? 1 : -1;
        CurrentElement = (ElementType)(((int)CurrentElement + addedElementIndex) % (int)ElementType.Count);

        onChangeElement?.Invoke(CurrentElement);
    }

    #region Base Magic
    // 기본 마법 발사
    public void ShootMagic(Vector3 position, Vector3 direction)
    {
        // 쿨타임이면 무시

        // 지정된 속성의 마법을 발사
        var magic = poolSystem.GetInstance<Magic>(baseMagicPrefabs[(int)CurrentElement]);
        magic.SetPosition(position);
        magic.SetDirection(direction);
        magic.StartMagic();
    }
    #endregion

    #region Charge Magic

    // 마법 충전이 완료됐을 때 이펙트
    // 
    public void StartCharge()
    {
        chargeEffect.gameObject.SetActive(true);
        chargeEffect.SetColor(CurrentElement);
    }

    private void OnCharge()
    {
        // 차지 충전 중 보여줄 정보
    }

    public void EndCharge()
    {
        if (chargeEffect.ChargeCompleted)
        {
            // 현재 속성의 마법 시전
            var magic = poolSystem.GetInstance<Magic>(chargeMagicPrefabs[(int)CurrentElement]);
            if (magic.IsSelfTarget)
            {
                magic.SetPosition(transform.position);
            }
            else
            {
                
            }
        }
        chargeEffect.gameObject.SetActive(false);
    }

    #endregion

    #region Grip Magic

    // 누르고 있는 동안 마법이 지속 시전됨

    public void TurnOnGrip()
    {
        // 마나 닳게 처리
        if (gripMagics[(int)CurrentElement])
        {
            gripMagics[(int)CurrentElement].TurnOn();
        }
    }

    public void TurnOffGrip()
    {
        if (gripMagics[(int)CurrentElement])
        {
            gripMagics[(int)CurrentElement].TurnOff();
        }
    }

    // 화염, 전기 등 그랩 중일 때 판정하는 
    private void OnGrip()
    {

    }

    #endregion
}
