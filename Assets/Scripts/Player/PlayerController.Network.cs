using Unity.Netcode;

namespace Player
{
    public partial class PlayerController
    {
        public override void OnNetworkSpawn()
        {
            networkObject = GetComponent<NetworkObject>();
            
            if (!IsOwner)
            {
                // Disable input processing for non-owners but keep the script for synchronization.
                enabled = false;
            }
        }
    }
}