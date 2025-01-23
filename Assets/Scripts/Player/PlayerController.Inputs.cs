using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public partial class PlayerController
    {
        public void OnMove(InputAction.CallbackContext context)
        {
            if (isDead) return;
            
            moveVector = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (isDead) return;
            
            if (context.action.WasPressedThisFrame())
            {
                Debug.Log("Jump");
                Jump();
            }
        }
        
        public void OnFire(InputAction.CallbackContext context)
        {
            if (isDead) return;
            
            if (context.action.WasPerformedThisFrame())
            {
                //return if last shot was too recent
                if (Time.time - lastShotTime < timeBetweenShots) return;
                
                lastShotTime = Time.time;
                
                Debug.Log("Fire");
            
                if (hitTransform != null)
                {
                    if (hitTransform.TryGetComponent<NetworkObject>(out var networkObject))
                    {
                        // Call server to process the shot
                        ShootPlayerServerRpc(networkObject.NetworkObjectId);
                        Debug.Log("Shot at player");
                    }
                    else 
                    {
                        ShootOtherServerRpc();
                    }
                }
            }
        }
    }
}