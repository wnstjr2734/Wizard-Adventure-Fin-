using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� �������� ���ư��� �����.
// ��ü�� ����� �� ������� �����, �� �ڸ��� ����Ʈ�� �ִ´�.

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
