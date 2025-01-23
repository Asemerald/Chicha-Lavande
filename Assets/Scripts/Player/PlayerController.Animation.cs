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
            if (!IsOwner) return;
            PlayFootstepServerRpc();
        }
        
        private void Rien()
        {
            
        }

        [ServerRpc]
        private void PlayFootstepServerRpc()
        {
            PlayFootstepClientRpc();
        }

        [ClientRpc]
        private void PlayFootstepClientRpc()
        {
            AudioManager.instance.PlayFootstep(transform.position, networkObject.OwnerClientId);
        }

    }
}
