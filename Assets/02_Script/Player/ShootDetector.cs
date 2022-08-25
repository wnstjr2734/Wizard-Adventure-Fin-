using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 마법을 쏘는 행동(손을 뻗음)을 했는지 감지
/// 작성자 - 차영철
/// </summary>
public class ShootDetector : MonoBehaviour
{
    [SerializeField, Tooltip("발사할 기본 마법 정보를 담은 클래스")] 
    private PlayerMagic playerMagic;

    [SerializeField, Tooltip("오른손 Transform")]
    private Transform rightHandTr;

    private void OnTriggerEnter(Collider other)
    {
        // 충돌 감지 - 유저가 바라보는 방향으로 손을 뻗었을 때 충돌 판정한다
        if (other.CompareTag("Right Hand"))
        {
            // 손의 각도가 기울어지는 경우 마법이 의도치 않은 방향으로 나갈 수 있다
            // 고로 손의 각도가 기울어지는 건 보정한다
            var localEuler = rightHandTr.localEulerAngles;
            localEuler.z = 0;

            var worldRot = rightHandTr.parent.rotation * Quaternion.Euler(localEuler);
            var revisedForward = worldRot * Vector3.forward;

            playerMagic.ShootMagic(other.transform.position, revisedForward);
        }
        //playerMagic.ShootMagic(other.transform.position, transform.forward);
    }
}
