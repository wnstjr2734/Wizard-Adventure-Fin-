using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// UI�� ���� ������ ����Ʈ�� �����ش�
/// �ۼ��� : ����ö
/// </summary>
public class FrameCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI frameCountText;

    void Update()
    {
        int currentFrame = (int)(1.0f / Time.unscaledDeltaTime);
        frameCountText.text = currentFrame + " FPS";
    }
}
