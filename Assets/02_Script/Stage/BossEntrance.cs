using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1. ���� �÷��̾ Ʈ���ſ� ����
//2. Boss Spawn ��ġ�� ��ġ�� �ٲ۴�.
/// <summary>
/// ������������ �Ա��� Ʈ���Ÿ� �ΰ� ���� ���ͷ� �̵� ��Ű��
/// ȿ�� �߰�
/// </summary>
public class BossEntrance : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject bossSpawnObj;
    [SerializeField] private Transform bossSpawn;
    //private Transform bossSpawn;

    [SerializeField] private float playerMoveTime = 3.0f;
    [SerializeField] private Transform playerMovePos;

    [SerializeField] private ParticleSystem teleportEffect;
    
    [SerializeField] private GameObject bossHp;

    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        bossSpawnObj = GameObject.Find("Boss Spawn");
        bossSpawn = bossSpawnObj.GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Contains("Player") == true)
        {
            print("Trigger Boss Entrance");
            StartCoroutine(IEBossIntro());
        }
    }

    private IEnumerator IEBossIntro()
    {
        //�÷��̾� ���� �� ����
        //    -�÷��̾ �� ä��� ������ �̵�
        //    (�ڷ���Ʈ ������ ���� �̵��� ����)
        //-�÷��̾� ���� ���Ƴ���
        //    -> �÷��̾� ���� �̵� - ���� ? ����
        //�÷��̾ ���� �����ϸ� �˹� ��Ű��
        //    -�����ϸ� ���� �ִ� ������ �˹�. �������� �� ����
        var player = GameManager.player;
        var playerController = player.GetComponent<PlayerController>();
        var playerMoveRotate = player.GetComponent<PlayerMoveRotate>();

        print(Time.timeScale);

        playerController.ActiveController(false);
        playerMoveRotate.ToMove(playerMovePos.position, playerMoveTime);

        yield return new WaitForSeconds(playerMoveTime);

        // ���� ����
        teleportEffect.Play(true);
        yield return new WaitForSeconds(0.2f);
        boss.transform.position = bossSpawn.position;

        // ���� ����
        var bossAnimator = boss.GetComponent<Animator>();
        bossAnimator.SetInteger("SkillState", 1);
        bossAnimator.SetInteger("Phase", 0);

        yield return new WaitForSeconds(2.0f);
        // ü�� �� �����ֱ�
        bossHp.SetActive(true);

        playerController.ActiveController(true);
    }
}
