using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public AudioMixerGroup mixerGroup;

    [Range(0f, 1f)]
    public float volume = 1f;

    [Range(-3f, 3f)]
    public float pitch = 1f;

    public bool loop;

    [HideInInspector]
    public AudioSource source;

    public void SetUpSource(GameObject gameObject)
    {
        source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
        source.outputAudioMixerGroup = mixerGroup;
    }
}
