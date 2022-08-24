using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���̷��� �Ҽ����� ������ ����ü�� ���� ����
/// �ۼ��� - ������
/// </summary>
public class EnemyProjectile : MonoBehaviour
{
    private Animation animator;
    private CharacterStatus charStatus;
    [SerializeField, Tooltip("����ü �̵��ӵ�")]
    private float moveSpeed = 4.5f;
    [SerializeField, Tooltip("����ü �����Ÿ�")]
    private float range = 5f;
    private float lifetime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        lifetime = range / moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward* moveSpeed * Time.deltaTime;
        
    }

    private void FixedUpdate()
    {
        // ���� ����
        lifetime -= Time.fixedDeltaTime;
        if (lifetime < 0)
        {
            Destroy();
        }
    }

        private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
        // �ε��� ����� Player�� �� �������� �ش�.
        if(collision.gameObject.name == "Player")
        {
            charStatus = GetComponent<CharacterStatus>();
            
        }
        // ����
        Destroy();
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }
}
