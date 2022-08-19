using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

/// <summary>
/// 캐릭터 능력치 클래스
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
            // 최대 체력이 늘어나면 증감량만큼 현재체력 증가
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
        // 초기 UI 갱신용
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
