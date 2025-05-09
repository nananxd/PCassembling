using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DisassembleButtonTutorialUI
{
    public string tutorialId;

    [TextArea(15,3)]
    public string tutorialText;
}
public class DisassembleTutorialUIController : MonoBehaviour
{
    public List<DisassembleButtonTutorialUI> buttonTutorialsUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
