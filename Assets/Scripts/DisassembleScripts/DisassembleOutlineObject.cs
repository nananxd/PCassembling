using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DisassembleParts
{
    none,
    screws,
    cables,
    other
}


public class DisassembleOutlineObject : MonoBehaviour
{
    public DisassembleParts disassemblePart;
    [Tooltip("same with part name since it use for reference")]public string outlineName;
    public bool isDone;
    [SerializeField] private List<Outline> outlines;
   
    void Start()
    {
        
    }

    

    public void DisableOutline()
    {
        foreach (var item in outlines)
        {
            item.enabled = false;
        }

      
    }

    public void EnableOutline()
    {
        foreach (var item in outlines)
        {
            item.enabled = true;
        }
    }
}
