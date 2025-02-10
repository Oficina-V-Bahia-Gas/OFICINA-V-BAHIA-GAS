using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStart : MonoBehaviour
{
    [SerializeField] string musicName;

    void Start()
    {
        AudioManager.instance.Stop("Squeak");
        AudioManager.instance.Stop("Brush");
        AudioManager.instance.Stop("Electric Hum");
        AudioManager.instance.Stop("Menu");
        AudioManager.instance.Stop("Tutorial");
        AudioManager.instance.Stop("Fase 1");
        AudioManager.instance.Stop("Fase 2");
        AudioManager.instance.Play(musicName);
    }
}
