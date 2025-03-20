using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class ChoicesButton : MonoBehaviour
{
    public ComponentGroup group;
    public string mainVirtualCameraName;
    public int currentId;
    public bool isSelected;

    [SerializeField] private Color completedColor,normalColor,textNormalColor,textCompletedColor,textDisableColor;
    [SerializeField] private Sprite completedSprite,normalSprite;
    [SerializeField] private Image backgroundImage;

    [Header("this field get reference at runtime")]
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI txt;

    [SerializeField] private BaseInteractivity interactivity;

    private void Start()
    {
        button = GetComponent<Button>();
        txt = button.GetComponentInChildren<TextMeshProUGUI>();
        //CompleteState();
    }

    public void CompleteState()
    {
        //button.image.sprite = completedSprite;
        button.image.color = completedColor;
        backgroundImage.color = completedColor;
        button.transform.DOScale(new Vector3(1.05f,1.05f,1.05f),05f).SetEase(Ease.OutBack);
        txt.color = textNormalColor ;

    }

    public void NormalState()
    {
        button.image.sprite = normalSprite;
        button.image.color = normalColor;
        txt.color = textNormalColor;
    }

    public void DisableState()
    {
        txt.color = textDisableColor;
    }

    public void NormalStateText()
    {
        txt.color = textNormalColor;
    }

    public void OnSelected()
    {
        //isSelected = !isSelected;
        switch (group)
        {
            case ComponentGroup.motherboard:
                GameManager.Instance.currentSelectedGroup = ComponentGroup.motherboard;
                break;
            case ComponentGroup.external:
                GameManager.Instance.currentSelectedGroup = ComponentGroup.external;
                break;
            case ComponentGroup.cables:
                GameManager.Instance.currentSelectedGroup = ComponentGroup.cables;
                break;
         
        }

        TutorialManager.Instance.CurrentSelectedInteractivity = interactivity;//add check if tutorial mode
        TutorialManager.Instance.CompleteTutorialSteps();
    }
}
