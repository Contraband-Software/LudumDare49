using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLine_Manager : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Voice Lines")]
    public List<AudioClip> voiceLines = new List<AudioClip>();

    [Header("Active Voice Line")]
    public AudioClip activeLine;

    void Start()
    {
    }

    public void PlayVoiceLine(string taskName)
    {
        foreach(AudioClip clip in voiceLines)
        {
            if(clip.name == taskName)
            {
                activeLine = clip;
                break;
            }
        }

        print("playing osund");

        audioSource.clip = activeLine;
        audioSource.Play();
    }
}
