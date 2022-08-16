using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    None,
    Fire,
    Ice,
    Lightning
}

/// <summary>
/// 속성 효과 정의 클래스
/// 속성에 필요한 정보를 Singleton 형식으로 정의하여 가져다 사용
/// 작성자 - 차영철
/// </summary>
[CreateAssetMenu(fileName = "Element Info", menuName = "Game Data/Element Info")]
public class ElementInfo : ScriptableObject
{
    /// <summary>
    /// 불 속성은 화상 데미지를 추가로 입힘
    /// 화상 데미지 - 주기적으로 데미지 입히기
    /// </summary>
    [Serializable]
    public class Fire
    {
        [Tooltip("최대 스택")]
        public int maxStack;
        [Tooltip("지속 시간")]
        public float duration;
        [Tooltip("초기 화상 데미지")]
        public int initDamage;
        [Tooltip("스택 당 추가 화상 데미지")]
        public int stackBonusDamage;
        [Tooltip("화상 몇 초에 한 번 입힐지")]
        public float interval;

        public int BurnDamage(int stack)
        {
            return initDamage + (stack - 1) * stackBonusDamage;
        }
    }

    [Serializable]
    public class Ice
    {
        [Tooltip("최대 스택")]
        public int maxStack;
        [Tooltip("지속 시간")]
        public float duration;
        [Tooltip("초기 슬로우 정도")]
        public float initSlowRate;
        [Tooltip("추가 슬로우 정도")] 
        public float stackBonusSlow;
    }

    [Serializable]
    public class Lightning
    {
        // 경직 보너스
        //public float 
    }

    public static ElementInfo Instance { get; private set; }

    [SerializeField] 
    private Fire fireInfo;
    [SerializeField]
    private Ice iceInfo;
    [SerializeField]
    private Lightning lightningInfo;

    // 외부에서 읽기만 가능하도록 getter 만들어서 사용
    public Fire FireInfo => fireInfo;
    public Ice IceInfo => iceInfo;
    public Lightning LightningInfo => lightningInfo;

    private void OnEnable()
    {
        Instance = this;
        Debug.Log("Element Info Init");
    }
}