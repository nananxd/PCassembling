using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class TutorialUIController : MonoBehaviour
{
    [Header("Inventory UI name")]
    [SerializeField] private string currentSelectedItemInventory;
    [SerializeField] private InventoryUI targetItem;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI displayTxt;
    [SerializeField] private Transform uiTransform;
    [SerializeField] private Button nextButton;

    [SerializeField] private bool isHaveButton;
    [SerializeField] private bool isWorldSpace;

    [Header("Arrow property")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrow1Position, arrow2Position,uiPosition;
    [SerializeField] private string display;

    [SerializeField] private BaseInteractivity interactivity;
    [SerializeField] private List<TutorialUI> tutorialsUI = new List<TutorialUI>();

    public Transform Arrow1Position { get => arrow1Position; set => arrow1Position = value; }
    public Transform Arrow2Position { get => arrow2Position; set => arrow2Position = value; }
    public Transform UiPosition { get => uiPosition; set => uiPosition = value; }
    public string Display { get => display; set => display = value; }
    public bool IsHaveButton { get => isHaveButton; set => isHaveButton = value; }
    public bool IsWorldSpace { get => isWorldSpace; set => isWorldSpace = value; }
    public List<TutorialUI> TutorialsUI { get => tutorialsUI; set => tutorialsUI = value; }

    private void Start()
    {
        tutorialsUI = GetComponentsInChildren<TutorialUI>(true).ToList();
        foreach (var item in tutorialsUI)
        {
            item.Setup();
        }
    }
    public void PositionUI()
    {
        displayTxt.text = display;
        if (isWorldSpace)
        {
            uiTransform.position = Camera.main.WorldToScreenPoint(uiPosition.position);
        }
        else
        {
            uiTransform.position = uiPosition.position;
        }
       

        nextButton.gameObject.SetActive(isHaveButton);
    }

    public void ActivateTutorialUI(string id)
    {
        var foundTutorial = tutorialsUI.Find(x => x.TutorialUid == id);
        var currentTutorial = TutorialManager.Instance.CurrentTutorial;
        //foundTutorial.SetTutorialText(currentTutorial.tutorialName);
        foundTutorial.gameObject.SetActive(true);
    }

    public void DeactivateTutorialUI(string id)
    {
        var foundTutorial = tutorialsUI.Find(x => x.TutorialUid == id);
        foundTutorial.gameObject.SetActive(false);
    }

    public void EventToEnableForScroll(string inventName)
    {
        currentSelectedItemInventory = inventName;
        targetItem = InventoryManager.Instance.GetInventoryItemByName(currentSelectedItemInventory);
        UIManager.Instance.inventoryScrollbar.onValueChanged.AddListener(IsVisible);
        

        //IsVisible(inventName);
    }

   
    public void IsVisible(Vector2 pos)
    {
        //var targetItem =  InventoryManager.Instance.GetInventoryItemByName(currentSelectedItemInventory);
         //targetItem = InventoryManager.Instance.GetInventoryItemByName(currentSelectedItemInventory);
        var inventoryViewport = UIManager.Instance.inventoryViewport;   
        

        if (RectTransformUtility.RectangleContainsScreenPoint(inventoryViewport, targetItem.transform.position, null) && !targetItem.isComplete)
        {
            TutorialManager.Instance.CurrentSelectedInteractivity = interactivity;
            TutorialManager.Instance.CompleteTutorialSteps();
            targetItem.isComplete = true;
        }
        else
        {

        }
    }

    public void GetInventName(string inventName)
    {
        currentSelectedItemInventory = inventName;
        targetItem = InventoryManager.Instance.GetInventoryItemByName(currentSelectedItemInventory);
    }

    public void ActivateIndicator(bool isActive)
    {
        targetItem.ActivateTutorial(isActive);
        UIManager.Instance.inventoryScrollbar.viewport.GetComponent<Mask>().enabled = !isActive;
    }
}
