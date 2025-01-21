using System;
using UnityEngine;

public class WaveInstantier : MonoBehaviour
{
    public static WaveInstantier instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private GameObject wave;
    
    public GameObject InstantiateWave(int power, Vector3 position, GameObject parentGO)
    {
        GameObject waveInstance = Instantiate(wave, position, Quaternion.identity);
        waveInstance.transform.localScale = Vector3.zero;
        waveInstance.GetComponent<WaveBehaviour>().audioPower = power;
        waveInstance.GetComponent<WaveBehaviour>().parentGO = parentGO;
        return waveInstance;
    }
}
