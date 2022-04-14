using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class TextDisplay : MonoBehaviour
{
    private TextMeshProUGUI tmpScore;

    void Start()
    {
        tmpScore = FindObjectOfType<TextMeshProUGUI>();
    }

    void Update()
    {
        tmpScore.SetText(ResourceBank.instance.resources.ToString());
    }
}
