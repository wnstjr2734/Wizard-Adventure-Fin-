using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// ����ü ������ �����ؼ� ���� ���
/// �ۼ��� - ����ö
/// </summary>
public class MagicShield : MonoBehaviour
{
    [SerializeField, Tooltip("���� ����/���� �� ������ CutOut - �ʱ� / Ȱ��ȭ")]
    private Vector2 cutoutValue = new Vector2(1.0f, 0.5f);

    [SerializeField, Tooltip("����ü ������ �� ���� �ð� �� ����")]
    private Vector3 blockVibrationValue = new Vector3(0.1f, 0.2f, 0.2f);

    [SerializeField, Tooltip("����ü�� ������ �� ����Ʈ")]
    private GameObject blockEffectPrefab;

    [SerializeField]
    private Material shieldObjMat;
    private readonly int cutoutID = Shader.PropertyToID("_CutOut");

    private void OnEnable()
    {
        shieldObjMat.SetFloat(cutoutID, cutoutValue.x);
        shieldObjMat.DOFloat(cutoutValue.y, cutoutID, 0.3f);
    }

    private void Start()
    {
        // �ִ� 3�� ���� �����Ŷ� ����
        PoolSystem.Instance.InitPool(blockEffectPrefab, 3);
    }

    /// <summary>
    /// ���� ��Ȱ��ȭ �� ���
    /// </summary>
    public void TurnOff()
    {
        Sequence s = DOTween.Sequence();
        s.Append(shieldObjMat.DOFloat(cutoutValue.x, cutoutID, 0.3f));
        s.onComplete = () => gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        print($"Collision Enter - {collision.gameObject.name}");
        // ������ �� ���� �ֱ�
        VibrationManager.Instance.SetVibration(blockVibrationValue.x,
            blockVibrationValue.y, blockVibrationValue.z, VibrationManager.ControllerType.LeftTouch);
        CreateBlockEffect(collision.contacts[0].point, collision.contacts[0].normal);
    }

    private void CreateBlockEffect(Vector3 position, Vector3 normal)
    {
        var blockEffect = PoolSystem.Instance.GetInstance<GameObject>(blockEffectPrefab);
        blockEffect.transform.position = position;
        blockEffect.transform.forward = normal;
    }
}
