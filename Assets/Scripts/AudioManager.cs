using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip bgmusic;
    public AudioSource bgmAudioSource;
    public AudioClip start;

    public AudioClip sfx;
    public AudioSource sfxAudioSource;

    void Start()
    {
        bgmAudioSource.PlayOneShot(start);
        bgmAudioSource.clip = bgmusic;
        bgmAudioSource.Play();
    }

    public void PlaySFX()
    {
        sfxAudioSource.clip = sfx;
        sfxAudioSource.PlayOneShot(sfx);
    }

    public void SetMusicVolume(float volume)
    {
        bgmAudioSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxAudioSource.volume = volume;
    }
}
