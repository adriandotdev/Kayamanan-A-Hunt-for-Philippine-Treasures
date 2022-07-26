using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public Sound[] sounds;

    private void Awake()
    {
        foreach (Sound sound in sounds)
        {
            sound.src = gameObject.AddComponent<AudioSource>();
            sound.src.clip = sound.clip;
            sound.src.volume = sound.volume;
            sound.src.pitch = sound.pitch;
            sound.src.loop = sound.isLooping;
        }

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlaySound("Main BG");
    }

    public void PlaySound(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name.ToUpper() == name.ToUpper());

        if (!sound.src.isPlaying)
            sound.src.Play();
    }
}

[System.Serializable] 
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(.3f, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource src;

    public bool isLooping;
}
