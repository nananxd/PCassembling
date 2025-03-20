using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ComponentParts", menuName = "ScriptableObjects/ComponentParts", order = 1)]

public class ComponentPartSteps : ScriptableObject
{
    public ComponentGroup group;
    public string componentName;
    public List<StepSO> stepsToComplete;
}

public enum ComponentGroup
{
    motherboard,
    external,
    cables

}
