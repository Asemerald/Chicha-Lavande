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
            DeathClientRpc(NetworkManager.Singleton.LocalClientId, rpcParams.Receive.SenderClientId);
        }


        [ClientRpc]
        private void DeathClientRpc(ulong deadPlayerId, ulong senderClientId, ClientRpcParams rpcParams = default)
        {
            // Disable visuals and physics for all clients
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(deadPlayerId, out var networkClient))
            {
                if (networkClient.PlayerObject.TryGetComponent<PlayerController>(out var playerController))
                {
                    playerController.playerMeshRenderer.enabled = false;
                    playerController.playerCollider.enabled = false;
                    playerController.rb.isKinematic = true;
                }
            }

            // Show the respawn UI only for the client who owns the object
            if (NetworkManager.Singleton.LocalClientId == senderClientId)
            {
                StartCoroutine(Respawn());
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
        void RespawnClientServerRpc()
        {
            RespawnClientRpc(networkObject.OwnerClientId);
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
