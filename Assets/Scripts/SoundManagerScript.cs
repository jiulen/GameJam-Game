using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] audioClip;
    int audioIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip[audioIndex];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextSound();
        }
    }
    public void PlayNextSound()
    {
        audioIndex++;
        if (audioIndex == 8)
        {
            audioIndex = 0;
        }
        audioSource.clip = audioClip[audioIndex];
        audioSource.Play();
    }
}
