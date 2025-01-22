using UnityEngine;

public class PlayerOutline : MonoBehaviour
{
    [SerializeField] private Material outlineMAT;

    // Update is called once per frame
    void Update()
    {
        outlineMAT.SetVector("_PlayerPos", transform.parent.position);
    }
}
