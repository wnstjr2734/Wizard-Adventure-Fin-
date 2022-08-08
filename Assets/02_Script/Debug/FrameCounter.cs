using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// UI에 현재 프레임 레이트를 보여준다
/// 작성자 : 차영철
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
