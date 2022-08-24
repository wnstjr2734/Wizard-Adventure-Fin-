using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ü�� ��� Enemy(Archer, Sorcerer)�� �߻�����
/// �ۼ��� - ������
/// </summary>
public class EnemyShooting : MonoBehaviour
{
    private Animator animator;
    public GameObject projFactory;
    public GameObject firePos;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void ProjectileShooting()
    {
        GameObject projInst = Instantiate(projFactory);
        projInst.transform.position = firePos.transform.position;
        projInst.transform.rotation = firePos.transform.rotation;
        ParticleSystem ps = projInst.GetComponent<ParticleSystem>();
        ps.Stop();
        ps.Play();


    }
}