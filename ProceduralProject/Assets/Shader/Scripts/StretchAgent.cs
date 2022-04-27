using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class StretchAgent : MonoBehaviour
{
    MusicData data;
    Rigidbody body;


    private void Start()
    {
        body = GetComponent<Rigidbody>();
        data = MusicData.data;
    }

    public void TakeData(float amp)
    {
        transform.localScale = Vector3.one * (transform.localScale.y + amp / 2);
    }

    private void Update()
    {
        Vector3 dif = data.transform.position - transform.position;
        Vector3 dir = dif.normalized;

        transform.localScale = AnimMath.Lerp(transform.localScale, Vector3.one, .01f);

        body.AddForce(dir * 200 * Time.deltaTime);
    }
}
