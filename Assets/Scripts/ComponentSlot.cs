using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ComponentSlot : MonoBehaviour,IPointerDownHandler,ISelectHandler,IDeselectHandler
{
    [SerializeField] private string slotId;
    [SerializeField] private string componentNameAccepted;

    [Header("UI Indicator Settings")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform indicatorTransform;
    [SerializeField] private GameObject indicatorOBject;
    [SerializeField] private Collider slotCollider;
    [SerializeField] private BaseInteractivity interactivity;

    public string SlotId { get => slotId; }
    public string ComponentNameAccepted { get => componentNameAccepted; }
    public Collider SlotCollider { get => slotCollider; set => slotCollider = value; }

    void Start()
    {
        SetupIndicator();
    }

    

    public void ActivateIndicator(bool isActive)
    {
        indicatorOBject.SetActive(isActive);
    }
    public void SetupIndicator()
    {
        interactivity = GetComponent<BaseInteractivity>();

        SlotCollider = GetComponent<Collider>();
        slotCollider.enabled = true;
        indicatorOBject = Instantiate(indicatorTransform.gameObject, UIManager.Instance.IndicatorParent);
        indicatorOBject.SetActive(false);
        
        //indicatorOBject.GetComponent<RectTransform>().anchoredPosition = Camera.main.WorldToScreenPoint(transform.position + offset);
        //indicatorOBject.transform.position = Camera.main.WorldToScreenPoint(transform.position);       
       
    }

    public void SetIndicatorPosition()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPos.z > 0)
        {
            screenPos.z = 0;
            //indicatorOBject.GetComponent<RectTransform>().anchoredPosition = screenPos;
            indicatorOBject.transform.position = screenPos;
        }
    }

    
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"Slot id :{slotId}");
        if (PCComponentManager.Instance.CurrentSelectedPart.partsName == slotId)
        {
            //switch camera
            GameManager.Instance.SwitchCamera(slotId);
            PCComponentManager.Instance.GetParts(slotId);
            UIManager.Instance.ShowControlPanelUI(true);
            PCComponentManager.Instance.HideCurrentSlotIndicator(this);
            UIManager.Instance.HideUnnecessaryUI();
            Debug.Log($"slot id:{slotId},part name:{PCComponentManager.Instance.CurrentSelectedPart.partsName}");

            TutorialManager.Instance.CurrentSelectedInteractivity = interactivity;
            TutorialManager.Instance.CompleteTutorialSteps(true);
        }
        else
        {
            Debug.Log($"Current selected part is not equal to slot id {slotId}");
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        indicatorOBject.SetActive(false);
        Debug.Log("OnSelect ComponentSlot");
    }

    public void OnDeselect(BaseEventData eventData)
    {
        
    }
}
