using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boss_MainMenu : MonoBehaviour
{
    [SerializeField] GameObject boss;
    [SerializeField] Transform point;


    // Start is called before the first frame update
    void Start()
    {
        
        boss.transform.DOMove(point.position, 6.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
