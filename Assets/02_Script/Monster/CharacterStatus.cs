using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

/// <summary>
/// ĳ���� �ɷ�ġ Ŭ����
/// </summary>
public class CharacterStatus : MonoBehaviour
{
    public delegate void HpChangedHandler(int current, int max);

    [SerializeField]
    protected int maxHp = 100;
    protected int currentHp;

    protected bool isDead = false;

    public int CurrentHp
    {
        get => currentHp;
        set
        {
            currentHp = value;
            if (currentHp <= 0)
            {
                currentHp = 0;
                if (!isDead)
                {
                    isDead = true;
                    OnDead?.Invoke();
                }
            }
            else if (currentHp > maxHp)
            {
                currentHp = maxHp;
            }

            OnHpChanged?.Invoke(currentHp, maxHp);
        }
    }

    public int MaxHp
    {
        get => maxHp;
        set
        {
            // �ִ� ü���� �þ�� ��������ŭ ����ü�� ����
            int changedAmount = Math.Max(value - maxHp, 0);
            maxHp = value;
            CurrentHp += changedAmount;
            Debug.Assert(maxHp > 0, "Error - Max Hp is lower than 0");
            OnHpChanged?.Invoke(currentHp, maxHp);
        }
    }

    public event HpChangedHandler OnHpChanged;
    public event Action OnDead;
    public event Action<int> OnDamaged;
    public event Action<int> OnHealed;

    protected virtual void OnEnable()
    {
        currentHp = maxHp;
        isDead = false;
    }

    private void Start()
    {
        // �ʱ� UI ���ſ�
        CurrentHp = CurrentHp;
    }

    public virtual void TakeDamage(int amount)
    {
        CurrentHp -= amount;
        if (!isDead)
        {
            OnDamaged?.Invoke(amount);
        }
    }

    public virtual void TakeHeal(int amount)
    {
        CurrentHp += amount;
        if (!isDead)
        {
            OnHealed?.Invoke(amount);
        }
    }
}
