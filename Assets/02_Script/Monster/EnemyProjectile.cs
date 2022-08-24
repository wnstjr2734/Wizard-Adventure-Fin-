using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스켈레톤 소서러가 날리는 투사체에 대한 정보
/// 작성자 - 성종현
/// </summary>
public class EnemyProjectile : MonoBehaviour
{
    private Animation animator;
    private CharacterStatus charStatus;
    [SerializeField, Tooltip("투사체 이동속도")]
    private float moveSpeed = 4.5f;
    [SerializeField, Tooltip("투사체 사정거리")]
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
        // 수명 제어
        lifetime -= Time.fixedDeltaTime;
        if (lifetime < 0)
        {
            Destroy();
        }
    }

        private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
        // 부딪힌 대상이 Player일 때 데미지를 준다.
        if(collision.gameObject.name == "Player")
        {
            charStatus = GetComponent<CharacterStatus>();
            
        }
        // 삭제
        Destroy();
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }
}
