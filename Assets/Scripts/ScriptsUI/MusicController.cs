using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip[] audioClips;

    private int audioClipeNumber = 0;

    void Start()
    {
        
    }

    void Update()
    {
        if (!audioSource.isPlaying && audioClips.Length > 0)
        {
            audioSource.clip = audioClips[audioClipeNumber];
            audioSource.Play();

            audioClipeNumber++;

            if(audioClipeNumber >= audioClips.Length)
            {
                audioClipeNumber = 0;
            }
        }
    }
}
