using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SubtitleManager : MonoBehaviour
{
    [SerializeField] List<Subtitle> subtitles;
    [SerializeField] float initialDelay = 1f;
    [SerializeField] TextMeshProUGUI subtitleTxt;
    [SerializeField] AudioSource subtitleAudio;

    Coroutine subtitleCoroutine;
    Accessibility accessibility;

    private void Start()
    {
        accessibility = FindObjectOfType<Accessibility>();

        if (subtitleTxt != null)
        {
            subtitleTxt.text = "";
        }

        if (accessibility != null && accessibility.IsDubEnabled())
        {
            StartSubtitles();
        }
    }

    public void StartSubtitles()
    {
        if (subtitleCoroutine != null)
        {
            StopCoroutine(subtitleCoroutine);
        }

        subtitleCoroutine = StartCoroutine(PlayAllSubtitles());
    }

    public void StopSubtitles()
    {
        if (subtitleCoroutine != null)
        {
            StopCoroutine(subtitleCoroutine);
            subtitleCoroutine = null;
        }

        if (subtitleTxt != null)
        {
            subtitleTxt.text = "";
        }

        if (subtitleAudio != null)
        {
            subtitleAudio.Stop();
        }
    }

    IEnumerator PlayAllSubtitles()
    {
        yield return new WaitForSeconds(initialDelay);

        foreach (Subtitle subtitle in subtitles)
        {
            if (subtitleTxt != null)
            {
                subtitleTxt.text = subtitle.text;
            }

            if (accessibility != null && accessibility.IsDubEnabled() && subtitleAudio != null && subtitle.voice != null)
            {
                subtitleAudio.PlayOneShot(subtitle.voice);
            }

            yield return new WaitForSeconds(subtitle.duration);
        }

        if (subtitleTxt != null)
        {
            subtitleTxt.text = "";
        }
    }
}

[Serializable]
public class Subtitle
{
    public string text;
    public float duration;
    public AudioClip voice;
}