using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public Text property;
    Dropdown drop;

    private void Start()
    {
        drop = GetComponent<Dropdown>();
    }

    private void OnDropdownValChanged()
    {
        
    }
}
