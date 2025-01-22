using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public partial class PlayerController : NetworkBehaviour
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
        private void DeathServerRpc()
        {
            // Notify all clients to disable this player's visual elements
            DeathClientRpc(NetworkObjectId);

            // Start the respawn coroutine on the server
            StartCoroutine(Respawn());
        }

        [ClientRpc]
        private void DeathClientRpc(ulong deadPlayerId)
        {
            if (NetworkObjectId == deadPlayerId)
            {
                // Disable this player's collider and mesh renderer
                if (playerCollider != null)
                    playerCollider.enabled = false;

                if (playerMeshRenderer != null)
                    playerMeshRenderer.enabled = false;
            }
        }

        private IEnumerator Respawn() // FOR SERVER ONLY
        {
            yield return new WaitForSeconds(3); // Wait for 3 seconds

            // Reset health and position on the server
            health = 100;

            // Notify all clients to re-enable this player's visual elements
            RespawnClientRpc(NetworkObjectId);
        }

        [ClientRpc]
        private void RespawnClientRpc(ulong respawningPlayerId)
        {
            if (NetworkObjectId == respawningPlayerId)
            {
                // Re-enable this player's collider and mesh renderer
                if (playerCollider != null)
                    playerCollider.enabled = true;

                if (playerMeshRenderer != null)
                    playerMeshRenderer.enabled = true;
            }
        }
    }
}
