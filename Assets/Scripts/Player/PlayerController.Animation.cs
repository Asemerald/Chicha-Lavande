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
            AudioManager.instance.PlayFootstep(feet.position, gameObject);
        }
    }
}
