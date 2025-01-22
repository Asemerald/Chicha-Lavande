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
        private void ShootPlayerServerRpc(ulong targetNetworkObjectId, ServerRpcParams rpcParams = default)
        {
            if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(targetNetworkObjectId, out var targetObject))
                return;

            var targetPlayer = targetObject.GetComponent<PlayerController>();
            if (targetPlayer != null)
            {
                // Notify all clients that a player was shot
                ShootPlayerClientRpc(targetNetworkObjectId);

                // Optionally handle game logic like reducing health on the server
            }
        }

        [ClientRpc]
        private void ShootPlayerClientRpc(ulong targetNetworkObjectId)
        {
            if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(targetNetworkObjectId, out var targetObject))
                return;

            var targetPlayer = targetObject.GetComponent<PlayerController>();
            if (targetPlayer != null)
            {
                if (targetPlayer.IsOwner)
                {
                    Debug.Log("You got shot!");
                }
                Debug.Log($"Player {targetPlayer.OwnerClientId} got shot!");
            }
        }
        
    }
}
