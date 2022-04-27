using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFractalData : MonoBehaviour
{
    private FractalMove move;
    public float middle = 1;
    public float amp = 1;
    public float time = 0.25f;

    private void Start()
    {
        move = GetComponent<FractalMove>();
    }

    private void Update()
    {
        move.UpdateFractal(middle + Mathf.Sin(Time.time * time) * amp);
    }
}
