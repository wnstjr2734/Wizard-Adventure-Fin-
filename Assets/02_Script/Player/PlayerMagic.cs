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

    // ���� ���� �Ӽ�
    public ElementType CurrentElement { get; private set; } = ElementType.Fire;

    [SerializeField] 
    private Projectile fireballPrefab;
    [SerializeField]
    private Projectile iceArrowPrefab;
    [SerializeField, Tooltip("��Ѹ��� ����")] 
    private int iceArrowShootCount = 5;
    // ����Ʈ�� ��Ʈ
    private LightningBoltScript lightningBoltPrefab;

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
        poolSystem.InitPool(iceArrowPrefab, 3 * iceArrowShootCount);   // ���̽� �ַο�� �� �� ���� �𸣰���
    }

    #region Change Element

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
        int addedElementIndex = changeNextElement ? 1 : -1;
        CurrentElement = (ElementType)(((int)CurrentElement + addedElementIndex) % (int)ElementType.Count);
    }

    #endregion

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
        // ���Ǹ��� ��Ѹ���
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
        // ��Ʈ ��ĵ ���
        //if (Physics.Raycast(leftHandTransform.position, leftHandTransform.forward, out hit,
        //        teleportRange, 1 << LayerMask.NameToLayer("Default")))
        //{
        //    //print(hit.collider.name);
        //    // ���� �ڷ���Ʈ ���� �ʵ��� ����
        //    if (Vector3.Dot(hit.normal, Vector3.up) > 0.5f)
        //    {
        //        // Z-fighting�� �Ͼ�� �ʰ� �ڷ���Ʈ ��ġ ����
        //        teleportTarget.position = hit.point + Vector3.up * 0.05f;
        //        DrawTeleportLineCurve(footPos.localPosition, teleportTarget.localPosition);
        //    }
        //}
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
