using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCube : MonoBehaviour
{
    public Transform wallArt;
    private BoxCollider box;
    public bool isSolid;

    private void Start()
    {
        box = GetComponent<BoxCollider>();
        UpdateArt();
    }

    private void OnMouseDown()
    {
        isSolid = !isSolid;
        UpdateArt();
    }

    void UpdateArt()
    {
        if (wallArt)
        {
            wallArt.gameObject.SetActive(isSolid);

            float yPos = isSolid ? .44f : 0f;
            float h = isSolid ? 1.1f : .2f;

            box.size = new Vector3(1, h, 1);
            box.center = new Vector3(0, yPos, 0);
        }
    }
}
