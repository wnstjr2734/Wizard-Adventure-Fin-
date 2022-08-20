using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 앞 방향으로 날아가게 만든다.
// 물체에 닿았을 때 사라지게 만들고, 그 자리에 이펙트를 넣는다.

public class MagicBolt : MonoBehaviour
{
    public float speed = 0.7f;
    public GameObject explosionFactory;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        //this.transform.position = target.transform.position * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        explosionFactory.transform.position = this.transform.position;
        Destroy(gameObject, 2f);
    }
}
