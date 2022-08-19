using System;
using System.Collections;
using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using UnityEngine;

/// <summary>
/// �Ӽ��� ������ �����ϴ� Ŭ����
/// ���� : �Ӽ� ������ �����ϰ�, ����/�ڷ���Ʈ ���� �ٸ� Ŭ�������� ó��
/// �ۼ��� - ����ö
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
    [Tooltip("��/����/���� �Ӽ� �⺻ ����")]
    private Magic[] baseMagicPrefabs = new Magic[(int)ElementType.Count];

    [Header("Grip")] 
    [SerializeField, EnumNamedArray(typeof(ElementType))]
    [Tooltip("��/����/���� �Ӽ� �׸� ����")] 
    private GripMagic[] gripMagics = new GripMagic[(int)ElementType.Count];

    [Header("Charge")] 
    [SerializeField] 
    private ChargeEffect chargeEffect;
    [SerializeField, EnumNamedArray(typeof(ElementType))]
    [Tooltip("��/����/���� �Ӽ� ���� ����")]
    private Magic[] chargeMagicPrefabs = new Magic[(int)ElementType.Count];

    [SerializeField, Tooltip("������ �߻�Ǵ� ��ġ")]
    private Transform MagicFirePositionTr;

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
        poolSystem.InitPool(baseMagicPrefabs[(int)ElementType.Fire], 3);
        poolSystem.InitPool(baseMagicPrefabs[(int)ElementType.Ice], 2);
        poolSystem.InitPool(baseMagicPrefabs[(int)ElementType.Lightning], 4);   // ���� �����̶� ��ĥ �� ����

        poolSystem.InitPool(chargeMagicPrefabs[(int)ElementType.Ice], 1);
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
        CurrentElement = (ElementType)(((int)CurrentElement + addedElementIndex) % (int)ElementType.Count);

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

    private void OnCharge()
    {
        // ���� ���� �� ������ ����
    }

    public void EndCharge()
    {
        if (chargeEffect.ChargeCompleted)
        {
            // ���� �Ӽ��� ���� ����
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

    // ������ �ִ� ���� ������ ���� ������

    public void TurnOnGrip()
    {
        // ���� ��� ó��
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

    // ȭ��, ���� �� �׷� ���� �� �����ϴ� 
    private void OnGrip()
    {

    }

    #endregion
}
