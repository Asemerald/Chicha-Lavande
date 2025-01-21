using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public partial class PlayerController
    {

        // Client-side prediction variables
        private Vector3 lastProcessedPosition;
        private Quaternion lastProcessedRotation;


        

        [ServerRpc]
        private void UpdateServerWithPredictionServerRpc(Vector3 position, Quaternion rotation, ServerRpcParams rpcParams = default)
        {
            // Validate and apply predicted position and rotation for the client
            if (IsServer)
            {
                lastProcessedPosition = position;
                lastProcessedRotation = rotation;

                UpdateClientsWithPositionClientRpc(position, rotation);
            }
        }

        [ClientRpc]
        private void UpdateClientsWithPositionClientRpc(Vector3 position, Quaternion rotation, ClientRpcParams rpcParams = default)
        {
            if (!IsOwner)
            {
                // Smoothly interpolate to the updated position
                transform.position = Vector3.Lerp(transform.position, position, 0.1f);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.1f);
            }
        }
    }
}
