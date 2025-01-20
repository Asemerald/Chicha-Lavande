using System;
using UnityEngine;

public class WaveBehaviour : MonoBehaviour
{
    public int audioPower;

    private float circleScaleValue = 0;

    private void Update()
    {
        if (audioPower == null)
            return;
        
        if (circleScaleValue < audioPower)
        {
            circleScaleValue += Time.deltaTime * audioPower;
        }
        
        else Destroy(gameObject);
    }
}
