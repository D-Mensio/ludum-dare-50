using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton
{
    [SerializeField]
    private AudioSource music;

    [SerializeField]
    private AudioSource sfx;

    public void ChangeMusic(AudioClip clip)
    {
        music.Stop();
        music.clip = clip;
        music.Play();
    }

    public void PlaySound(AudioClip clip, float volume = 1)
    {
        sfx.PlayOneShot(clip, volume);
    }

}
