using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{


    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private AudioClip deathSoundClip;

    [SerializeField] private AudioSource audioSource;

    public void Play()
    {
        audioSource.clip = sounds[Random.Range(0, sounds.Length - 1)];
        audioSource.Play();
    }
    
    public void PlayDeathSound()
    {
        audioSource.clip = deathSoundClip;
        audioSource.Play();
    }

    
}
