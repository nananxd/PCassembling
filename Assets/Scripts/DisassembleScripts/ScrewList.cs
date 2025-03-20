using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScrewList : MonoBehaviour
{
    [SerializeField] private List<Transform> screwList = new List<Transform>();
    
    public List<Transform> ScrewsList { get => screwList; set => screwList = value; }
    void Start()
    {
        screwList = GetComponentsInChildren<Transform>(true).Skip(1).ToList() ;
    }

   
}
