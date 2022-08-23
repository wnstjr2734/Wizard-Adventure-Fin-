using System;
using System.Collections;
using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// �Ӽ��� ������ �����ϴ� Ŭ����
/// ���� : �Ӽ� ������ �����ϰ�, ����/�ڷ���Ʈ ���� �ٸ� Ŭ�������� ó��
/// �ۼ��� - ����ö
/// </summary>
public class PlayerMagic : MonoBehaviour
{
    [FormerlySerializedAs("rightHandTransfrom")] [FormerlySerializedAs("rightHandTr")] [SerializeField, Tooltip("������ Transform")]
    private Transform rightHandTransform;

    [SerializeField, Tooltip("������ �߻�Ǵ� ��ġ")]
    private Transform MagicFirePositionTr;

    [SerializeField, Tooltip("���� ��ġ ǥ��")] 
    private GameObject magicIndicator;
    private Vector3 targetPos;

    [Header("Base Magic")] 
    [SerializeField, EnumNamedArray(typeof(ElementType))]
    [Tooltip("��/����/���� �Ӽ� �⺻ ����")]
    private Magic[] baseMagicPrefabs = new Magic[(int)ElementType.None];

    [Header("Grip")] 
    [SerializeField, EnumNamedArray(typeof(ElementType))]
    [Tooltip("��/����/���� �Ӽ� �׸� ����")] 
    private GripMagic[] gripMagics = new GripMagic[(int)ElementType.None];

    [Header("Charge")] 
    [SerializeField] 
    private ChargeEffect chargeEffect;
    [SerializeField, EnumNamedArray(typeof(ElementType))]
    [Tooltip("��/����/���� �Ӽ� ���� ����")]
    private Magic[] chargeMagicPrefabs = new Magic[(int)ElementType.None];

    private PoolSystem poolSystem;

    // ���� ���� �Ӽ�
    public ElementType CurrentElement { get; private set; } = ElementType.Fire;

    public event Action<ElementType> onChangeElement;

    private void Awake()
    {
        
    }

    private void Start()
    {
        poolSystem = PoolSystem.Instance;
        Debug.Assert(poolSystem, "Error : Pool System is not created");

        // �߻��� ������ ������ �̸� �����س��´�
        for (int i = 0; i < (int)ElementType.None; i++)
        {
            poolSystem.InitPool(baseMagicPrefabs[i], baseMagicPrefabs[i].PoolSize);
            poolSystem.InitPool(chargeMagicPrefabs[i], chargeMagicPrefabs[i].PoolSize);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="changeNextElement">
    /// ���� ���ҷ� �ٲ��� ���� ���ҷ� �ٲ��� ����
    /// ex) changeNextElement = true : �� -> ���� -> ���� -> ��
    /// ex) changeNextElement = false : �� -> ���� -> ���� -> ��
    /// </param>
    public void ChangeElement(bool changeNextElement)
    {
        // �Ӽ� �ٲٸ� �׸� Ǯ��
        // �Ӽ� �ٲٸ� ���� Ǯ��

        int addedElementIndex = changeNextElement ? 1 : -1;
        int currentElementNum = ((int)CurrentElement + addedElementIndex) % (int)ElementType.None;
        CurrentElement = (ElementType)currentElementNum;

        onChangeElement?.Invoke(CurrentElement);
    }

    #region Base Magic
    // �⺻ ���� �߻�
    public void ShootMagic(Vector3 position, Vector3 direction)
    {
        // ��Ÿ���̸� ����

        // ������ �Ӽ��� ������ �߻�
        var magic = poolSystem.GetInstance<Magic>(baseMagicPrefabs[(int)CurrentElement]);
        magic.SetPosition(position);
        magic.SetDirection(direction);
        magic.StartMagic();
    }
    #endregion

    #region Charge Magic

    // ���� ������ �Ϸ���� �� ����Ʈ
    // 
    public void StartCharge()
    {
        chargeEffect.gameObject.SetActive(true);
        chargeEffect.SetColor(CurrentElement);
    }

    // ���� ���� �� ������ ����
    public void OnCharge()
    {
        if (!chargeEffect.ChargeCompleted || 
            chargeMagicPrefabs[(int)CurrentElement].IsSelfTarget)
        {
            magicIndicator.SetActive(false);
            return;
        }

        // Ÿ�� ������ �����̸� ������ �׸���
        RaycastHit hit;
        if (Physics.Raycast(rightHandTransform.position, rightHandTransform.forward, out hit,
                20, 1 << LayerMask.NameToLayer("Default")))
        {
            // ������ �׸���
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
            // ���� �Ӽ��� ���� ����
            var magic = poolSystem.GetInstance<Magic>(chargeMagicPrefabs[(int)CurrentElement]);
            magic.SetPosition(magic.IsSelfTarget ? transform.position : targetPos);
            magic.StartMagic();
        }
        chargeEffect.gameObject.SetActive(false);
        magicIndicator.SetActive(false);
    }

    #endregion

    #region Grip Magic

    // ������ �ִ� ���� ������ ���� ������

    public void TurnOnGrip()
    {
        // ���� ��� ó��
        if (gripMagics[(int)CurrentElement])
        {
            gripMagics[(int)CurrentElement].gameObject.SetActive(true);
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

    // ȭ��, ���� �� �׷� ���� �� �����ϴ� 
    private void OnGrip()
    {

    }

    #endregion
}
