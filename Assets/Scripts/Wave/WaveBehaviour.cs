using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using Wave;

public class WaveBehaviour : NetworkBehaviour
{
    [HideInInspector] public int audioPower;
    [HideInInspector] public ulong parentId;
    [SerializeField] private float waveDistMultiplier = 1.5f;
    [SerializeField] private float waveSpeedMultiplier = 2f;

    private float currentScale;

    [SerializeField] private ParticleSystem ps;
    [SerializeField] private PreviewShape previewShape;

    private void Start()
    {
        var main = ps.main;
        main.startSize = audioPower * (0.1f * waveDistMultiplier);
        main.startLifetime = audioPower * ((0.05f * waveDistMultiplier) / waveSpeedMultiplier);

        currentScale = 0;
    }

    public override void OnNetworkSpawn()
    {

    }

    [ServerRpc (RequireOwnership = false)]
    private void SpawnObjectServerRpc()
    {
        var networkObject = GetComponent<NetworkObject>();
        networkObject.Spawn();
    }

    private void Update()
    { 
        if (audioPower == null)
            return;
        
        //scale collider with fx
        currentScale += Time.deltaTime * (2 * waveSpeedMultiplier);

        if (currentScale / 2 > audioPower * (0.05f * (waveDistMultiplier)) + 0.5f)
        {
            Destroy(gameObject);
        }

        transform.localScale = new Vector3(currentScale, currentScale, currentScale);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.GetComponent<NetworkObject>().OwnerClientId != NetworkManager.Singleton.LocalClientId)
        {
            PreviewShape instanceShape = Instantiate(previewShape, other.transform.position, other.transform.rotation);
            var networkObject = GetComponent<NetworkObject>();
            Destroy(gameObject, 3f);
            //StartCoroutine(Despawn());
        }
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(3);
        DespawnServerRpc();
    }
    
    [ServerRpc (RequireOwnership = false)]
    private void DespawnServerRpc()
    {
        var networkObject = GetComponent<NetworkObject>();
        networkObject.Despawn(true);
    }
}
