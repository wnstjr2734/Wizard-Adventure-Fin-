using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpellTest : MonoBehaviour
{
    private Animator animator;
    public GameObject fxFactory;
    public GameObject firePos;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    animator.SetTrigger("MagicboltCast");
        //}
    }

    void MagicboltCast()
    {
        GameObject fxInst = Instantiate(fxFactory);
        fxInst.transform.position = firePos.transform.position;
        fxInst.transform.rotation = firePos.transform.rotation;
        ParticleSystem ps = fxInst.GetComponent<ParticleSystem>();
        ps.Stop();
        ps.Play();
    }
}
