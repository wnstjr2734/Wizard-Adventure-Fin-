using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� 1������ �ڵ�
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
        [SerializeField, Tooltip("����ü ���� ����")]
        private float magicMissileCount = 3;

        [SerializeField, Tooltip("����ü �߻� ����")] 
        private float interval = 0.3f;

        [SerializeField, Tooltip("����ü ����")] 
        private DelayedMagic delayedMagic;
    }

    [System.Serializable]
    private class Phase1_Skill2Data
    {
        [SerializeField, Tooltip("")] 
        private float chargingTime = 3.0f;

        [SerializeField, Tooltip("��¡ ����Ʈ")] 
        private ParticleSystem chargingEffect;

        [SerializeField, Tooltip("�߻� ����Ʈ")] 
        private GameObject laserEffect;

        public float ChargingTime => chargingTime;
        public ParticleSystem ChargingEffect => chargingEffect;
        public GameObject LaserEffect => laserEffect;
    }

    [System.Serializable]
    private class Phase1_Skill3Data
    {
        [SerializeField, Tooltip("���� ��ó�� �̻��� ���� Ȯ��")]
        private float shootToUserPercent = 0.3f;

        //[SerializeField, Tooltip("�̻��� ��ġ ǥ�� ����Ʈ")]
        //private 
    }

    [System.Serializable]
    private class Phase1_Skill4Data
    {
        [SerializeField, Tooltip("�� ������ ����Ʈ")]
        private ParticleSystem chargingEffect;

        //[SerializeField, Tooltip("")]
        //private FieldMagic 
    }

    [Header("Phase 1")]
    [SerializeField, Tooltip("Phase 1�� �� ��ų �ð� ����")]
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

        // ��� ��ٿ��̸� ��� ��ų�� ��ٿ��� ���� �ٿ����´�
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
            // ��ü ���ư���
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
        // �� ĵ���ϰ� ���� ������ ȣ��
        // ���� �� ȸ��
        charStatus.ResetStatus();

        // ������ 1 ������
        charStatus.onDead -= Phase1_End;


        // ������ 2 ����
    }
}
