using System;
using System.Collections;
using System.Collections.Generic;
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
        Lightning
    }

    // ���� ���� �Ӽ�
    public ElementType CurrentElement { get; private set; } = ElementType.Fire;

    [SerializeField] 
    private Projectile fireballPrefab;
    [SerializeField]
    private Projectile iceArrowPrefab;
    // ����Ʈ�� ��Ʈ

    [SerializeField, Tooltip("������ �߻�Ǵ� ��ġ")]
    private Transform MagicFirePositionTr;

    private PoolSystem poolSystem;

    private void Awake()
    {
        
    }

    private void Start()
    {
        poolSystem = PoolSystem.Instance;
        Debug.Assert(poolSystem, "Error : Pool System is not created");

        // �߻��� ������ ������ �̸� �����س��´�
        poolSystem.InitPool(fireballPrefab, 3);
        poolSystem.InitPool(iceArrowPrefab, 15);   // ���̽� �ַο�� �� �� ���� �𸣰���
    }
    
    #region Base Magic
    // �⺻ ���� �߻�
    public void ShootMagic(Vector3 position, Vector3 direction)
    {
        // ��Ÿ���̸� ����

        // ������ �Ӽ��� ������ �߻�
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
        // ���Ǹ��� ��Ѹ���
    }

    private IEnumerator IEShootIceArrow(Vector3 direction)
    {
        yield return null;
    }

    private void ShootLightningBolt(Vector3 direction)
    {
        // ��Ʈ ��ĵ ���
        // �ش� ������ �������� Raycast�ؼ� ���� ���
    }
    #endregion

    #region Charge Magic

    // ���� ������ �Ϸ���� �� ����Ʈ
    // 

    #endregion

    #region Focusing Magic

    // ������ �ִ� ���� ������ ���� ������
    // ���� ���� ��� 

    public void TurnOnCharge()
    {
        // 
    }

    public void TurnOffCharge()
    {

    }

    #endregion
}
