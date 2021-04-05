using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(){
        audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
}
