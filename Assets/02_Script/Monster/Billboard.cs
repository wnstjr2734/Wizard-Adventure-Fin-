using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy ü�¹ٸ� ��� �������� ������ �ȹٷ� �� �� �ֵ��� �ϱ�
/// �ۼ��� - ������
/// </summary>
public class Billboard : MonoBehaviour
{
    Transform camTransform;
    public Transform attackTarget;
    // Start is called before the first frame update
    void Start()
    {
        //camTransform = Camera.main.transform;
    }

    void Update()
    {
        //transform.rotation = camTransform.rotation;
        this.transform.LookAt(attackTarget);
    }
}
