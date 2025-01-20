using System;
using TMPro;
using UnityEngine;

public class AudioLoudnessDetection : MonoBehaviour
{
    public int sampleWindow = 64;
    private AudioClip microphoneClip;

    public AudioSource source;
    public TextMeshProUGUI uiText;

    public float loudnessSensibility = 100;
    public float threshold = 0.1f;

    private void Start()
    {
        MicrophoneToAudioClip();
    }

    private float maxVolume = 0;

    private void Update()
    {
        
        float loudness = GetLoudnessFromMicrophone() * loudnessSensibility;
        string displayValue;

        if (loudness < threshold)
            loudness = 0;
        
        uiText.text = loudness.ToString();
    }

    public void MicrophoneToAudioClip()
    {
        //get the first microphone in device list
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }
    
    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;

        if (startPosition < 0)
            return 0;

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);
        
        //compute loudness
        float totalLoudness = 0;

        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness / sampleWindow;
    }
    
    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
    }
}
