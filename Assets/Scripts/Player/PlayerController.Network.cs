using Unity.Netcode;

namespace Player
{
    public partial class PlayerController
    {
        public override void OnNetworkSpawn()
        {
            networkObject = GetComponent<NetworkObject>();
            
/*#if !UNITY_EDITOR
            debugText.gameObject.SetActive(false);
#endif*/
            
            if (IsOwner)
            {
                // Disable others UI 
                debugText.gameObject.SetActive(true);
                debugText.transform.parent.transform.parent.gameObject.SetActive(true); //TODO pitié c'est dégeulasse
                
            }
        }
    }
}