using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertiesSelect : MonoBehaviour
{
    public GameObject[] pp = new GameObject[3];
    // Start is called before the first frame update
    void Start()
    {
        InProperties();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InProperties()
    {
        for (int i = 0; i < pp.Length; i++)
        {
            pp[i] = transform.GetChild(i).gameObject;
        }
        
    }

}
