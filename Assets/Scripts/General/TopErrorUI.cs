using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopErrorUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI componentTxt, valueTxt;

    public void Setup(string component,string count)
    {
        componentTxt.text = component;
        valueTxt.text = count;
    }
}
