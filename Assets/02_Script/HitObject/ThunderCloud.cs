using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// ���� �ð����� ���� �� ����� �����ϴ� ����
/// �ۼ��� - ����ö
/// </summary>
public class ThunderCloud : Magic
{
    private float maxHeight = 15;
    private float circleSize = 10;

    [SerializeField] 
    private ParticleSystem[] flareParticles;
    private Queue<ParticleSystem> flareParticlesQueue = new Queue<ParticleSystem>();

    [SerializeField] 
    private LightningBolt[] lightningBolts;
    private Queue<LightningBolt> lightningBoltQueue = new Queue<LightningBolt>();

    private void Awake()
    {
        foreach (var flareParticle in flareParticles)
        {
            flareParticlesQueue.Enqueue(flareParticle);
        }
        foreach (var lightningBolt in lightningBolts)
        {
            lightningBoltQueue.Enqueue(lightningBolt);
        }
    }

    private void OnEnable()
    {
        foreach (var flareParticle in flareParticles)
        {
            flareParticle.gameObject.SetActive(false);
        }
        foreach (var lightningBolt in lightningBolts)
        {
            lightningBolt.gameObject.SetActive(false);
        }
    }

    public override void StartMagic()
    {
        RevisePosition();
        StartCoroutine(IEThunderAttack());
    }

    // õ�� ���� ��ġ�� �����ǵ��� �����Ѵ�
    // ���� õ���� �ʹ� ���ٸ� �ִ� ���̿� �����ǵ��� �����Ѵ�
    private void RevisePosition()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit,
                maxHeight, LayerMask.NameToLayer("Default")))
        {
            // õ�� ���� �Ʒ� 1m ��ġ�� �����ǰ� ����
            transform.position = hit.point + Vector3.down;
        }
        else
        {
            transform.position = transform.position + Vector3.up * maxHeight;
        }
    }

    private IEnumerator IEThunderAttack()
    {
        float waitTime = 0.25f;
        int attackCount = 10;
        for (int i = 0; i < attackCount; i++)
        {
            Thunder();
            yield return new WaitForSeconds(waitTime);
        }

        gameObject.SetActive(false);
    }

    // ���� ���� ���� ���� �� ��ġ�� �����Ǽ� ���� ���� �õ�
    private void Thunder()
    {
        var circlePos = Random.insideUnitCircle * circleSize;
        Vector3 startPos = new Vector3(transform.position.x + circlePos.x, transform.position.y, transform.position.z + circlePos.y);

        // Flare ������ ��ġ ���� �� ����
        var flare = flareParticlesQueue.Dequeue();
        flareParticlesQueue.Enqueue(flare);
        flare.gameObject.SetActive(true);
        flare.transform.position = startPos + Vector3.down;

        var lightning = lightningBoltQueue.Dequeue();
        lightningBoltQueue.Enqueue(lightning);
        lightning.gameObject.SetActive(true);
        lightning.SetPosition(startPos);
        lightning.SetDirection(Vector3.down);
        lightning.StartMagic();
    }
}
