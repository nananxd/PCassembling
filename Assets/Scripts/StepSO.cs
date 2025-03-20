using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StepData", menuName = "ScriptableObjects/StepData", order = 1)]
public class StepSO : ScriptableObject
{  
    public string questID;
    public string questName;
    public string questStepToFinish;
    public string dependencyStepId;//reference to other step in order for this step to complete
    public bool isComplete;
}
