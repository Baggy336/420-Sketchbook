using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(LineRenderer))]
public class MusicVisual : MonoBehaviour
{
    private AudioSource player;
    private LineRenderer line;

    public float radius = 500;
    public float amp = 4;
    public float height = 10;

    public int numBands = 512;

    public Transform prefab;

    private List<Transform> objects = new List<Transform>();

    private void Start()
    {
        player = GetComponent<AudioSource>();
        line = GetComponent<LineRenderer>();

        // Spawn objects and add them to the array
        for (int i = 0; i < numBands; i++)
        {
            Vector3 p = new Vector3(0, i * height / numBands, 0);
            objects.Add(Instantiate(prefab, p, Quaternion.identity, transform));
        }
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
            float p = (float)i / numBands;
            objects[i].localScale = Vector3.one * bands[i] * 200 * p;
            objects[i].position = new Vector3(0, i * height / numBands, 0);
        }
    }

    private void UpdateWave()
    {
        int samples = 1024;
        float[] data = new float[samples];
        player.GetOutputData(data, 0);

        Vector3[] points = new Vector3[samples];

        for (int i = 0; i < data.Length; i++)
        {
            float d = data[i];

            float rads = Mathf.PI * 2 * i / samples;

            float x = Mathf.Cos(rads) * radius;
            float z = Mathf.Sin(rads) * radius;
            float y = d * amp;

            points[i] = new Vector3(x, y, z);
        }
        line.positionCount = points.Length;
        line.SetPositions(points);
    }
}
