using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private ChoicesButtonController choicesController;
    [SerializeField] private GameObject controlUI;
    [SerializeField] private Image wrongFeedbackUI;
    [SerializeField] private RectTransform choicesPanelUI, hidePanelUI;
    [SerializeField] private Button showButton, hideButton;
    [SerializeField] private Vector3 choicePanelHidePosition, choicePanelShowPosition;
    [SerializeField] private CanvasGroup choiceCanvasGroup;
    [SerializeField] private float duration;
    [SerializeField] private Ease easeType;
    [SerializeField] private GameObject cinematicUI;

    [Header("General UI")]
    [SerializeField] private GameObject levelCompleteUI;
    [SerializeField] private Button rotateLeftBtn, rotateRightBtn;

    [Header("Choices UI Area")]
    [SerializeField] private Image contentImage;
    [SerializeField] private List<ChoicesButton> choicesBtns;
    [SerializeField] private CanvasGroup contentCanvasGroup;
    [SerializeField] private ChoicesButton prevChoiceBtn, currentSelectedBtn;
    [SerializeField] private Button choicesButton;
    [SerializeField] private bool isChoicesOpen;
    [SerializeField] private Vector3 buttonsPanelShowPos, buttonsPanelHidePos;
    

    [Header("Loading UI ")]
    [SerializeField] private GameObject loadingScreenUI;
    [SerializeField] private CanvasGroup loadingCanvasGroup;

    [Header("Menu UI")]
    [SerializeField] private CanvasGroup menuCanvasGroup;
    [SerializeField] private RectTransform menuUI;
    [SerializeField] private Vector3 hidePosition, showPosition;
    [SerializeField] private Button menuRestartBtn, menuMainMenuBtn;
    [SerializeField] private List<CanvasGroup> menuBtnCanvasGroup;

    [Header("Inventory UI")]
    [SerializeField] private GameObject inventoryPrefab;
    [SerializeField] private Transform parent;
    [SerializeField] private CanvasGroup inventoryCanvasGroup,showInventoryCanvasGroup;
    [SerializeField] private RectTransform inventoryRect, showInventoryRec;
    [SerializeField] private Vector3 inventoryHidePos, inventoryShowPos;
    [SerializeField] private Vector3 showInventoryHidePos, showInventoryShowPos;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Image showImage, hideImage;
    [SerializeField] private bool isInventoryOpen;

    [Header("Install Motherboard UI")]
    public UnityEvent OnInstallMotherBoardEvent;
    [SerializeField] private GameObject InstallMotherboard;
    [SerializeField] private Button motherBoardBtn;
    [SerializeField] private BaseInteractivity interactivity;//motherboard ui interactivity

    [Header("Debug UI")]
    public TextMeshProUGUI mousePositionText;
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Transform indicatorParent;
    [Header("Quiz or Trivia UI")]
    [SerializeField] private Vector3 scaleFactor;
    [SerializeField] private Transform triviaTransform;
    [SerializeField] private CanvasGroup triviaCanvasGroup;
    [SerializeField] private QuizButton choiceA, choiceB, choiceC, choiceD;
    [SerializeField] private TextMeshProUGUI quizNameTxt;


    [Header("Statistics")]
    [SerializeField] private TextMeshProUGUI timeText, correctAnswerText, mistakeText;
    [SerializeField] private RectTransform statRect,timeRect,correctAnsRect,mistakeRect;
    [SerializeField] private CanvasGroup statisticCanvasGroup;
    [SerializeField] private Vector3 statHidePos, statShowPos;
    [SerializeField] private bool isStatShow;
    [SerializeField] private Button statCloseButton;

    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private CanvasGroup timerCanvasGroup;
    [SerializeField] private RectTransform timerRect,timerIconRect;
    [SerializeField] private Vector3 timerShowPos, timerHidePos;
    [SerializeField] private bool isTimerShown;

    [Header("Notification UI")]
    [SerializeField] private CanvasGroup notifCanvasGroup;
    [SerializeField] private RectTransform notificationRect;
    [SerializeField] private TextMeshProUGUI notificationTxt;
    [SerializeField] private Vector3 notifHidePos, notifShowPos;

    [Header("End Screen for practice and tutorial")]
    [SerializeField] private TextMeshProUGUI endScreenTxt;
    [SerializeField] private CanvasGroup endScreenCanvas;
    [SerializeField] private RectTransform endScreenRect;
    [SerializeField] private Vector3 endScreenHidePos, endScreenShowPos;
    [SerializeField] private Button mainMenuBtn, restartBtn;

    [Header("Tutorial")]
    [SerializeField] private TextMeshProUGUI tutorialTxt;

    [Header("Worldspace canvas ui indicator")]
    public GameObject worldSpaceCanvas;
    public Vector3 offset;
    public QuizButton ChoiceA { get => choiceA; set => choiceA = value; }
    public QuizButton ChoiceB { get => choiceB; set => choiceB = value; }
    public QuizButton ChoiceC { get => choiceC; set => choiceC = value; }
    public QuizButton ChoiceD { get => choiceD; set => choiceD = value; }

    public GameObject CinematicUI { get => cinematicUI; }
    public Canvas MainCanvas { get => mainCanvas; }
    public Transform IndicatorParent { get => indicatorParent; set => indicatorParent = value; }

    public ChoicesButtonController ChoicesController { get => choicesController; }

    public bool IsInventoryOpen { get => isInventoryOpen; }
    public bool IsChoiceOpen { get => isChoicesOpen; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        statCloseButton.onClick.AddListener(OnCloseStatisticUI);
        SetupInventoryUI();
        SetCurrentComponent();
        motherBoardBtn.onClick.AddListener(OnInstallMotherboard);

        SetupButtonOnclick();
    }

    private void SetupButtonOnclick()
    {
        var sceneLoader = SceneLoaderManager.Instance;

        restartBtn.onClick.AddListener(sceneLoader.RestartLevel);
        mainMenuBtn.onClick.AddListener(()=>sceneLoader.LoadLevelAsAsync("MainMenu"));

        menuRestartBtn.onClick.AddListener(sceneLoader.RestartLevel);
        menuMainMenuBtn.onClick.AddListener(() => sceneLoader.LoadLevelAsAsync("MainMenu"));

        statCloseButton.onClick.AddListener(() => sceneLoader.LoadLevelAsAsync("MainMenu"));
        Debug.Log("Setup button complete");
    }

    private void LateUpdate()
    {
        timerText.text = StatisticsManager.Instance.FormattedTime();
        SetTutorialUI();// to be refactor nee to remove here
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ShowNotification();
        }

        if (StatisticsManager.Instance.CountDown <= 0 && StatisticsManager.Instance.CanTimerStart)
        {
            isTimerShown = true;
            StatisticsManager.Instance.IsTimerStart = true;
            ToggleTimer();
        }

        
    }

    #region Tutorial
    public void SetTutorialUI()
    {
        if (SceneLoaderManager.Instance.currentGameType == GameType.tutorial)
        {
            var tutorial = TutorialManager.Instance.CurrentTutorial;
            if (tutorial.tutorialName != "")
            {
                tutorialTxt.text = tutorial.tutorialName;
            }
        }
        
        
    }
    #endregion
    public void HideUnnecessaryUI()
    {
        if (isInventoryOpen)
        {
            OnHideInventory();
            isInventoryOpen = false;
        }

        if (isChoicesOpen)
        {
            OnHideChoicesPanel();
            isChoicesOpen = false;
        }
    }

    public void SetStatisticUI()
    {
        correctAnswerText.text = StatisticsManager.Instance.correctAnswersCounter.ToString();
        mistakeText.text = StatisticsManager.Instance.mistakeCounter.ToString();
    }

    public void OnLevelCompleteUI()
    {
        levelCompleteUI.SetActive(true) ;
    }
    public void SetQuizUI(string quizTitle)
    {
        quizNameTxt.text = quizTitle;
        choiceA.SetUI();
        choiceB.SetUI();
        choiceC.SetUI();
        choiceD.SetUI();
    }

    public void ShowQuiz()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(triviaTransform.DOScale(scaleFactor, duration).SetEase(easeType));
        sequence.Join(triviaCanvasGroup.DOFade(1f, duration).SetEase(easeType));
        SetChoicesButtonToNormal();
        triviaCanvasGroup.blocksRaycasts = true;
        triviaCanvasGroup.interactable = true;
        
    }

    public void HideQuiz()
    {
        
        Sequence sequence = DOTween.Sequence();
       
        sequence.Join(triviaTransform.DOScale(0, duration).SetEase(easeType));
        sequence.Join(triviaCanvasGroup.DOFade(0, duration).SetEase(easeType));
      
        triviaCanvasGroup.blocksRaycasts = false;
        triviaCanvasGroup.interactable = false;
        

    }

    public IEnumerator HideQuizCoroutine()
    {
        yield return new WaitForSeconds(2f);
        HideQuiz();
    }

    private void SetChoicesButtonToNormal()
    {
        Debug.Log("Setting button choice to normal");
        choiceA.SetToNormal();
        choiceB.SetToNormal();
        choiceC.SetToNormal();
        choiceD.SetToNormal();
    }
    public void SetQuizChoiceButton(string a,string b,string c ,string d)
    {
        choiceA.SetChoice(a);
        choiceB.SetChoice(b);
        choiceC.SetChoice(c);
        choiceD.SetChoice(d);            
    }

    public void SetQuizAnswer(string a,string b,string c,string d)
    {
        choiceA.SetAnswer(a);
        choiceB.SetAnswer(b);
        choiceC.SetAnswer(c);
        choiceD.SetAnswer(d);
    }

    

    public void OnInstallMotherboard()
    {       
        
        GameManager.Instance.SwitchCamera("motherboard");// to be refactor
        PCComponentManager.Instance.GetParts("motherboard");
        GameManager.Instance.CurrentMainCamera = "motherboard";
        GameManager.Instance.IsMotherboardInstalled = true;

        TutorialManager.Instance.CompleteTutorialSteps();

        EnableInstallMotherboardUI(false);
        ShowControlPanelUI(true);
        OnInstallMotherBoardEvent?.Invoke();
    }

   


    public void ActivateLoadingScreen()// need to be refactor
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1f);
        //sequence.AppendCallback(()=>EnableLoadingObject(true));
        sequence.Append(loadingCanvasGroup.DOFade(1f, .7f).SetEase(Ease.InSine));
        loadingCanvasGroup.blocksRaycasts = true;
        loadingCanvasGroup.interactable = true;
        sequence.AppendInterval(1f);
        sequence.Append(loadingCanvasGroup.DOFade(0, .7f).SetEase(Ease.OutSine));
        loadingCanvasGroup.blocksRaycasts = false;
        loadingCanvasGroup.interactable = false;
        sequence.AppendInterval(.5f);
        // sequence.AppendCallback(() => EnableLoadingObject(false));
    }

    public void FastLoadingScreen()// need to remove if loading screen is refactor
    {
        loadingCanvasGroup.alpha = 1f;
        Sequence sequence = DOTween.Sequence();
        //sequence.Append(loadingCanvasGroup.DOFade(1f, .2f).SetEase(Ease.InSine));
        loadingCanvasGroup.blocksRaycasts = true;
        loadingCanvasGroup.interactable = true;
        sequence.AppendInterval(.5f);
        sequence.Append(loadingCanvasGroup.DOFade(0, .2f).SetEase(Ease.OutSine));
        loadingCanvasGroup.blocksRaycasts = false;
        loadingCanvasGroup.interactable = false;
        //sequence.AppendInterval(.5f);
    }

    public void EnableInstallMotherboardUI(bool isEnable)
    {
        InstallMotherboard.SetActive(isEnable);

        TutorialManager.Instance.CurrentSelectedInteractivity = interactivity;
    }

    public void OnClickMenu()
    {
        Sequence sequence = DOTween.Sequence();
        Ease temEase = Ease.OutExpo;
        float temDuration = .1f;
        sequence.Append(menuUI.DOScaleX(1f, temDuration).SetEase(temEase));
        sequence.Join(menuUI.DOScaleY(.003f, temDuration).SetEase(temEase));

        sequence.AppendInterval(.1f);
        //sequence.Append(menuUI.DOAnchorPos(showPosition, duration).SetEase(easeType));
        sequence.Join(menuCanvasGroup.DOFade(1f, .1f).SetEase(Ease.InSine));
        sequence.Append(menuUI.DOScaleY(1f, temDuration).SetEase(temEase));

        foreach (var item in menuBtnCanvasGroup)
        {
            sequence.Append(item.DOFade(1f, .1f).SetEase(Ease.InSine));
            sequence.Join(item.transform.DOScale(Vector3.one, .1f).SetEase(Ease.InSine));
        }


        menuCanvasGroup.blocksRaycasts = true;
        menuCanvasGroup.interactable = true;
    }

    public void OnClickResume()
    {
        Sequence sequence = DOTween.Sequence();
        Ease temEase = Ease.OutExpo;
        float temDuration = .1f;
        foreach (var item in menuBtnCanvasGroup)
        {
            sequence.Join(item.DOFade(0, .1f).SetEase(temEase));
            sequence.Join(item.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), .1f).SetEase(temEase));
        }
        sequence.Append(menuUI.DOScaleY(.003f, temDuration).SetEase(temEase));

        sequence.AppendInterval(.05f);

        sequence.Append(menuUI.DOScaleX(0, temDuration).SetEase(temEase));
        sequence.Join(menuUI.DOScaleY(0, temDuration).SetEase(temEase));
        sequence.Join(menuCanvasGroup.DOFade(0, .3f).SetEase(Ease.OutSine));


        menuCanvasGroup.blocksRaycasts = false;
        menuCanvasGroup.interactable = false;
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    private void EnableLoadingObject(bool isActive)
    {
        loadingScreenUI.SetActive(isActive);
    }
    public void OnRotateRight()
    {
        PCComponentManager.Instance.CurrentSelectedPart.RotateRight();
        StartCoroutine(ButtonDisableCoroutine());
    }

    public void OnRotateLeft()
    {
        PCComponentManager.Instance.CurrentSelectedPart.RotateLeft();
        StartCoroutine(ButtonDisableCoroutine());
    }

    IEnumerator ButtonDisableCoroutine()
    {
        rotateLeftBtn.enabled = false;
        rotateRightBtn.enabled = false;

        yield return new WaitForSeconds(1f);

        rotateLeftBtn.enabled = true;
        rotateRightBtn.enabled = true;

    }

    private void OnDone(PcPart currentSelectedPart,string partName)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() => TimelineManager.Instance.PlayCinematic());
        sequence.AppendInterval(.3f);
        sequence.AppendCallback(() => currentSelectedPart.MoveAnimation());
        sequence.AppendInterval(1f);
        ActivateLoadingScreen();
        sequence.AppendInterval(1f);
        sequence.AppendCallback(() => GameManager.Instance.ResetCamera());
        sequence.AppendInterval(1f);
        sequence.AppendCallback(() => PCComponentManager.Instance.ActivatePart(currentSelectedPart.referencePartName != "" ? currentSelectedPart.referencePartName : partName));
        sequence.AppendCallback(() => PCComponentManager.Instance.DeactivateParts(partName));
        sequence.AppendCallback(()=> CinematicUI.SetActive(false));

       // QuestManager.Instance.CompleteStep(currentStep.componentPartName, currentStep.stepId);
    }
    public void OnClickDone()
    {
        PcPart currentSelectedPart = PCComponentManager.Instance.CurrentSelectedPart;
        StepController currentStep = currentSelectedPart.GetComponent<StepController>();
        string partName = currentSelectedPart.partsName;      
        LocationTrigger currentLocationTrigger = LocationTriggerManager.Instance.CurrentLocationTrigger;

        bool isRotationCorrect = false;


        if (currentSelectedPart.isDontHaveControlUI)
        {
            OnDone(currentSelectedPart,partName);
        }
        else
        {
             isRotationCorrect = PCComponentManager.Instance.IsRotationCorrect(currentSelectedPart.gameObject.transform);
            if (isRotationCorrect)
            {
                OnDone(currentSelectedPart, partName);
                if (PCComponentManager.Instance.CurrentSelectedInteractablePart != null)
                {
                    var currentInteractable = PCComponentManager.Instance.CurrentSelectedInteractablePart;


                    currentInteractable.Slot.SlotCollider.enabled = false;
                    currentInteractable.DeactivateObject();
                    currentInteractable = null;
                    //PCComponentManager.Instance.CurrentSelectedInteractablePart.DeactivateObject();
                    //PCComponentManager.Instance.CurrentSelectedInteractablePart = null;
                }

                TutorialManager.Instance.CompleteTutorialSteps();//check for if tutorial mode


            }
            else
            {
                Debug.Log("wrong placement");
                StatisticsManager.Instance.CountPlaceMistake();
                UIManager.Instance.WrongFeedback();
            }
        }

        QuestManager.Instance.CompleteStep(currentStep.componentPartName, currentStep.stepId);
        QuestManager.Instance.CheckGroupIfComplete(GameManager.Instance.currentSelectedGroup);
        QuestManager.Instance.CheckIfQuestCompleted(currentStep.componentPartName);
        if (QuestManager.Instance.IsAllQuestFinished())
        {
            Debug.Log("ALL QUEST COMPLETED");
            StatisticsManager.Instance.IsTimerStart = false;
            StatisticsManager.Instance.CanTimerStart = false;
            StartCoroutine(SetStatisticCoroutine());
            GameManager.Instance.Save();
            QuizManager.Instance.CanStartQuiz = false;
            
        }

        if (isRotationCorrect)
        {
            if (GameManager.Instance.currentSelectedGroup == ComponentGroup.motherboard)
            {
                if (QuestManager.Instance.CheckIfQuestCompletedFilter(ComponentGroup.motherboard, "motherboard") && GameManager.Instance.IsMotherboardInstalled == false)
                {
                    //EnableInstallMotherboardUI(true);
                    StartCoroutine(EnableInstallMotherboardUICoroutine());
                }

            }
        }
       
        SetCurrentComponent();

       
        #region // old code
        //if (PCComponentManager.Instance.IsRotationCorrect(currentSelectedPart.gameObject.transform) || currentSelectedPart.isDontHaveControlUI)
        //{
        //    //play animation or change the camera view to overall view
        //    //QuestManager.Instance.CompleteCurrentQuest(partName);
        //    Sequence sequence = DOTween.Sequence();
        //    sequence.AppendCallback(()=> TimelineManager.Instance.PlayCinematic());
        //    sequence.AppendInterval(.3f);
        //    sequence.AppendCallback(()=> currentSelectedPart.MoveAnimation());
        //    sequence.AppendInterval(1f);
        //    ActivateLoadingScreen();
        //    sequence.AppendInterval(1f);
        //    sequence.AppendCallback(() => GameManager.Instance.ResetCamera());
        //    sequence.AppendInterval(1f);
        //    sequence.AppendCallback(()=> PCComponentManager.Instance.ActivatePart(currentSelectedPart.referencePartName != "" ? currentSelectedPart.referencePartName: partName));          
        //    sequence.AppendCallback(() => PCComponentManager.Instance.DeactivateParts(partName));


        //    QuestManager.Instance.CompleteStep(currentStep.componentPartName,currentStep.stepId);

        //    Debug.Log("Correct placement");
        //}
        //else
        //{
        //    Debug.Log("wrong placement");
        //    UIManager.Instance.WrongFeedback();
        //    //show some red overlay feedback
        //}
        #endregion
        //if (QuestManager.Instance.CheckIfQuestCompleted())
        //{
        //    // coroutine display finished game ui
        //}

    }

    private IEnumerator EnableInstallMotherboardUICoroutine()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("MotherboardCoroutine");
        EnableInstallMotherboardUI(true);
    }

    public void ShowControlPanelUI(bool isActive)
    {
        controlUI.gameObject.SetActive(isActive);
        if (isActive)
        {
            hidePanelUI.gameObject.SetActive(false);
        }
        else
        {
            hidePanelUI.gameObject.SetActive(true);
        }
    }

    public void WrongFeedback()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(wrongFeedbackUI.DOFade(.3f, .1f));
        seq.Append(wrongFeedbackUI.DOFade(0,.1f).SetDelay(.1f));

    }
    #region Choices UI or  Component Part Selection

    public void ToggleChoices()
    {
        isChoicesOpen = !isChoicesOpen;
        if (isChoicesOpen)
        {
            OnShowChoicesPanel();
        }
        else
        {
            OnHideChoicesPanel();
        }
    }
    public void OnShowChoicesPanel()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() => choicesController.HideChoices());
        sequence.Append(choiceCanvasGroup.DOFade(1f, 2f).SetEase(easeType));
        sequence.Join(choicesButton.transform.DOScale(new Vector3(3f, 3f, 3f), duration).SetEase(easeType));
        sequence.Join(hidePanelUI.DOAnchorPos(buttonsPanelHidePos, duration).SetEase(easeType)) ;
        sequence.Join(choicesButton.transform.DOScale(new Vector3(1f, 1f, 1f), duration).SetEase(easeType));
        sequence.Join(choicesPanelUI.DOAnchorPos(choicePanelShowPosition, duration).SetEase(easeType));
        

        choiceCanvasGroup.blocksRaycasts = true;
        choiceCanvasGroup.interactable = true;
       
    }

    public void OnHideChoicesPanel()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() => choicesController.ShowAllChoices());
        sequence.Append(choiceCanvasGroup.DOFade(0, 2f)).SetEase(easeType);
        sequence.Join(choicesButton.transform.DOScale(new Vector3(3f, 3f, 3f), duration).SetEase(easeType));
        sequence.Join(hidePanelUI.DOAnchorPos(buttonsPanelShowPos, duration).SetEase(easeType));
        sequence.Join(choicesButton.transform.DOScale(new Vector3(1f, 1f, 1f), duration).SetEase(easeType));
        sequence.Join(choicesPanelUI.DOAnchorPos(choicePanelHidePosition, duration).SetEase(easeType));
        

        choiceCanvasGroup.blocksRaycasts = false;
        choiceCanvasGroup.interactable = false;
        
    }
    #endregion

    public void OnChoicesButtonClick(Button currentBtn)
    {
        //ActivateLoadingScreen();
        FastLoadingScreen();
        if (currentSelectedBtn!=null)
        {
            prevChoiceBtn = currentSelectedBtn;
          
            
        }

        if (prevChoiceBtn!= null)
        {
            prevChoiceBtn.isSelected = false;
        }

        currentSelectedBtn = currentBtn.GetComponent<ChoicesButton>();
        currentSelectedBtn.isSelected = !currentSelectedBtn.isSelected;
        GameManager.Instance.CurrentMainCamera =  currentSelectedBtn.mainVirtualCameraName;
        GameManager.Instance.StartTimer();
       

        if (currentSelectedBtn.isSelected)
        {
            HideOrShowCanvasGroup(contentCanvasGroup, false);
           // contentImage.enabled = true;
            //contentImage.transform.SetSiblingIndex(currentBtn.transform.GetSiblingIndex() + 1);
            
        }
        else
        {
            HideOrShowCanvasGroup(contentCanvasGroup, true);
            //contentImage.enabled = false;
            //contentImage.transform.SetSiblingIndex(choicesBtns.Count + 1);
        }



        GameManager.Instance.SwitchCamera(GameManager.Instance.CurrentMainCamera);
        OnHideChoicesPanel();
       
    }

    public void SetComponentGroupComplete()
    {
        var currentChoiceButton = choicesBtns.Find(x => x.group == GameManager.Instance.currentSelectedGroup);
        if (currentChoiceButton != null)
        {
            currentChoiceButton.CompleteState();
        }
    }
    public void SetCurrentComponent()
    {
        //GameManager.Instance.currentSelectedGroup = ComponentGroup.cables;// to be remove for testing only
        //GameManager.Instance.CurrentMainCamera = "cableVirtualCam";// to be remove for testing only
        for (int i = 0; i < choicesBtns.Count; i++)
        {
            if (choicesBtns[i].group == GameManager.Instance.currentSelectedGroup)
            {
                choicesBtns[i].gameObject.GetComponent<Button>().interactable = true;
                //choicesBtns[i].NormalStateText();
                
            }
            else
            {
                choicesBtns[i].gameObject.GetComponent<Button>().interactable = false;
                choicesBtns[i].DisableState();
            }
        }
    }

    private void HideOrShowCanvasGroup(CanvasGroup canvas,bool isHidden)
    {
        if (isHidden)
        {
            canvas.DOFade(0,duration).SetEase(easeType);
        }
        else
        {
            canvas.DOFade(1f, duration).SetEase(easeType);
        }
    }


    #region Inventory 
    public void SetupInventoryUI()
    {
        var itemList = InventoryManager.Instance.itemSO.items;
        for (int i = 0; i < itemList.Count; i++)
        {
            var currentItem = itemList[i];
            var go = Instantiate(inventoryPrefab);
            go.transform.SetParent(parent, false);
            var inventoryUI = go.GetComponent<InventoryUI>();
            inventoryUI.Setup(currentItem.itemImage, currentItem.itemName, currentItem.itemPrefab);
            InventoryManager.Instance.inventoryItems.Add(inventoryUI);
            //TutorialManager.Instance.objectsToHide.Add(inventoryUI.Interactivity);
        }
    }
   
    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        if (isInventoryOpen)
        {
            OnShowInventory();
        }
        else
        {
            OnHideInventory();
        }
    }

    public void OnShowInventory()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() => choicesController.HideChoices());
        sequence.Append(inventoryCanvasGroup.DOFade(1f, duration).SetEase(easeType));
        sequence.Join(inventoryButton.transform.DOScale(new Vector3(3f, 3f, 3f), duration).SetEase(easeType));            
        //sequence.Join(inventoryButton.transform.DORotate(new Vector3(0,0,180f),duration).SetEase(easeType));        
        sequence.Join(inventoryRect.DOAnchorPos(inventoryShowPos,duration).SetEase(easeType));
        sequence.Join(inventoryButton.transform.DOScale(new Vector3(1f, 1f, 1f), duration).SetEase(easeType));
        sequence.Join(hidePanelUI.DOAnchorPos(buttonsPanelHidePos, duration).SetEase(easeType));
        inventoryCanvasGroup.blocksRaycasts = true;
        inventoryCanvasGroup.interactable = true;

        
    }

    public void OnHideInventory()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() => choicesController.ShowAllChoices());
        sequence.Join(inventoryRect.DOAnchorPos(inventoryHidePos, duration).SetEase(easeType));
        sequence.Join(inventoryButton.transform.DOScale(new Vector3(3f, 3f, 3f), duration).SetEase(easeType));
        //sequence.Join(inventoryButton.transform.DORotate(new Vector3(0, 0, 0), duration).SetEase(easeType));
        sequence.Join(inventoryButton.transform.DOScale(new Vector3(1f, 1f, 1f), duration).SetEase(easeType));
        sequence.Join(hidePanelUI.DOAnchorPos(buttonsPanelShowPos, duration).SetEase(easeType));
        sequence.Append(inventoryCanvasGroup.DOFade(0, duration).SetEase(easeType));

        inventoryCanvasGroup.blocksRaycasts = false;
        inventoryCanvasGroup.interactable = false;

        showInventoryCanvasGroup.blocksRaycasts = true;
        showInventoryCanvasGroup.interactable = true;
        //sequence.Play();
    }
    #endregion

    #region Timer UI

    public void ToggleTimer()
    {
        //isTimerShown = !isTimerShown;
        if (isTimerShown)
        {
            ShowTimer();
        }
        else
        {
            HideTimer();
        }
    }

    public void ShowTimer()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(timerRect.transform.DOScale(new Vector3(.9f, .9f, .9f), duration).SetEase(easeType));
        sequence.Join(timerRect.DOAnchorPos(timerShowPos,duration).SetEase(easeType));
        sequence.Join(timerCanvasGroup.DOFade(1f, duration).SetEase(easeType));             
        sequence.Append(timerRect.transform.DOScale(new Vector3(1f, 1f, 1f), duration).SetEase(easeType));
        sequence.Append(timerIconRect.DORotate(new Vector3(0,0,180f),duration).SetEase(easeType));
    }

    public void HideTimer()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(timerCanvasGroup.DOFade(0, duration).SetEase(easeType));
        sequence.Join(timerRect.transform.DOScale(new Vector3(1f, 1f, 1f), duration).SetEase(easeType));
        sequence.Join(timerRect.DOAnchorPos(timerHidePos, duration).SetEase(easeType));
        sequence.Append(timerIconRect.DORotate(new Vector3(0, 0, 0), duration).SetEase(easeType));

    }

    #endregion

    #region Statistic UI

    public void OnCloseStatisticUI()
    {
        HideStatisticUI();
        //change scene
        //save data
    }
    public void ToggleStatisticUI()
    {
        isStatShow = !isStatShow;
        if (isStatShow)
        {
            ShowStatisticUI();
        }
        else
        {
            HideStatisticUI();
        }
    }

    private IEnumerator SetStatisticCoroutine()
    {
        yield return new WaitForSeconds(2.5f);
        if (SceneLoaderManager.Instance.currentGameType == GameType.asessment)
        {
            SetStatisticInfoUI();
            ShowStatisticUI();
        }
        else
        {
            ShowEndScreen();
        }
            
    }
    public void SetStatisticInfoUI()
    {
        timeText.text = StatisticsManager.Instance.FormattedTime();
        correctAnswerText.text = StatisticsManager.Instance.correctAnswersCounter.ToString();
        mistakeText.text = StatisticsManager.Instance.mistakeCounter.ToString();
    }
    public void ShowStatisticUI()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(statRect.DOAnchorPos(statShowPos,duration).SetEase(easeType));
        sequence.Append(statisticCanvasGroup.DOFade(1f,duration).SetEase(easeType));

        sequence.Append(timeRect.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), duration).SetEase(easeType));
        sequence.Append(timeRect.transform.DOScale(new Vector3(1f, 1f, 1f), duration).SetEase(easeType));

        sequence.Append(correctAnsRect.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), duration).SetEase(easeType));
        sequence.Append(correctAnsRect.transform.DOScale(new Vector3(1f, 1f, 1f), duration).SetEase(easeType));

        sequence.Append(mistakeRect.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), duration).SetEase(easeType));
        sequence.Append(mistakeRect.transform.DOScale(new Vector3(1f, 1f, 1f), duration).SetEase(easeType));

        statisticCanvasGroup.interactable = true;
        statisticCanvasGroup.blocksRaycasts = true;

    }

    public void HideStatisticUI()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(statRect.DOAnchorPos(statHidePos, duration).SetEase(easeType));
        sequence.Join(statisticCanvasGroup.DOFade(0, duration).SetEase(easeType));

        sequence.Join(timeRect.transform.DOScale(new Vector3(0, 0, 0), duration).SetEase(easeType));
        sequence.Join(correctAnsRect.transform.DOScale(new Vector3(0, 0, 0), duration).SetEase(easeType));
        sequence.Join(mistakeRect.transform.DOScale(new Vector3(0, 0, 0), duration).SetEase(easeType));

        statisticCanvasGroup.interactable = false;
        statisticCanvasGroup.blocksRaycasts = false;
    }

    #endregion

    #region Notification UI
    public void ShowNotification()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1f);
        sequence.Join(notificationRect.DOAnchorPos(notifShowPos,duration).SetEase(easeType));
        sequence.Join(notifCanvasGroup.DOFade(1f, duration).SetEase(easeType));
        sequence.AppendInterval(2f);
        sequence.Append(notificationRect.DOAnchorPos(notifHidePos, duration).SetEase(easeType));
        sequence.Join(notifCanvasGroup.DOFade(0, duration).SetEase(easeType));
    }

    public void SetNotificationInfoUI(string info)
    {
        notificationTxt.text = info;
    }
    #endregion

    #region End screen for tutorial and practice mode

    public void SetEndScreenText()
    {
        if (SceneLoaderManager.Instance == null)
        {
            return;
        }
        switch (SceneLoaderManager.Instance.currentGameType)
        {
            case GameType.none:
                break;
            case GameType.tutorial:
                endScreenTxt.text = $"TUTORIAL FINISHED!";
                break;
            case GameType.practice:
                endScreenTxt.text = $"PRACTICE FINISHED!";
                break;
            case GameType.asessment:
                break;
           
        }
    }
    public void ShowEndScreen()//for practice and tutorial only
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(endScreenCanvas.DOFade(1f, .5f).SetEase(easeType));

        sequence.Join(endScreenRect.DOAnchorPos(endScreenShowPos, duration).SetEase(easeType));


        endScreenCanvas.blocksRaycasts = true;
        endScreenCanvas.interactable = true;
        SetEndScreenText();

    }

    public void HideEndScreen()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(endScreenCanvas.DOFade(0, 2f).SetEase(easeType));

        sequence.Join(endScreenRect.DOAnchorPos(endScreenHidePos, duration).SetEase(easeType));


        endScreenCanvas.blocksRaycasts = false;
        endScreenCanvas.interactable = false;
    }
    #endregion
}
