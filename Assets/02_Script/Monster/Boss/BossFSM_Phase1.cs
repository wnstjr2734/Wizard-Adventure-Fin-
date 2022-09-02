using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 보스 1페이즈 코드
/// </summary>
public partial class BossFSM : MonoBehaviour
{
    public enum Phase1
    {
        Skill1,
    }

    [System.Serializable]
    private class Phase1_Skill1Data
    {
        [SerializeField, Tooltip("투사체 날릴 개수")]
        private float magicMissileCount = 3;

        [SerializeField, Tooltip("투사체 발사 간격")] 
        private float interval = 0.3f;

        [SerializeField, Tooltip("투사체 마법")] 
        private DelayedMagic delayedMagic;
    }

    [System.Serializable]
    private class Phase1_Skill2Data
    {
        [SerializeField, Tooltip("")] 
        private float chargingTime = 3.0f;

        [SerializeField, Tooltip("차징 이펙트")] 
        private ParticleSystem chargingEffect;

        [SerializeField, Tooltip("발사 이펙트")] 
        private GameObject laserEffect;

        public float ChargingTime => chargingTime;
        public ParticleSystem ChargingEffect => chargingEffect;
        public GameObject LaserEffect => laserEffect;
    }

    [System.Serializable]
    private class Phase1_Skill3Data
    {
        [SerializeField, Tooltip("유저 근처로 미사일 날릴 확률")]
        private float shootToUserPercent = 0.3f;

        //[SerializeField, Tooltip("미사일 위치 표시 이펙트")]
        //private 
    }

    [System.Serializable]
    private class Phase1_Skill4Data
    {
        [SerializeField, Tooltip("기 모으는 이펙트")]
        private ParticleSystem chargingEffect;

        //[SerializeField, Tooltip("")]
        //private FieldMagic 
    }

    [Header("Phase 1")]
    [SerializeField, Tooltip("Phase 1에 쓸 스킬 시간 정보")]
    private BossSkillData[] phase1SkillDatas = new BossSkillData[4];

    [SerializeField]
    private Phase1_Skill1Data phase1Skill1;
    [SerializeField]
    private Phase1_Skill2Data phase1Skill2;
    [SerializeField]
    private Phase1_Skill3Data phase1Skill3;
    [SerializeField]
    private Phase1_Skill4Data phase1Skill4;

    private Coroutine actionDelayCoroutine = null;

    private static readonly int inChargeID = Animator.StringToHash("InCharge");


    private IEnumerator IEPhase1_DecreaseCooldown()
    {
        while (true)
        {
            for (int i = 0; i < phase1SkillDatas.Length; i++)
            {
                phase1SkillDatas[i].CurrentCooldown -= actionTime;
            }
            yield return actionWS;
        }
    }

    public void Phase1_JudgeAction()
    {
        if (IsPlayerNearby() && phase1SkillDatas[3].CurrentCooldown <= 0)
        {
            Phase1_UseSkill(3);
            return;
        }

        int minCooldownNum = 0;
        float minCooldown = phase1SkillDatas[0].CurrentCooldown;
        for (int i = 0; i < 3; i++)
        {
            if (phase1SkillDatas[i].CurrentCooldown < minCooldown)
            {
                minCooldown = phase1SkillDatas[i].CurrentCooldown;
                minCooldownNum = i;
            }
        }

        // 모두 쿨다운이면 모든 스킬의 쿨다운을 전부 줄여놓는다
        if (minCooldown > 0)
        {
            for (int i = 0; i < phase1SkillDatas.Length; i++)
            {
                phase1SkillDatas[i].CurrentCooldown -= minCooldown;
            }
        }
        else
        {
            Phase1_UseSkill(minCooldownNum);
        }
    }

    private void Phase1_UseSkill(int skillNum)
    {
        var randomCooldown = UnityEngine.Random.Range(
            phase1SkillDatas[skillNum].Cooldown.x, phase1SkillDatas[skillNum].Cooldown.y);
        phase1SkillDatas[skillNum].CurrentCooldown = randomCooldown;
        animator.SetInteger(skillStateID, skillNum);
    }

    private bool IsPlayerNearby()
    {
        return Vector3.Distance(transform.position, attackTarget.position) < 3;
    }

    private void Phase1_NextActionDelay()
    {
        var skillState = animator.GetInteger(skillStateID);
        if (actionDelayCoroutine == null)
        {
            actionDelayCoroutine = StartCoroutine(IEWaitActionDelay(phase1SkillDatas[skillState].NextActionDelay));
        }
    }

    private IEnumerator IEWaitActionDelay(float delay)
    {
        animator.SetBool(isActionDelayID, true);
        yield return new WaitForSeconds(delay);
        animator.SetBool(isActionDelayID, false);
        actionDelayCoroutine = null;
    }
    
    private IEnumerator IEPhase1_Skill2_Aim()
    {
        animator.SetBool(inChargeID, true);
        float currentTime = 0;
        while (currentTime < phase1Skill2.ChargingTime)
        {
            // 몸체 돌아가기
            var targetPos = attackTarget.position;
            targetPos.y = transform.position.y;
            transform.forward = (targetPos - transform.position).normalized;

            currentTime += Time.deltaTime;
            yield return null;
        }
        animator.SetBool(inChargeID, false);
    }

    private void Phase1_Skill2_ShootLaser()
    {

    }

    private void Phase1_End()
    {
        // 다 캔슬하고 보스 쓰러짐 호출
        // 보스 피 회복
        charStatus.ResetStatus();

        // 페이즈 1 끝내기
        charStatus.onDead -= Phase1_End;


        // 페이즈 2 시작
    }
}
