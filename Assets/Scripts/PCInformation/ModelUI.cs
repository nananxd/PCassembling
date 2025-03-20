using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ModelUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private string modelName;
    [SerializeField] private Image modelImage;
    
    void Start()
    {
        
    }

    public void Setup(string name,Sprite visual)
    {
        modelName = name;
        modelImage.sprite = visual;
        button.onClick.AddListener(OnClickModel);
    }

    public void OnClickModel()
    {
        ModelsManager.Instance.GetModelByName(modelName);
        ManagerUI.Instance.ShowModelControl(true);
        ManagerUI.Instance.InfoOpenBtn.transform.DOScale(Vector3.one,1f).SetEase(Ease.OutBack);
    }
}
