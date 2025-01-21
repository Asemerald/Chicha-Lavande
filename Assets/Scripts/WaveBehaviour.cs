using System;
using UnityEngine;

public class WaveBehaviour : MonoBehaviour
{
    [HideInInspector] public int audioPower;
    [HideInInspector] public GameObject parentGO;
    [SerializeField] private float waveDistMultiplier = 1.5f;
    [SerializeField] private float waveSpeedMultiplier = 2f;

    private float currentScale;

    [SerializeField] private ParticleSystem ps;

    private void Start()
    {
        var main = ps.main;
        main.startSize = audioPower * (0.1f * waveDistMultiplier);
        main.startLifetime = audioPower * ((0.05f * waveDistMultiplier) / waveSpeedMultiplier);

        currentScale = 0;
    }

    private void Update()
    { 
        if (audioPower == null)
            return;
        
        //scale collider with fx
        currentScale += Time.deltaTime * (2 * waveSpeedMultiplier);

        if (currentScale / 2 > audioPower * (0.05f * (waveDistMultiplier)) + 0.5f)
        {
            Destroy(gameObject);
        }

        transform.localScale = new Vector3(currentScale, currentScale, currentScale);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject != parentGO)
        {
            Debug.Log($"Player ({other.gameObject.name}) find at {other.transform.position}");
        }
    }
}
