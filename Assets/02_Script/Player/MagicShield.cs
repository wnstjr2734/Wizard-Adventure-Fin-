using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ü ������ �����ؼ� ���� ���
/// �ۼ��� - ����ö
/// </summary>
public class MagicShield : MonoBehaviour
{
    [SerializeField, Tooltip("����ü ������ �� ���� �ð� �� ����")]
    private Vector2 blockVibrationValue = new Vector2(0.1f, 0.2f);

    [SerializeField, Tooltip("����ü�� ������ �� ����Ʈ")]
    private GameObject blockEffectPrefab;
    
    private void Start()
    {
        // �ִ� 3�� ���� �����Ŷ� ����
        PoolSystem.Instance.InitPool(blockEffectPrefab, 3);
    }

    private void OnCollisionEnter(Collision collision)
    {
        print($"Collision Enter - {collision.gameObject.name}");
        // ������ �� ���� �ֱ�
        StartCoroutine(nameof(IEApplyVibration));
        CreateBlockEffect(collision.contacts[0].point, collision.contacts[0].normal);
    }

    private IEnumerator IEApplyVibration()
    {
        OVRInput.SetControllerVibration(blockVibrationValue.x, blockVibrationValue.y, OVRInput.Controller.LTouch);
        yield return new WaitForSeconds(blockVibrationValue.x);
        OVRInput.SetControllerVibration(blockVibrationValue.x, blockVibrationValue.y, OVRInput.Controller.None);
    }

    private void CreateBlockEffect(Vector3 position, Vector3 normal)
    {
        var blockEffect = PoolSystem.Instance.GetInstance<GameObject>(blockEffectPrefab);
        blockEffect.transform.position = position;
        blockEffect.transform.forward = normal;
    }
}
