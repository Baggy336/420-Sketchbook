using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFractalData : MonoBehaviour
{
    public FractalMove move;
    public float middle = 1;
    public float amp = 1;
    public float time = 0.25f;

    private void Start()
    {

    }

    private void Update()
    {
        
    }

    public void SetFractal(Properties i)
    {
        move.UpdateFractal(i, middle + Mathf.Sin(Time.time * time) * amp);
    }
}
