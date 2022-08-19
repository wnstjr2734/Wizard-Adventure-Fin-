using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShooting : MonoBehaviour
{
    private Animation animator;
    public GameObject fxFactory;
    public GameObject firePos;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ArrowShoot()
    {
        GameObject fxInst = Instantiate(fxFactory);
        fxInst.transform.position = firePos.transform.position;
        fxInst.transform.rotation = firePos.transform.rotation;
        //ParticleSystem ps = fxInst.GetComponent<ParticleSystem>();
        //ps.Stop();
        //ps.Play();
        
    }

}
