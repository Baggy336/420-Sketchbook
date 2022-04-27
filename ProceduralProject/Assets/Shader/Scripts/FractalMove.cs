using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Properties
{
    Zoom,
    ZoomX,
    ZoomY,
    PanX,
    PanY,
    Aspect,
    SeedX,
    SeedY
}

public class FractalMove : MonoBehaviour
{
    public static FractalMove move;
    public Material fractalMat;
    public Properties property;

    public void UpdateFractal(float a)
    {
        var current = fractalMat.GetVector("_Zoom");
        var currentP = fractalMat.GetVector("_Pan");
        var currentS = fractalMat.GetVector("_Seed");
        switch (property)
        {
            case Properties.Zoom:
                fractalMat.SetVector("_Zoom", new Vector4(a, a, a, a));
                break;
            case Properties.ZoomX:
                fractalMat.SetVector("_Zoom", new Vector4(a, current.y, current.z, current.w));
                break;
            case Properties.ZoomY:
                fractalMat.SetVector("_Zoom", new Vector4(current.x, a, current.z, current.w));
                break;
            case Properties.Aspect:
                fractalMat.SetFloat("_Aspect", a);
                break;
            case Properties.PanX:
                fractalMat.SetVector("_Pan", new Vector4(a, currentP.y, currentP.z, currentP.w));
                break;
            case Properties.PanY:
                fractalMat.SetVector("_Pan", new Vector4(currentP.x, a, currentP.z, currentP.w));
                break;
            case Properties.SeedX:
                fractalMat.SetVector("_Seed", new Vector4(a, currentS.y, currentS.z, currentS.w));
                break;
            case Properties.SeedY:
                fractalMat.SetVector("_Seed", new Vector4(currentS.x, a, currentS.z, currentS.w));
                break;
        }
    }
}
