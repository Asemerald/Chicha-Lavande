using Unity.Netcode;
using UnityEngine;

public class PlayerOutline : NetworkBehaviour
{
    [SerializeField] private Material outlineMAT;

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        outlineMAT.SetVector("_PlayerPos", transform.parent.position);
    }
}
