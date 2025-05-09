using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TestTutorialUI : MonoBehaviour
{
    public string inventName;
    public Button testButton;

    // Start is called before the first frame update

    private void Awake()
    {
        testButton = GetComponent<Button>();
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
