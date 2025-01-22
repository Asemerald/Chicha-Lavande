using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
    [SerializeField] private AudioClip[] impactSounds;
    [SerializeField] private AudioClip reload;
    
    [SerializeField] private AudioClip[] miscSounds;

    private void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume = 1, ulong parentID = 99)
    {
        GameObject gameObject = Instantiate(audioPrefab);
        gameObject.transform.position = position;

        ObjectsWaveInstantier waveInstantier = gameObject.GetComponent<ObjectsWaveInstantier>();
        
        waveInstantier.parentWaveID = parentID;
        
        
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
        audioSource.Play();
        Object.Destroy((Object) gameObject, clip.length * ((double) Time.timeScale < 0.009999999776482582 ? 0.01f : Time.timeScale));
    }

    public void PlayFootstep(Vector3 position, ulong parentID)
    {
        AudioClip randomClip = footsteps[Random.Range(0, footsteps.Length - 1)];
        PlayClipAtPoint(randomClip, position, 1, parentID);
    }
    
    
    public void PlayBulletShot(int bulletRemainsIndex, Vector3 position, ulong parentID)
    {
        AudioClip thisBullet;
        if (bulletRemainsIndex == 0)
        {
            thisBullet = emptyShots[Random.Range(0, emptyShots.Length - 1)];
        }
        else
        {
            thisBullet = fireBullets[Random.Range(0, fireBullets.Length - 1)];
        }
        
        PlayClipAtPoint(thisBullet, position, 0.5f, parentID);
    }
    
    public void PlayImpactShot(Vector3 position, ulong parentID)
    {
        AudioClip impact = impactSounds[Random.Range(0, impactSounds.Length - 1)];
        
        PlayClipAtPoint(impact, position, 1, parentID);
    }

    public void PlayMiscSound(int clipIndex, Vector3 position, ulong parentID)
    {
        PlayClipAtPoint(miscSounds[clipIndex], position, 1, parentID);
    }
}
