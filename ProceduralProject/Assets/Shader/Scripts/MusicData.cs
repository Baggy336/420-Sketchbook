using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicData : MonoBehaviour
{
    // Singleton reference to the data class
   public static MusicData data { get; private set; }

    private AudioSource musicPlayer;

    public StretchAgent agent;

    public int bands = 512;

    public float[] freqBands = new float[512];
    private float height = 10;

    private List<StretchAgent> agents = new List<StretchAgent>();

    private void Start()
    {
        // Set up the singleton
        if (data != null)
        {
            Destroy(gameObject);
            return;
        }
        data = this;

        musicPlayer = GetComponent<AudioSource>();

        for (int i = 0; i < bands; i++)
        {
            Vector3 p = new Vector3(0, i * height / bands, 0);
            agents.Add(Instantiate(agent, p, Quaternion.identity));
        }
    }
    private void Update()
    {
        BandData();
        WaveData();
    }

    private void OnDestroy()
    {
        if (data == this) data = null;
    }

    private void BandData()
    {

        musicPlayer.GetSpectrumData(freqBands, 0, FFTWindow.Hamming);

        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].TakeData(freqBands[i] * 100);
        }
    }

    private void WaveData()
    {
        int samples = 1024;
        float[] wave = new float[samples];

        musicPlayer.GetOutputData(wave, 0);

        float avgAmp = 0;

        for (int i = 0; i < wave.Length; i++)
        {
            avgAmp += wave[i];

        }

        avgAmp /= samples;
    }
}
