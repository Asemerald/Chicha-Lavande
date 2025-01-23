using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public partial class PlayerController
    {
        private Vector3 velocity;
        
        private void HandleAnimation()
        {
            velocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            playerAnimator.SetFloat("Speed", velocity.magnitude);
        }

        private void Footstep()
        {
            Debug.Log($"Footstep called on client {NetworkManager.Singleton.LocalClientId}");
            if (!IsOwner) return;
            PlayFootstepServerRpc();
        }

        [ServerRpc]
        private void PlayFootstepServerRpc()
        {
            Debug.Log($"PlayFootstepServerRpc called by client {NetworkManager.Singleton.LocalClientId}");
            PlayFootstepClientRpc();
        }

        [ClientRpc]
        private void PlayFootstepClientRpc()
        {
            Debug.Log($"PlayFootstepClientRpc received on client {NetworkManager.Singleton.LocalClientId}");

            AudioManager.instance.PlayFootstep(transform.position, networkObject.OwnerClientId);
        }

    }
}
