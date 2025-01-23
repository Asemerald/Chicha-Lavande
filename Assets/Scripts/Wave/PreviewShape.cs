using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Wave
{
    public class PreviewShape : NetworkBehaviour
    {
        public static PreviewShape instance;
        

        private void Start()
        {
            //if instantierID is the same as playerid, then destroy instantly
            //SpawnObjectServerRpc();
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(instance.gameObject);
                instance = this;
            }
            
            if (OwnerClientId == NetworkManager.Singleton.LocalClientId)
            {
                Destroy(gameObject);
                
            }
            
            GetComponent<ParticleSystem>().Play();
            StartCoroutine(DestroyAfterTime(3f));
        }
        
        [ServerRpc (RequireOwnership = false)]
        private void SpawnObjectServerRpc()
        {
            var networkObject = GetComponent<NetworkObject>();
            networkObject.Spawn();
        }
        
        [ServerRpc (RequireOwnership = false)]
        private void DestroyObjectServerRpc()
        {
            var networkObject = GetComponent<NetworkObject>();
            networkObject.Despawn(true);
        }

        public override void OnNetworkSpawn()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(instance.gameObject);
                instance = this;
            }
            
            if (OwnerClientId == NetworkManager.Singleton.LocalClientId)
            {
                Destroy(gameObject);
                
            }
            
            GetComponent<ParticleSystem>().Play();
            StartCoroutine(DestroyAfterTime(3f));
        }
        
        private IEnumerator DestroyAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
            //DestroyObjectServerRpc();
        }
    }
}