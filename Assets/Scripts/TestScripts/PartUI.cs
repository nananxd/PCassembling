using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Disassemble;
using DG.Tweening;
public class PartUI : MonoBehaviour
{
    [Header("Dotween settings")]
    [SerializeField] private Ease easeType;
    [SerializeField] private float duration;
    [Space(2)]
    [SerializeField] private CanvasGroup partUICanvas;
    [SerializeField] private Transform parUITransform;

    [SerializeField] BaseInteractivity interactivity;
    [SerializeField] private TextMeshProUGUI partNameTxt;
    [SerializeField][Tooltip("It is also the quest name or quest id to use")] private string currentPartName;
    [SerializeField] private string componentGroupType;
    [SerializeField] private Button button;
    [SerializeField] private bool isPowerSupplyCable, isScrew,isFrontPanel;
    [SerializeField] private DisassembleOutlineObject referenceOutlineObject; // reference for the object outline script to disable or enable outline material


    public string CurrentPartName { get => currentPartName; }
    public string ComponentGroupType { get => componentGroupType; set => componentGroupType = value; }
    private void OnDestroy()
    {
        if (SceneLoaderManager.Instance.currentGameType == GameType.tutorial)
        {
            DisassembleTutorialManager.Instance.objectsToHide.Remove(interactivity);
        }

      
    }

    public void EnableOrDisableOutline(bool isEnable)
    {
        if (isEnable)
        {
            referenceOutlineObject.EnableOutline();
        }
        else
        {
            referenceOutlineObject.DisableOutline();
        }
    }
    public void SetUI(string partName,string groupType,bool powerSupply,bool screw,bool frontPanel)
    {
        button = GetComponent<Button>();
        interactivity = GetComponent<BaseInteractivity>();//for tutorial
        currentPartName = partName;
        componentGroupType = groupType;
        //partNameTxt.text = partName;
        partNameTxt.text = SetItemName(partName);
        isPowerSupplyCable = powerSupply;
        isFrontPanel = frontPanel;

        isScrew = screw;
        button.onClick.AddListener(OnClickPartUI);
        SetUIVisibility();

        interactivity.id = partName;
        if (SceneLoaderManager.Instance.currentGameType == GameType.tutorial)
        {
            DisassembleTutorialManager.Instance.objectsToHide.Add(interactivity);
            //TutorialManager.Instance.DisableOtheComponents();
        }
    }

    private void SetUIVisibility()
    {
        if (SceneLoaderManager.Instance.currentGameType == GameType.tutorial)
        {
            #region old code
            //if (DisassembleGameManager.Instance.currentDisassemblePart == referenceOutlineObject.disassemblePart)
            //{
            //    ShowOrHide(partUICanvas, true);
            //}
            //else
            //{
            //    ShowOrHide(partUICanvas, false);
            //}
            #endregion
           
            if (DisassembleTutorialManager.Instance.IsInteractivityInCurrentQuest(currentPartName))
            {
                ShowOrHide(partUICanvas, true);
            }
            else
            {
                ShowOrHide(partUICanvas, false);
            }
        }

    }

    public void SetOutlineObjectReference(DisassembleOutlineObject currentOutlineObject)
    {
        referenceOutlineObject = currentOutlineObject;
    }

    public void OnClickPartUI()
    {
        // put a checking 
        if (DisassembleGameManager.Instance.CheckIfPartRemove(currentPartName))
        {
            // setting transform for parts position
            Transform selectedTransform = DisassembleGameManager.Instance.GetDownTransform(currentPartName);
            Transform selectedRightTransform = DisassembleGameManager.Instance.GetRightTransform(currentPartName);

            DisassembleGameManager.Instance.SetRightTransform(selectedRightTransform);
            DisassembleGameManager.Instance.SetDownTransform(selectedTransform);

            DisassembleGameManager.Instance.SetScrewUpTransform(selectedTransform);

            DisassembleGameManager.Instance.CurrentSelectedGroupParent.DisableObject(currentPartName);
            DisassembleGameManager.Instance.CurrentSelectedGroupParent.RemoveParts(currentPartName);

            //DisassembleGameManager.Instance.DisableOutlineObject(referenceOutlineObject);
            if (isPowerSupplyCable || isFrontPanel)
            {
                CamManager.Instance.AnimateZoomInCam();
            }
            if (!isPowerSupplyCable && ! isScrew && !isFrontPanel)
            {
                CamManager.Instance.AnimateVirtualCam();
            }


            //TutorialManager.Instance.CurrentSelectedInteractivity = interactivity;
            //TutorialManager.Instance.CompleteTutorialSteps(true);

            DisassembleTutorialManager.Instance.CompleteTutorialSteps(currentPartName);
         



            DisassembleGameManager.Instance.AddToRemovePart(currentPartName);            

            // checking if game is finished 
            DisassembleGameManager.Instance.CheckIfGameFinished(componentGroupType, currentPartName);
           // DisassembleGameManager.Instance.SetOutlineObjects();
        }
        else
        {
            Disassemble.UIManager.Instance.WrongFeedback();
            StatisticsManager.Instance.CountPlaceMistake();
            DisassembleGameManager.Instance.AddComponentError(currentPartName,1);
            Debug.Log("Required part has not yet remove");
            

        }
       
    }

    private void ShowOrHide(CanvasGroup canvasGroup,bool isShown)
    {
        if (isShown)
        {
            canvasGroup.DOFade(1f,duration).SetEase(easeType);
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.DOFade(0, duration).SetEase(easeType);
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    private string SetItemName(string name)
    {
        string selectedName = "";
        switch (name)
        {
            case "hdd":
                selectedName = "Hard Disk Drive";
                break;
            case "hddSata":
                selectedName = "HDD SATA Cable";
                break;
            case "hddScrews":
                selectedName = "HDD Screws";
                break;
            case "motherboard":
                selectedName = "Motherboard ";
                break;
            case "powerSupplyScrews":
                selectedName = "Power Supply Screws";
                break;
            case "motherboardScrews":
                selectedName = "Motherboard Screws";
                break;
            case "opticScrews":
                selectedName = "Optical Drive Screws";
                break;
            case "frontPanel":
                selectedName = "Front Panel Connectors";
                break;
            case "opticdisk":
                selectedName = "Optical Disk Drive";
                break;
            case "opticSata":
                selectedName = "ODD SATA Cable";
                break;
            case "powersupply":
                selectedName = "Power Supply";
                break;
            case "4x4pin":
                selectedName = "4-pin 12V Power Connector";
                break;
            case "24pin":
                selectedName = "24-pin ATX Power Connector";
                break;
            case "powerHddCable":
                selectedName = "SATA Power Connector(HDD)";
                break;
            case "powerOpticCable":
                selectedName = "SATA Power Connector(ODD)";
                break;
            case "ram1":
                selectedName = "RAM 1";
                break;
            case "ram2":
                selectedName = "RAM 2";
                break;
            case "cpu":
                selectedName = "CPU";
                break;
            case "fan":
                selectedName = "CPU Fan";
                break;


        }

        return selectedName;
    }
}
