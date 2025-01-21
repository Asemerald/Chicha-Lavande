using UnityEngine;

public class PlayerOutline : MonoBehaviour
{
    [SerializeField] private MeshRenderer outlineMesh;

    // Update is called once per frame
    void Update()
    {
        outlineMesh.material.SetVector("PlayerPos", transform.position);
    }
}
