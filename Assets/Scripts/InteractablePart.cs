using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractablePart : MonoBehaviour,IPointerDownHandler,ISelectHandler,IDeselectHandler
{
    [SerializeField] private ComponentGroup group;
    [SerializeField]private StepController stepController;
    [SerializeField] private bool isConnectedToOtherComponent;
    [SerializeField] private Transform correctSlotTransform;
    [SerializeField] private ComponentSlot slot;
    [SerializeField] private Outline outline;
    [SerializeField] private BaseInteractivity interactivity;

    public ComponentSlot Slot { get => slot; set => slot = value; }

    private void Start()
    {
        //stepController = GetComponent<StepController>();
       // Setup();
    }

    public void Setup()
    {
        stepController = GetComponent<StepController>();
        interactivity = GetComponent<BaseInteractivity>();
        Slot = PCComponentManager.Instance.GetSlot(stepController.componentPartName);
        outline = GetComponent<Outline>();
        if (outline == null)
        {
            outline = GetComponentInChildren<Outline>();
        }
        outline.OutlineWidth = 5f;

        if (SceneLoaderManager.Instance.currentGameType == GameType.tutorial)
        {
            EnableHighlight(true);
        }
        else
        {
            EnableHighlight(false);
        }
    }

    public void DeactivateObject()
    {
        gameObject.SetActive(false);
    }

    public void EnableHighlight(bool isEnable)
    {
        outline.enabled = isEnable;
    }
    public void OnClickInteractablePart()
    {
        Debug.Log($"Click {transform.name}");
        PCComponentManager.Instance.CurrentSelectedInteractablePart = this;
       // QuestManager.Instance.CompleteStep(stepController.componentPartName, stepController.stepId);

        if (isConnectedToOtherComponent)
        {
            Debug.Log("connected to other component");
        }
        else
        {
            Debug.Log("not connected to other component");
        }
    }

    private void OnMouseDown()
    {
        //OnClickInteractablePart();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (group == GameManager.Instance.currentSelectedGroup)
        {
            //PCComponentManager.Instance.GetParts(stepController.componentPartName);
            PCComponentManager.Instance.GetParts(stepController.componentPartName,false);
            OnClickInteractablePart();
            Slot.ActivateIndicator(true);
            Slot.SetIndicatorPosition();
            PCComponentManager.Instance.SetComponentSlotIndicator(Slot);

            TutorialManager.Instance.CurrentSelectedInteractivity = interactivity;
            TutorialManager.Instance.CompleteTutorialSteps(true);
        }
        else
        {
            Debug.Log($"group:{group} is not the same as current selected group:{GameManager.Instance.currentSelectedGroup}");
           
        }
        
    }

    public void OnSelect(BaseEventData eventData)
    {
       
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log($"Deselect is trigger on interactablePart script");
        Slot.ActivateIndicator(false);
    }
}
