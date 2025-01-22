using System.Collections;
using Player;
using TMPro;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(AudioLoudnessDetection))]
public class ObjectsWaveInstantier : NetworkBehaviour
{
    [SerializeField] private bool continius;
    [SerializeField] private bool isSync;
    [SerializeField] private SyncCooldown syncCooldown;
    
    [SerializeField] private AudioSource source;
    [SerializeField] private float lerpLoudnessSpeed = 300f;

    public GameObject parentWave;
    
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
        
        canWave = true;
        
        //canWave = true;
        currentLoudness = 0;
        loadingLoudTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSync)
        {
            canWave = syncCooldown.canWave;
        }
        
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

        if (canWave && loudness > 0)
        {
            WaveInstantier.instance.InstantiateWaveServerRpc(Mathf.RoundToInt(loudnessCurve.Evaluate(loudness) * 100),
                transform.position, var component = transform.parent.TryGetComponent<PlayerController>() != null ? NetworkManager.Singleton.LocalClientId : 99);

            if (isSync)
            {
                syncCooldown.Cooldown();
            }
            else
            {
                StartCoroutine(WaveCD());
            }
            
        }


        

        /*if (canWave)
        {
            if (((loudness < currentLoudness - 40 || loudness < threshold + 2) && loudness > 0) || loudness == 100)
            {
                Debug.Log("Instanciate");
                //canWave = false;
                WaveInstantier.instance.InstantiateWave(Mathf.RoundToInt(loudnessCurve.Evaluate(currentLoudness) * 100), transform.position, transform.gameObject);
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
        }*/

        if (uiText != null)
        {
            uiText.text = loudness.ToString();
        }
        
    }
    
    private IEnumerator WaveCD()
    {
        canWave = false;
        yield return new WaitForSeconds(0.3f);
        canWave = true;
    }
}