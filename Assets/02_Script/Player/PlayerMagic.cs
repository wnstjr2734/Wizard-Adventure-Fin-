using System;
using System.Collections;
using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 속성별 마법을 구현하는 클래스
/// 주의 : 속성 마법만 포함하고, 쉴드/텔레포트 등은 다른 클래스에서 처리
/// 작성자 - 차영철
/// </summary>
public class PlayerMagic : MonoBehaviour
{
    private enum MagicState
    {
        None,
        Grip,
        Charging,
    }

    [SerializeField, Tooltip("오른손 Transform")]
    private Transform rightHandTransform;

    [SerializeField, Tooltip("완드")] 
    private GameObject wandObj;

    [SerializeField, Tooltip("마법이 발사되는 위치")]
    private Transform MagicFirePositionTr;

    [SerializeField, Tooltip("손 모션 설정")] 
    private HandController handController;

    [SerializeField, Tooltip("마법 위치 표시")] 
    private GameObject magicIndicator;
    private Vector3 targetPos;

    [Header("Base Magic")] 
    [SerializeField, EnumNamedArray(typeof(ElementType))]
    [Tooltip("불/얼음/전기 속성 기본 마법")]
    private Magic[] baseMagicPrefabs = new Magic[(int)ElementType.None];

    [Header("Grip")] 
    [SerializeField, EnumNamedArray(typeof(ElementType))]
    [Tooltip("불/얼음/전기 속성 그립 마법")] 
    private GripMagic[] gripMagics = new GripMagic[(int)ElementType.None];

    [Header("Charge")] 
    [SerializeField] 
    private ChargeEffect chargeEffect;
    [SerializeField, EnumNamedArray(typeof(ElementType))]
    [Tooltip("불/얼음/전기 속성 차지 마법")]
    private Magic[] chargeMagicPrefabs = new Magic[(int)ElementType.None];

    private MagicState magicState = MagicState.None;

    private PoolSystem poolSystem;

    // 현재 마법 속성
    public ElementType CurrentElement { get; private set; } = ElementType.Fire;

    public event Action<ElementType> onChangeElement;

    private void Start()
    {
        poolSystem = PoolSystem.Instance;
        Debug.Assert(poolSystem, "Error : Pool System is not created");

        // 발사할 마법의 개수를 미리 지정해놓는다
        for (int i = 0; i < (int)ElementType.None; i++)
        {
            poolSystem.InitPool(baseMagicPrefabs[i], baseMagicPrefabs[i].PoolSize);
            poolSystem.InitPool(chargeMagicPrefabs[i], chargeMagicPrefabs[i].PoolSize);
        }

        // 손 모양 지정
        handController.SetRightHandAction(HandController.RightAction.WandGrip);
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
        if (magicState == MagicState.Grip)
        {
            TurnOffGrip();
        }
        // 속성 바꾸면 차지 풀림
        if (magicState == MagicState.Charging)
        {
            EndCharge();
        }

        int addedElementIndex = changeNextElement ? 1 : -1;
        int currentElementNum = ((int)CurrentElement + addedElementIndex) % (int)ElementType.None;
        if (currentElementNum < 0)
        {
            currentElementNum += (int)ElementType.None;
        }
        CurrentElement = (ElementType)currentElementNum;

        onChangeElement?.Invoke(CurrentElement);
    }

    #region Base Magic
    // 기본 마법 발사
    public void ShootMagic(Vector3 position, Vector3 direction)
    {
        // 그립/차지 중이거나 쿨타임이면 무시
        if (magicState != MagicState.None)
        {
            return;
        }

        // 지정된 속성의 마법을 발사
        var magic = poolSystem.GetInstance<Magic>(baseMagicPrefabs[(int)CurrentElement]);
        magic.SetPosition(MagicFirePositionTr.position);
        magic.SetDirection(direction);
        magic.StartMagic();
    }
    #endregion

    #region Charge Magic
    // 마법 충전이 완료됐을 때 이펙트
    public void StartCharge()
    {
        chargeEffect.gameObject.SetActive(true);
        chargeEffect.SetColor(CurrentElement);
        magicState = MagicState.Charging;
    }

    // 차지 충전 중 보여줄 정보
    public void OnCharge()
    {
        if (!chargeEffect.ChargeCompleted || 
            chargeMagicPrefabs[(int)CurrentElement].IsSelfTarget)
        {
            magicIndicator.SetActive(false);
            return;
        }

        // 타겟 지정형 마법이면 마법진 그리기
        RaycastHit hit;
        if (Physics.Raycast(rightHandTransform.position, rightHandTransform.forward, out hit,
                20, 1 << LayerMask.NameToLayer("Default")))
        {
            // 마법진 그리기
            if (!magicIndicator.activeSelf)
            {
                magicIndicator.SetActive(true);
            }

            targetPos = hit.point + Vector3.up * 0.05f;
            magicIndicator.transform.position = targetPos;
            magicIndicator.transform.forward = Vector3.up;
        }
    }

    public void EndCharge()
    {
        if (chargeEffect.ChargeCompleted)
        {
            // 현재 속성의 마법 시전
            var magic = poolSystem.GetInstance<Magic>(chargeMagicPrefabs[(int)CurrentElement]);
            magic.SetPosition(magic.IsSelfTarget ? transform.position : targetPos);
            magic.StartMagic();
        }
        chargeEffect.gameObject.SetActive(false);
        magicIndicator.SetActive(false);
        magicState = MagicState.None;
    }

    #endregion

    #region Grip Magic
    public void TurnOnGrip()
    {
        if (magicState != MagicState.None)
        {
            return;
        }

        // 마나 닳게 처리
        int currentElement = (int)CurrentElement;
        Debug.Assert(gripMagics[currentElement], $"Error : grip Magic - {CurrentElement} not set");
        gripMagics[currentElement].gameObject.SetActive(true);
        gripMagics[currentElement].TurnOn();
        magicState = MagicState.Grip;

        // 얼음검일 때 손 보정
        if (CurrentElement == ElementType.Ice)
        {
            handController.SetRightHandAction(HandController.RightAction.SwordGrip);
            wandObj.SetActive(false);
        }
    }

    public void TurnOffGrip()
    {
        int currentElement = (int)CurrentElement;
        if (magicState == MagicState.Grip && gripMagics[currentElement])
        {
            gripMagics[currentElement].TurnOff();
            magicState = MagicState.None;
            handController.SetRightHandAction(HandController.RightAction.WandGrip);
            wandObj.SetActive(true);
        }
    }
    #endregion
}
