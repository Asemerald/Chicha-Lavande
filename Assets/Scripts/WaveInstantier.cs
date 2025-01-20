using UnityEngine;

public class WaveInstantier : MonoBehaviour
{
    [SerializeField] private GameObject wave;
    
    public void InstantiateWave(int power, Vector3 position)
    {
        GameObject waveInstance = Instantiate(wave, position, Quaternion.identity);
        waveInstance.transform.localScale = Vector3.zero;
        waveInstance.GetComponent<WaveBehaviour>().audioPower = power;
    }
}
