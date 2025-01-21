using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioLoudnessDetection))]
public class ObjectsWaveInstantier : MonoBehaviour
{
    [SerializeField] private bool continius;
    [SerializeField] private AudioSource source;
    [SerializeField] private float lerpLoudnessSpeed = 300f;
    
    private AudioLoudnessDetection detector;
    private float loudness;
    //private bool canWave;
    private bool canWave;

    private float loudnessValue;
    
    private float currentLoudness;
    private float loadingLoudTimer;
    
    public float loudnessSensibility = 100;
    public float threshold = 0.1f;
    
    public TextMeshProUGUI uiText;

    public AnimationCurve loudnessCurve;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        detector = GetComponent<AudioLoudnessDetection>();
        
        if (continius)
            detector.MicrophoneToAudioClip();
        
        //canWave = true;
        canWave = true;
        currentLoudness = 0;
        loadingLoudTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (continius)
        {
            loudnessValue = detector.GetLoudnessFromMicrophone() *  loudnessSensibility;
        }
        else
        {
            loudnessValue = detector.GetLoudnessFromAudioClip(source.timeSamples, source.clip)  * loudnessSensibility;
        }

        loudness = Mathf.Round(Mathf.Clamp(Mathf.Lerp(loudness, loudnessValue, lerpLoudnessSpeed * Time.deltaTime), 0, 100));

        if (loudness < threshold)
            loudness = 0;
        

        if (canWave)
        {
            if ((loudness < currentLoudness - 40 || loudness < 2) && loudness > 0)
            {
                Debug.Log("Instanciate");
                //canWave = false;
                WaveInstantier.instance.InstantiateWave(Mathf.RoundToInt(loudnessCurve.Evaluate(currentLoudness) * 100), transform.position, transform.gameObject);
                canWave = false;
                currentLoudness = 0;
                loadingLoudTimer = 0;
                
                //Debug.Log($"Instancier après {loadingLoudTimer} secondes");
                StartCoroutine(WaveCD());
            }
            else
            {
                currentLoudness = loudness;
                loadingLoudTimer += Time.deltaTime;
            }
        }
        
        uiText.text = loudness.ToString();
    }
    
    private IEnumerator WaveCD()
    {
        yield return new WaitForSeconds(0.3f);
        canWave = true;
    }
}