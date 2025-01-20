using System;
using UnityEngine;

public class WaveBehaviour : MonoBehaviour
{
    [HideInInspector] public int audioPower;

    private float circleScaleValue = 0;

    private void Update()
    {
        if (audioPower == null)
            return;
        
        if (circleScaleValue < audioPower/2)
        {
            circleScaleValue += Time.deltaTime * 2;
            transform.localScale = new Vector3(circleScaleValue, circleScaleValue, circleScaleValue);
        }
        
        else Destroy(gameObject);
    }
}
