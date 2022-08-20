using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Gun : MonoBehaviour
{
    EnemyFSM enemy;
    void Start()
    {
        //enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyFSM>();
    }


    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.CompareTag("Enemy"))
        if(other.CompareTag("Enemy"))
        {
            print(other);
            print("==========Hit==========");
            other.GetComponent<EnemyFSM>().OnDamaged(1);
            //enemy.OnDamaged(1);
        }
    }
}
