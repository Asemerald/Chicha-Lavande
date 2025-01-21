using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioContainer : MonoBehaviour
{
    public static AudioContainer Instance;
    private void Awake()
    {
        Instance = this;
    }
    
    public AudioClip jump, land, bumper, spawnBumper, death;
    [Space(15)]
    public AudioClip ambiance, music;

    public void PlaySound(AudioSource source, AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}
