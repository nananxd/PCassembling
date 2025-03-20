using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ModelsManager : MonoBehaviour
{
    [Header("Animation settings")]
    [SerializeField] private Ease easeType;
    [SerializeField] private float duration;

    [Header("Model variables")]
    [SerializeField] private Transform parentTransform;
    public static ModelsManager Instance;
    public ModelSO modelSO;
    [SerializeField] private List<ModelName> models;

    private Vector3 currentRot;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentRot = transform.eulerAngles;
    }

    public void RotateModel(bool isLeft)
    {
        if (isLeft)
        {
            Rotate(45f, -Vector3.up);
        }
        else
        {
            Rotate(45f, Vector3.up);
        }
    }

    public void GetModelByName(string modelName)
    {
        var selectedModelInfo = modelSO.models.Find(i => i.modelName == modelName);
        //var selectedModel = models.Find(x => x.nameOfModel == modelName);
       
        EnableModel(modelName);
        ManagerUI.Instance.SetInfoDetails(selectedModelInfo.description, selectedModelInfo.modelName);
    }

    private void EnableModel(string selectedModelName)
    {
        for (int i = 0; i < models.Count; i++)
        {
            if (models[i].nameOfModel == selectedModelName)
            {
                models[i].gameObject.SetActive(true);
            }
            else
            {
                models[i].gameObject.SetActive(false);
            }
        }
    }

    public void Rotate(float angle, Vector3 axis)
    {
        // Calculate the new target rotation using Quaternion multiplication
        Quaternion targetRotation = parentTransform.transform.rotation * Quaternion.AngleAxis(angle, axis);

        // Animate to the new rotation
        parentTransform.transform.DORotateQuaternion(targetRotation, duration).SetEase(easeType);
    }


}
