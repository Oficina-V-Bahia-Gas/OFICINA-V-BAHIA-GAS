using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixer mainMixer;
    public Sound[] sounds;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.SetUpSource(gameObject);
        }
    }

    public void Play(string soundName)
    {
        if (string.IsNullOrWhiteSpace(soundName))
        {
            Debug.LogWarning("O nome do som está vazio ou nulo!");
            return;
        }

        Sound sound = Array.Find(sounds, s => s.name == soundName);
        if (sound == null)
        {
            Debug.LogWarning($"Som '{soundName}' não encontrado! Verifique o nome no AudioManager.");
            return;
        }
        sound.source.Play();
    }

    public void Stop(string soundName)
    {
        Sound sound = Array.Find(sounds, s => s.name == soundName);
        if (sound == null)
        {
            Debug.LogWarning($"Som '{soundName}' não encontrado!");
            return;
        }
        sound.source.Stop();
    }

    public void SetVolume(string mixerGroup, float volume)
    {
        mainMixer.SetFloat(mixerGroup, Mathf.Log10(volume) * 20);
    }

    public bool IsPlaying(string soundName)
    {
        Sound sound = Array.Find(sounds, s => s.name == soundName);
        if (sound == null)
        {
            Debug.LogWarning($"Som '{soundName}' não encontrado!");
            return false;
        }
        return sound.source.isPlaying;
    }

    public bool AnySoundPlaying()
    {
        foreach (Sound sound in sounds)
        {
            if (sound.source.isPlaying)
            {
                return true;
            }
        }
        return false;
    }
}