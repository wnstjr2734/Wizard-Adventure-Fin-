using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BGMPlayer : Singleton<BGMPlayer>
{
    [SerializeField] private float fadeTime = 3.0f;
    [SerializeField, Range(0, 1)] 
    private float volumeSize = 1.0f;

    private AudioSource audioSource;

    protected override void OnAwake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // ���� �����ϰ� ���� ������ BGM �÷���
    public void PlayBGM(AudioClip newBGM)
    {
        audioSource.clip = newBGM;
        audioSource.Play();
    }

    // ������ �÷����ϴ� ������ �ٲ㼭 ���
    // �ٲٴ� ���� �� ������ Fade In / Fade Out
    public void Change(AudioClip newBGM)
    {
        Sequence s = DOTween.Sequence();
        s.Append(audioSource.DOFade(0, fadeTime));
        s.AppendCallback(() => {
            audioSource.Stop();
            audioSource.time = 0;
            audioSource.clip = newBGM;
            audioSource.Play();
        });
        s.Append(audioSource.DOFade(volumeSize, fadeTime));
    }
}
