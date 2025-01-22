using System;
using System.Collections;
using UnityEngine;

public class SyncCooldown : MonoBehaviour
{
    public bool canWave;
    [SerializeField] private float waveDelay = 0.3f;

    private void Start()
    {
        canWave = true;
    }

    public void Cooldown()
    {
        StartCoroutine(WaveCD());
    }
    
    public IEnumerator WaveCD()
    {
        canWave = false;
        yield return new WaitForSeconds(waveDelay);
        canWave = true;
    }
}
