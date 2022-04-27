using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class UIHandler : MonoBehaviour
{
    public TMP_Dropdown drop;
    private int dropVal;
    public FractalMove move;
    public SetFractalData data;

    private void Start()
    {
        drop = GetComponent<TMP_Dropdown>();
    }

    private void Update()
    {
        dropVal = drop.value;

        switch (dropVal)
        {
            case 0:
                data.SetFractal(Properties.Zoom);
                break;
            case 1:
                data.SetFractal(Properties.ZoomX);
                break;
            case 2:
                data.SetFractal(Properties.ZoomY);
                break;
            case 3:
                data.SetFractal(Properties.Aspect);
                break;
            case 4:
                data.SetFractal(Properties.PanX);
                break;
            case 5:
                data.SetFractal(Properties.PanY);
                break;
            case 6:
                data.SetFractal(Properties.SeedX);
                break;
            case 7:
                data.SetFractal(Properties.SeedY);
                break;
        }
    }
}
