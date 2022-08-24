using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 투사체를 쏘는 Enemy(Archer, Sorcerer)의 발사정보
/// 작성자 - 성종현
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
