using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

/// <summary>
/// 원소 공격으로 인한 효과 VFX 적용 클래스
/// 작성자 - 차영철
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
            shapeType = ParticleSystemShapeType.SkinnedMeshRenderer;
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
            shape.meshRenderer = renderer as MeshRenderer;
            shape.skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
        }
    }
}
