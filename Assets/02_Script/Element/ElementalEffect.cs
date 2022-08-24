using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

/// <summary>
/// ���� �������� ���� ȿ�� VFX ���� Ŭ����
/// �ۼ��� - ����ö
/// </summary>
public class ElementalEffect : MonoBehaviour
{
    private ParticleSystem[] effects;

    private void Awake()
    {
        effects = GetComponentsInChildren<ParticleSystem>();
    }

    public void SetEffectTarget(Transform target)
    {
        var renderer = target.GetComponent<Renderer>();
        if (!renderer)
        {
            Debug.LogError("Error : There is No Renderer");
            return;
        }

        ParticleSystemShapeType shapeType;
        if (renderer is MeshRenderer)
        {
            shapeType = ParticleSystemShapeType.MeshRenderer;
        }
        else if (renderer is SkinnedMeshRenderer)
        {
            shapeType = ParticleSystemShapeType.MeshRenderer;
        }
        else
        {
            Debug.LogError("Error : Unknown Renderer Type. Can't Matching");
            return;
        }

        foreach (var effect in effects)
        {
            var shape = effect.shape;
            shape.shapeType = shapeType;
        }

        // ����� ȿ���� �������� 
        // ����� ������ ����Ʈ �����ؾ��Ѵ�

    }
}
