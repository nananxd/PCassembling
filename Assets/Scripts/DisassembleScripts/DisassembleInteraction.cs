using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Disassemble;
using DG.Tweening;



public class DisassembleInteraction : NameController,IPointerDownHandler
{
    [Tooltip("This is for the component part identification: use in quest")] 
    public ComponentGroup componentGroupType;
    public string groupParent;
    [Header("Animation Variables")]
    [SerializeField] private Ease easeType;
    [SerializeField] float duration;

   

    [Header("For power supply cable")]
    [Header("power supply settings")]
    [SerializeField] private bool isPowerSupplyCable;
    [Header("power supply settings")]
    [SerializeField] private bool isX, isY, isZ;
    [Header("power supply settings")]
    [SerializeField] private Transform targetTransform;
    [Header("power supply settings")]
    [SerializeField] private Transform targetLocation;

    [Header("Screws settings")]
    [SerializeField] private bool isScrew;
    [SerializeField] private DisassembleOutlineObject disAssembleOutlineObject;
    [SerializeField] private BaseInteractivity interactivity;

    [Header("Front Panel Settings")]
    public bool isFrontPanel;
    [SerializeField] private DisassembleFrontPanelAnimator frontPanelAnimator;


    public bool IsPowerSupplyCable { get => isPowerSupplyCable; set => isPowerSupplyCable = value; }
    public bool IsX { get => isX; set => isX = value; }
    public bool IsY { get => isY; set => isY = value; }
    public bool IsZ { get => isZ; set => isZ = value; }
    public bool IsScrew { get => isScrew; set => isScrew = value; }
    public DisassembleOutlineObject DisAssembleOutlineObject { get => disAssembleOutlineObject; set => disAssembleOutlineObject = value; }


    private void Awake()
    {
        disAssembleOutlineObject = GetComponent<DisassembleOutlineObject>();
        if (disAssembleOutlineObject == null)
        {
            disAssembleOutlineObject = GetComponentInChildren<DisassembleOutlineObject>();
        }
        //interactivity = GetComponent<BaseInteractivity>();
        disAssembleOutlineObject.outlineName = partsName;
    }
    void Start()
    {
        
    }

    private void OnEnable()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DisassembleGameManager.Instance.CurrentSelectedInteraction = this;
        var selectedParent = DisassembleGameManager.Instance.CheckParent(this);
        DisassembleGameManager.Instance.CurrentSelectedGroupParent = selectedParent;
        Disassemble.UIManager.Instance.SpawnUI(selectedParent);

        //selectedParent.DisableObject(partsName);
        //TutorialManager.Instance.CurrentSelectedInteractivity = interactivity;
        //TutorialManager.Instance.CompleteTutorialSteps();
    }

    public void DisableObject()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1f);
        sequence.AppendCallback(()=> {

            gameObject.SetActive(false);
        });
    }

    public void OnMove()
    {
        if (isPowerSupplyCable)
        {
            return;
        }
        var manager = DisassembleGameManager.Instance;
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(.5f);
        sequence.Append(transform.DOMove(manager.RightTransform.position, duration).SetEase(easeType));
        sequence.AppendInterval(.5f);
        sequence.Append(transform.DOMove(manager.UpTransform.position,duration).SetEase(easeType));
        if (partsName == "motherboard")
        {
            sequence.AppendInterval(.5f);
            sequence.Append(manager.pcCase.transform.DOMove(manager.pcCaseCorrectPos.position, duration).SetEase(easeType));
        }
        sequence.AppendInterval(1f);
        sequence.Append(transform.DOMove(manager.DownTransform.position, duration).SetEase(easeType));
        sequence.AppendInterval(1f);
        sequence.AppendCallback(() => {
            if (partsName == "motherboard")
            {
                manager.SetParentToOriginal();
            }
            gameObject.SetActive(false);
            DisassembleGameManager.Instance.DownTransform.gameObject.SetActive(true);
        });

    }

    public void MoveCable()
    {
        if (!isPowerSupplyCable)
        {
            return;
        }

        Sequence sequence = DOTween.Sequence();
        sequence.Append(targetTransform.DOMove(targetLocation.position,duration +3f).SetEase(easeType));
    }

    public void MoveScrews()
    {
        if (!IsScrew)
        {
            return;
        }
        var screwList = GetComponent<ScrewList>();
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < screwList.ScrewsList.Count; i++)
        {
            var screwTransform = screwList.ScrewsList[i];
            sequence.Append(screwTransform.DOMove(DisassembleGameManager.Instance.ScrewsUpTrans.transform.position, .7f).SetEase(easeType));
        }

       // sequence.Append(transform.DOMove(DisassembleGameManager.Instance.ScrewsUpTrans.transform.position,3f).SetEase(easeType));
        sequence.AppendInterval(1f);
        sequence.AppendCallback(() => {

            gameObject.SetActive(false);
            DisassembleGameManager.Instance.ScrewsUpTrans.gameObject.SetActive(true);
        });
    }

    public void MoveFrontPanel()
    {
        if (!isFrontPanel)
        {
            return;
        }

        frontPanelAnimator.AnimateFrontPanel();
    }
}
