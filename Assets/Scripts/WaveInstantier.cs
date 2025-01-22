using System;
using Unity.Netcode;
using UnityEngine;

public class WaveInstantier : NetworkBehaviour
{
    public static WaveInstantier instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private WaveBehaviour wave;
    
    private void InstantiateWave(int power, Vector3 position, ulong parentId)
    {
        WaveBehaviour waveInstance = Instantiate(wave, position, Quaternion.identity);
        waveInstance.transform.localScale = Vector3.zero;
        waveInstance.audioPower = power;
        waveInstance.parentId = parentId;
    }
    
    [ServerRpc (RequireOwnership = false)]
    public void InstantiateWaveServerRpc(int power, Vector3 position, ulong parentId)
    {
        InstantiateWaveClientRpc(power, position, parentId);
    }
    
    [ClientRpc]
    private void InstantiateWaveClientRpc(int power, Vector3 position, ulong parentId)
    {
        InstantiateWave(power, position, parentId);
    }
}
