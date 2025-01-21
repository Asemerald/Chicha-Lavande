using System;
using UnityEngine;

public class WaveBehaviour : MonoBehaviour
{
    [HideInInspector] public int audioPower;

    private float circleScaleValue = 0;

    [SerializeField] private ParticleSystem ps;

    private void Start()
    {
        var main = ps.main;
        main.startSize = audioPower * 0.1f;
        main.startLifetime = audioPower * 0.05f;
        
        //ps.sizeOverLifetime.size.curve[]
    }

    private void Update()
    {
        
        if (audioPower == null)
            return;
        
        if (circleScaleValue < audioPower/2)
        {
            circleScaleValue += Time.deltaTime * audioPower * 0.2f;
            transform.localScale = new Vector3(circleScaleValue, circleScaleValue, circleScaleValue);
        }
        
        else Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject)
        {
            Debug.Log($"Player ({other.gameObject.name}) find at {other.transform.position}");
        }
    }
}
