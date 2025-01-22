using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private GameObject audioPrefab;
    
    [SerializeField] private AudioClip[] footsteps;
    
    [SerializeField] private AudioClip[] fireBullets;
    [SerializeField] private AudioClip[] emptyShots;
    [SerializeField] private AudioClip reload;
    
    [SerializeField] private AudioClip[] miscSounds;

    private void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume)
    {
        GameObject gameObject = audioPrefab;
        gameObject.transform.position = position;
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
        audioSource.Play();
        Object.Destroy((Object) gameObject, clip.length * ((double) Time.timeScale < 0.009999999776482582 ? 0.01f : Time.timeScale));
    }

    public void PlayFootstep(Vector3 position)
    {
        AudioClip randomClip = footsteps[Random.Range(0, footsteps.Length)];
        PlayClipAtPoint(randomClip, position, 1);
    }
    
    
    public void PlayBulletShot(int bulletRemainsIndex, Vector3 position)
    {
        AudioClip thisBullet;
        if (bulletRemainsIndex == 0)
        {
            thisBullet = emptyShots[Random.Range(0, 1)];
        }
        else
        {
            thisBullet = fireBullets[Random.Range(0, fireBullets.Length)];
        }
        
        PlayClipAtPoint(thisBullet, position, 1);
    }

    public void PlayMiscSound(int clipIndex, Vector3 position, float volume)
    {
        PlayClipAtPoint(miscSounds[clipIndex], position, volume);
    }
}
