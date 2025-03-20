using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ChoicesBtnUI : MonoBehaviour
{
    [SerializeField] private int choiceId;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Ease easeType;
    [SerializeField] private float duration;
    [SerializeField] private Button button;
    [SerializeField] private BaseInteractivity interactivity;
    [SerializeField] private Color disableColor, enableColor;

    public int ChoiceID { get => choiceId; }
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickChoice);
    }

    public void EnableOrDisableState(bool isEnable)
    {
        if (isEnable)
        {
            button.image.color = enableColor;
        }
        else
        {
            button.image.color = disableColor;
        }
    }
    private void OnClickChoice()
    {
        UIManager.Instance.ChoicesController.currentSelecteedChoiceId = choiceId;

        TutorialManager.Instance.CurrentSelectedInteractivity = interactivity;//add check if tutorial mode
        TutorialManager.Instance.CompleteTutorialSteps();
    }
    
    public void ShowOrHideUI(bool isShown)
    {
        if (isShown)
        {
            canvasGroup.DOFade(1f,duration).SetEase(easeType);
        }
        else
        {
            canvasGroup.DOFade(0, duration).SetEase(easeType);
        }
        canvasGroup.blocksRaycasts = isShown;
        canvasGroup.interactable = isShown;
    }
}
