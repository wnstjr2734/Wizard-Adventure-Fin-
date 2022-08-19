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

    [Header("Base Magic")]
    [SerializeField] 
    private Projectile fireballPrefab;
    [SerializeField]
    private Projectile iceArrowPrefab;
    [SerializeField, Tooltip("��Ѹ��� ����")] 
    private int iceArrowShootCount = 5;
    [SerializeField]
    private LightningBolt lightningBolt;
    [SerializeField, Tooltip("����Ʈ�� ��Ʈ ��Ʈ��ų ���")] 
    private LayerMask lightningLayerMask;

    [Header("Grip")] 
    [SerializeField] 
    private IceSword iceSword;

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

        // �Ӽ� �ٲٸ� �׸� Ǯ��
        // �Ӽ� �ٲٸ� ���� Ǯ��
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
        bool rayHit = Physics.Raycast(position, direction, out var hit, 20, lightningLayerMask);
        Collider hitEnemy;
        // ��Ʈ ��ĵ ��� - ���� ���� �ʿ�
        if (rayHit)
        {
            // ����Ʈ�� ��Ʈ ��ġ ����
            hitEnemy = hit.collider;
        }
        // ���� �Ÿ��� ���� �� ���� ����� �༮�� �������� ��
        else
        {
            Vector3 endPosition = position + 20 * direction;
            // ���� ���߱�
            var enemies = Physics.OverlapCapsule(position, endPosition, 3, 
                1 << LayerMask.NameToLayer("Enemy"));
            if (enemies.Length == 0)
            {
                return;
            }
            
            // ���� ����� ���� ����
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

    // ���� ������ �Ϸ���� �� ����Ʈ
    // 

    #endregion

    #region Grip Magic

    // ������ �ִ� ���� ������ ���� ������

    public void TurnOnGrip()
    {
        // ���� ��� ó��

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
