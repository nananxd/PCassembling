using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    [SerializeField] private Image staticImage,draggableImage;
    [SerializeField] private TextMeshProUGUI itemNameTxt;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Vector2 startPos;
    [SerializeField] private RectTransform parentTransform;
    [SerializeField] private RectTransform originalParent;
    [SerializeField] private BaseInteractivity interactivity;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button currentButton;

    [Header("Tutorial UI")]
    [SerializeField] private TextMeshProUGUI tutorialItemTxt;
    [SerializeField] private GameObject tutorialParent;
    public bool isComplete;

    private string itemName;
    private Vector2 offset;

    public BaseInteractivity Interactivity { get => interactivity; set => interactivity = value; }
    public string ItemName { get => itemName; set => itemName = value; }

    private void Start()
    {
        //startPos = transform.position;
        //startPos = Vector2.zero;
        startPos = rectTransform.anchoredPosition;
        currentButton = GetComponent<Button>();
        originalParent = transform.GetComponentInParent<RectTransform>();
        Interactivity = GetComponent<BaseInteractivity>();

        Interactivity.id = ItemName;
        //TutorialManager.Instance.objectsToHide.Add(interactivity);
    }

    public void EnableOrDisableState(bool isEnable)
    {
        if (isEnable)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }
        else
        {
            canvasGroup.alpha = .1f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }

    public void Setup(Sprite itemSprite, string itemName, GameObject item = null)
    {
        if (itemSprite != null)
        {
            staticImage.sprite = itemSprite;
            draggableImage.sprite = itemSprite;
        }

        itemPrefab = item;

        this.itemNameTxt.text = SetItemName(itemName);
        //this.itemNameTxt.text = itemName;
        this.ItemName = itemName;

        tutorialItemTxt.text = itemName;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentTransform = transform.parent.parent.parent.GetComponentInParent<RectTransform>();
        rectTransform.transform.parent = parentTransform;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform,
          eventData.position, eventData.pressEventCamera, out offset);
        offset = rectTransform.anchoredPosition - offset;

        TutorialManager.Instance.CurrentSelectedInteractivity = Interactivity;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //rectTransform.anchoredPosition += eventData.delta;

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform,
            eventData.position, eventData.pressEventCamera, out localPoint))
        {
            rectTransform.anchoredPosition = localPoint + offset;
        }

        PCComponentManager.Instance.HighlightObject(ItemName);

        //if (itemName == "motherboardScrews")
        //{
           
        //}
       
        

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.transform.parent = originalParent;
        PCComponentManager.Instance.HighlightObject(ItemName, false);
        //if (itemName == "motherboardScrews")
        //{
            
        //}
            

        var hit = GameManager.Instance.CastRay();
        if (hit.collider != null && hit.collider.CompareTag("partsLocation"))
        {
            var locationTrigger = hit.collider.gameObject.GetComponent<LocationTrigger>();
            LocationTriggerManager.Instance.CurrentLocationTrigger = locationTrigger;
            if (locationTrigger.IsNameOnTriggerList(ItemName))
            {
                if (PCComponentManager.Instance.placementDependencies.TryGetValue(ItemName, out string requiredComponent))
                {
                    if (PCComponentManager.Instance.placedComponents.Contains(requiredComponent))
                    {
                        PlaceComponent(ItemName);
                        TutorialManager.Instance.CompleteTutorialSteps();
                    }
                    else
                    {
                        Debug.Log($"Item Inventory UI :Need to place first the require parts");
                        UIManager.Instance.WrongFeedback();
                        GameManager.Instance.AddComponentError(itemName,1);
                        rectTransform.anchoredPosition = startPos;
                    }
                }
                else
                {
                    PlaceComponent(ItemName);
                    TutorialManager.Instance.CompleteTutorialSteps();
                }
            }
            else
            {
                UIManager.Instance.WrongFeedback();
                GameManager.Instance.AddComponentError(itemName, 1);
                rectTransform.anchoredPosition = startPos;
                Debug.Log("Item Inventory UI: the item name is  not in trigger list");
            }

            //Instantiate(itemPrefab, hit.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Item Inventory UI: No collider found");
            rectTransform.anchoredPosition = startPos;
        }
       
       
    }

    private void PlaceComponent(string partName)
    {
        PCComponentManager.Instance.placedComponents.Add(partName);
        GameManager.Instance.SwitchCamera(partName);        
        PCComponentManager.Instance.GetParts(partName);
        
        //PCComponentManager.Instance.CurrentSelectedPart
        if (PCComponentManager.Instance.CurrentSelectedPart.isDontHaveControlUI)
        {
            UIManager.Instance.ShowControlPanelUI(false);
            UIManager.Instance.OnClickDone();
        }
        else
        {
            UIManager.Instance.ShowControlPanelUI(true);
        }

        currentButton.interactable = false;
        draggableImage.raycastTarget = false;

        UIManager.Instance.HideUnnecessaryUI();
        rectTransform.anchoredPosition = startPos;
    }



    //tutorial

    public void ActivateTutorial(bool isActive)
    {
        tutorialParent.SetActive(isActive);
    }

    private string SetItemName(string name)
    {
        string selectedName = "";
        switch (name)
        {
            case "thermalPaste":
                selectedName = "Thermal Paste";
                break;
            case "hddSata":
                selectedName = "HDD SATA Data Connector";
                break;
            case "opticSata":
                selectedName = "Optical Drive SATA Data Connector";
                break;
            case "motherboardScrews":
                selectedName = "Motherboard Screws";
                break;
            case "powerSupplyScrews":
                selectedName = "Power Supply Screws";
                break;
            case "hddScrews":
                selectedName = "HDD Screws";
                break;
            case "opticScrews":
                selectedName = "Optical Drive Screws";
                break;
            case "frontPanel":
                selectedName = "Front Panel Connectors";
                break;
           

        }

        return selectedName;
    }

    

    
}

////
///player drag the item from inventory to 3d scene to place
///the camera zoom to pc part to rotate
///on press done the part goes to slot activating the correct parts of 3d model
///if there is an interactable part of this model the player can click on it
///on player click on the interactable part other parts of the scene highlight where player can put the interactable to slot
///when player click the slot the camera will zoom to slot and player control the interactable
///on click done the model will go to slot and another 3d model version of the component will activate
