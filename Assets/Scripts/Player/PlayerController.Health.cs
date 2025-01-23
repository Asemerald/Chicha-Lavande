using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public partial class PlayerController
    {

        public void TakeDamage(int damage = 50) // TODO add damage types
        {
            health -= damage;
            if (health <= 0)
            {
                DeathServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void DeathServerRpc(ServerRpcParams rpcParams = default)
        {
            // Notify all clients, including passing the dead player's ID
            DeathClientRpc(rpcParams.Receive.SenderClientId);
        }


        [ClientRpc]
        private void DeathClientRpc(ulong senderClientId, ClientRpcParams rpcParams = default)
        {
            // Show the respawn UI only for the client who owns the object and set their NetworkVariable to true
            if (NetworkManager.Singleton.LocalClientId == senderClientId)
            {
                StartCoroutine(Respawn());

                // Ensure PlayerObject is not null
                var playerObject = NetworkManager.Singleton.LocalClient.PlayerObject;
                if (playerObject == null)
                {
                    Debug.LogError("PlayerObject is null for the local client.");
                    return;
                }

                // Get the PlayerController component
                var playerController = playerObject.GetComponent<PlayerController>();
                if (playerController == null)
                {
                    Debug.LogError("PlayerController component not found on the PlayerObject.");
                    return;
                }

                // Update the NetworkVariable rbKinematic
                playerController.rbKinematic.Value = true; // Update the NetworkVariable
                playerController.meshRendererEnabled.Value = false;
                playerController.colliderEnabled.Value = false;
            }
        }



        private IEnumerator Respawn() // 
        {
            deathText.transform.parent.gameObject.SetActive(true);
            deathText.text = "You died! Respawning in 3 seconds...";
            yield return new WaitForSeconds(1); // Wait for 1 seconds
            deathText.text = "You died! Respawning in 2 seconds...";
            yield return new WaitForSeconds(1); // Wait for 1 seconds
            deathText.text = "You died! Respawning in 1 seconds...";
            yield return new WaitForSeconds(1); // Wait for 1 seconds
            deathText.transform.parent.gameObject.SetActive(false);

            // Reset health and position on the server
            health = 100;
            
            // Reset the player's position to a random spawn point
            transform.position = GameManager.Instance.spawnPoints[Random.Range(0, GameManager.Instance.spawnPoints.Length)].position;

            // Notify all clients to re-enable this player's visual elements
            RespawnClientServerRpc();
        }
        
        [ServerRpc(RequireOwnership = false)]
        void RespawnClientServerRpc(ServerRpcParams rpcParams = default)
        {
            RespawnClientRpc(rpcParams.Receive.SenderClientId);
        }

        [ClientRpc]
        private void RespawnClientRpc(ulong respawningPlayerId)
        {
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(respawningPlayerId, out var networkClient))
            {
                if (networkClient.PlayerObject.TryGetComponent<PlayerController>(out var playerController))
                {
                    playerController.playerMeshRenderer.enabled = true;
                    playerController.playerCollider.enabled = true;
                    playerController.rb.isKinematic = false;
                }
            }
        }
    }
}
