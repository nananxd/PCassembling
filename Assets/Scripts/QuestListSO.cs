using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestList", menuName = "ScriptableObjects/QuestList", order = 1)]
public class QuestListSO : ScriptableObject
{
    public AsessmentType assesmentType;
    public List<StepSO> quests;
}

public enum AsessmentType
{
    Assemble,
    Disassemble
}
