using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float value)
    {
        float dB = value > 0.01f ? Mathf.Lerp(-30, 0, value) : -80f;
        audioMixer.SetFloat("MasterVolume", dB);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        float dB = value > 0.01f ? Mathf.Lerp(-30, 0, value) : -80f;
        audioMixer.SetFloat("MusicVolume", dB);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        float dB = value > 0.01f ? Mathf.Lerp(-30, 0, value) : -80f;
        audioMixer.SetFloat("SFXVolume", dB);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
}
