namespace Player
{
    public partial class PlayerController
    {
        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                // Disable input processing for non-owners but keep the script for synchronization.
                enabled = false;
            }
        }
    }
}