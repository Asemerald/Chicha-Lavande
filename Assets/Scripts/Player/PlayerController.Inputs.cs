using System.Collections;
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
            
            if (context.action.WasPressedThisFrame())
            {
                //return if last shot was too recent
                if (Time.deltaTime - lastShotTime < timeBetweenShots && !canShoot) return;

                lastShotTime = Time.deltaTime;
                
                canShoot = false;
                StartCoroutine(ShootCooldown());
                
                Debug.Log("Fire");
            
                if (hitTransform != null)
                {
                    if (hitTransform.CompareTag("Player"))
                    {
                        // Call server to process the shot
                        ShootPlayerServerRpc(hitTransform.GetComponent<NetworkObject>().OwnerClientId);
                        Debug.Log("Shot at player");
                    }
                    else 
                    {
                        ShootOtherServerRpc();
                    }
                }
            }
        }

        private IEnumerator ShootCooldown()
        {
            yield return new WaitForSeconds(2);
            canShoot = true;
        }
    }
}