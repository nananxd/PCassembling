using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoneUI : MonoBehaviour
{
    [SerializeField] private BaseInteractivity interactivity;


    public void OnDone()
    {
        TutorialManager.Instance.CurrentSelectedInteractivity = interactivity;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
