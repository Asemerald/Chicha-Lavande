using System;
using Unity.Netcode;
using UnityEngine;
using Screen = UnityEngine.Device.Screen;

namespace Player
{
    public partial class PlayerController : NetworkBehaviour
    {
        private Transform hitTransform;

        private void HandleShooting()
        {
            Vector3 screenCenterPoint = new Vector3(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            hitTransform = null;

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                hitTransform = hit.transform;
            }
        }
 
        [ServerRpc (RequireOwnership = false)]
        private void ShootPlayerServerRpc(ulong targetNetworkObjectId, ulong ShooterID, ServerRpcParams rpcParams = default)
        {
            if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(targetNetworkObjectId, out var targetObject))
                return;

            var targetPlayer = targetObject.GetComponent<PlayerController>();
            if (targetPlayer != null)
            {
                // Notify all clients that a player was shot
                ShootPlayerClientRpc(targetNetworkObjectId, ShooterID);

                // Optionally handle game logic like reducing health on the server
            }
        }

        [ClientRpc]
        private void ShootPlayerClientRpc(ulong targetNetworkObjectId, ulong ShooterID)
        {
            if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(targetNetworkObjectId, out var targetObject))
            {
                
                if (ShooterID != NetworkManager.Singleton.LocalClientId)
                {
                    Debug.Log("You got shot!");
                    TakeDamage();
                }
                
                //Audio
                AudioManager.instance.PlayBulletShot(1, transform.position, networkObject.OwnerClientId);
                debugText.text = $"Player shoot! " + debugCounter++;
                return; 
                
            };

            var targetPlayer = targetObject.GetComponent<PlayerController>();
            if (targetPlayer != null)
            {
                if (targetPlayer.IsOwner)
                {
                    Debug.Log("You got shot!");
                    TakeDamage();
                }
                Debug.Log($"Player {targetPlayer.OwnerClientId} got shot!");
                
                //Audio
                AudioManager.instance.PlayBulletShot(1, transform.position, networkObject.OwnerClientId);
                debugText.text = $"Player shoot! " + debugCounter++;
            }
        }

        [ServerRpc (RequireOwnership = false)]
        private void ShootOtherServerRpc()
        {
            ShootOtherClientRpc();
        }
        
        [ClientRpc]
        private void ShootOtherClientRpc()
        {
            AudioManager.instance.PlayBulletShot(1, transform.position, networkObject.OwnerClientId);
            debugText.text = $"Player shoot! " + debugCounter++;
        }
        
    }
}
