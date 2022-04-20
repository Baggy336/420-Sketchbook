using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(LineRenderer))]
public class MusicVisual : MonoBehaviour
{
    static public MusicVisual viz { get; private set; }

    private AudioSource player;
    private LineRenderer line;

    public float lineRadius = 500;
    public float lineHeight = 4;
    public float height = 10;

    public int numBands = 512;

    public orb prefab;

    private List<orb> objects = new List<orb>();

    private void Start()
    {
        if (viz != null)
        {
            Destroy(gameObject);
            return;
        }
        viz = this;

        player = GetComponent<AudioSource>();
        line = GetComponent<LineRenderer>();

        // Spawn objects and add them to the array
        for (int i = 0; i < numBands; i++)
        {
            Vector3 p = new Vector3(0, i * height / numBands, 0);
            objects.Add(Instantiate(prefab, p, Quaternion.identity, transform));
        }
    }

    private void OnDestroy()
    {
        if (viz == this) viz = null;   
    }

    private void Update()
    {
        UpdateWave();
        UpdateFrequencyBands();
    }

    private void UpdateFrequencyBands()
    {
        float[] bands = new float[numBands];
        player.GetSpectrumData(bands, 0, FFTWindow.BlackmanHarris);

        for (int i = 0; i < objects.Count; i++)
        {
            //float p = (float)i / numBands;
            //objects[i].localScale = Vector3.one * bands[i] * 200 * p;
            //objects[i].position = new Vector3(0, i * height / numBands, 0);

            objects[i].AudioData(bands[i] * 100);
        }
    }

    private void UpdateWave()
    {
        int samples = 1024;
        float[] data = new float[samples];
        player.GetOutputData(data, 0);

        Vector3[] points = new Vector3[samples];

        float avgAmp = 0;

        for (int i = 0; i < data.Length; i++)
        {
            float d = data[i];
            avgAmp += data[i];

            float rads = Mathf.PI * 2 * i / samples;

            float x = Mathf.Cos(rads) * lineRadius;
            float z = Mathf.Sin(rads) * lineRadius;
            float y = d * lineHeight;

            points[i] = new Vector3(x, y, z);
        }

        avgAmp /= samples;

        line.positionCount = points.Length;
        line.SetPositions(points);
    }
}
