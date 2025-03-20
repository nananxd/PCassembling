using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComponentGroupData", menuName = "ScriptableObjects/ComponentGroup", order = 1)]

public class ComponentGroupSO : ScriptableObject
{
    public List<ComponentPartSteps> components;
}
