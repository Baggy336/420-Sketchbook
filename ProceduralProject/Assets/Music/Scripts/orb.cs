using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]

public class orb : MonoBehaviour
{
    MusicVisual viz;
    Rigidbody body;
    void Start()
    {
        viz = MusicVisual.viz;
        GetComponent<MeshRenderer>().material.SetFloat("_TimeOffset", Random.Range(0, 2 * Mathf.PI));
        body = GetComponent<Rigidbody>();
    }

    public void AudioData(float amp)
    {
        transform.localScale = Vector3.one * (transform.localScale.x + amp);
    }

    void Update()
    {
        Vector3 dif = viz.transform.position - transform.position;
        Vector3 dirToViz = dif.normalized;

        transform.localScale = AnimMath.Lerp(transform.localScale, Vector3.zero, .01f);

        body.AddForce(dirToViz * 100 * Time.deltaTime);
    }
}
