using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public partial class PlayerController
    {
        public void OnMove(InputAction.CallbackContext context)
        {
            moveVector = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.action.WasPressedThisFrame())
            {
                Jump();
            }
        }
    }
}