using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ModelsScriptable", menuName = "ScriptableObjects/ModelList", order = 1)]

public class ModelSO : ScriptableObject
{
    public List<Model> models;
}


[System.Serializable]
public class Model
{
    public string modelName;
    public Sprite modelVisual;

    [TextArea(4,3)]
    public string description;
}
