using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TestUIManager : MonoBehaviour
{
    public static TestUIManager Instance;
    [SerializeField] private GameObject testPref;
    [SerializeField] private SaveManager saveMan;
    [SerializeField] RectTransform parent;

    [SerializeField] private GameObject loadingScreen;

    [Header("Animation settings")]
    [SerializeField] private float duration;
    [SerializeField] private Ease easeType;

    [Header("Main Panel")]
    [SerializeField] private CanvasGroup mainPanelCanvas;

    [Header("game mode ui")]
    [Space(2f)]
    [SerializeField] private CanvasGroup modeCanvasGroup;
    [SerializeField] private RectTransform modeRect;

    [Header("stats  ui")]
    [Space(2f)]
    [SerializeField] private CanvasGroup statsCanvasGroup;
    [SerializeField] private RectTransform statRect;

    [Header("buttons  ui")]
    [Space(2f)]
    [SerializeField] private Button assesmentBtn, practiceBtn, tutorialBtn, statBtn,quitBtn,infoButton;


    [Header("Panel Positions")]
    [Space(2f)]
    [SerializeField] private Vector3 hidePos;
    [SerializeField] private Vector3 showPos;

    [Header("Summary and Chart")]
    [SerializeField] private CanvasGroup summaryCanvas, chartCanvas;
    [SerializeField] private Button summaryBtn, chartBtn;
    

    [Header("Button Tracker")]
    [SerializeField] private SelectedButtonTracker buttonTracker;
    [Header("Summary and Chart button tracker")]
    [SerializeField] private SelectedButtonTracker tabButtonTracker;

    public GameObject LoadingScreen { get => loadingScreen; set => loadingScreen = value; }
    public SaveManager SaveMan { get => saveMan; set => saveMan = value; }

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        InitializeSaveData();
        Setup();
    }

    public void InitializeSaveData()
    {
        var loadedData = SaveMan.LoadDataResult();
        for (int i = 0; i < loadedData.saveDataList.Count; i++)
        {
            var data = loadedData.saveDataList[i];
            GameObject go = Instantiate(testPref,parent);
            var statUI = go.GetComponent<StatLogUI>();
            statUI.SetUI(data.gameMode,data.timeTake,data.correctCounts,data.mistakesCount,data.timeStamp,data.overall);
            go.SetActive(true);
        }
    }


    private void Setup()
    {
        assesmentBtn.onClick.AddListener(()=> OnSelectMode(GameType.asessment));
        practiceBtn.onClick.AddListener(() => OnSelectMode(GameType.practice));
        tutorialBtn.onClick.AddListener(() => OnSelectMode(GameType.tutorial));
        infoButton.onClick.AddListener(OnClickInfoButton);
        statBtn.onClick.AddListener(OnSelectStats);
        quitBtn.onClick.AddListener(OnGameQuit);

        chartBtn.onClick.AddListener(OnChartClick);
        summaryBtn.onClick.AddListener(OnSummaryClick);
    }

    public void OnChartClick()
    {
        HideOrShowCanvas(chartCanvas,true);
        HideOrShowCanvas(summaryCanvas,false);
        Sequence sequence = DOTween.Sequence();
        sequence.Join(chartCanvas.DOFade(1f,duration).SetEase(easeType));
        sequence.Join(summaryCanvas.DOFade(0, duration).SetEase(easeType));
    }

    public void OnSummaryClick()
    {
        HideOrShowCanvas(chartCanvas, false);
        HideOrShowCanvas(summaryCanvas, true);
        Sequence sequence = DOTween.Sequence();
        sequence.Join(chartCanvas.DOFade(0, duration).SetEase(easeType));
        sequence.Join(summaryCanvas.DOFade(1f, duration).SetEase(easeType));
    }

    public void OnSelectMode(GameType gameType)
    {
        string cameraName = "";
        SceneLoaderManager.Instance.currentGameType = gameType;
        switch (gameType)
        {
           
            case GameType.tutorial:
                cameraName = "tutorial";
                break;
            case GameType.practice:
                cameraName = "practice";
                break;
            case GameType.asessment:
                cameraName = "assesment";
                break;
           
        }
       // MainMenuCamera.Instance.SwitchCam(cameraName);
        ShowGameMode(cameraName);
    }

    public void OnBack(string currentScreen)
    {
        if (currentScreen == "gameMode")
        {
            HideGameMode();
        }
        else if (currentScreen =="stat")
        {
            OnHideStats();
        }
        MainMenuCamera.Instance.SwitchToMainCam();
        buttonTracker.ResetAll();
        tabButtonTracker.ResetAll();
        mainPanelCanvas.DOFade(1f, duration).SetEase(easeType);
        HideOrShowCanvas(mainPanelCanvas, true);
    }

    public void ShowGameMode(string camName)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(()=> MainMenuCamera.Instance.SwitchCam(camName));
        sequence.AppendInterval(.5f);
        sequence.Append(modeRect.DOAnchorPos(showPos,duration).SetEase(easeType));
        sequence.Join(modeCanvasGroup.DOFade(1f, duration).SetEase(easeType));
        HideOrShowCanvas(modeCanvasGroup, true);

        //mainPanelCanvas.DOFade(.1f, duration).SetEase(easeType);
        HideOrShowCanvas(mainPanelCanvas, false);
    }

    public void HideGameMode()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(modeRect.DOAnchorPos(hidePos, duration).SetEase(easeType));
        sequence.Join(modeCanvasGroup.DOFade(0,duration).SetEase(easeType));
        HideOrShowCanvas(modeCanvasGroup, false);
    }

    public void OnSelectStats()
    {

        //InitializeSaveData();
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() => MainMenuCamera.Instance.SwitchCam("history"));
        sequence.AppendInterval(.5f);
        sequence.Append(statRect.DOAnchorPos(showPos, duration).SetEase(easeType));
        sequence.Join(statsCanvasGroup.DOFade(1f, duration).SetEase(easeType));
        HideOrShowCanvas(statsCanvasGroup, true);

        tabButtonTracker.CurrentSelectedButton = summaryBtn.gameObject.GetComponent<ButtonAnimator>();
        tabButtonTracker.SelectCurrentButton();
        OnSummaryClick();

        mainPanelCanvas.DOFade(.1f,duration).SetEase(easeType);
        HideOrShowCanvas(mainPanelCanvas,false);
    }

    public void OnHideStats()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(statRect.DOAnchorPos(hidePos, duration).SetEase(easeType));
        sequence.Join(statsCanvasGroup.DOFade(0, duration).SetEase(easeType));
        HideOrShowCanvas(statsCanvasGroup, false);
    }

    private void HideOrShowCanvas(CanvasGroup canvas,bool isEnable)
    {
        if (isEnable)
        {
            canvas.blocksRaycasts = true;
            canvas.interactable = true;
        }
        else
        {
            canvas.blocksRaycasts = false;
            canvas.interactable = false;
        }
    }

    private void OnClickInfoButton()
    {
        MainMenuCamera.Instance.SwitchCam("explore");
        StartCoroutine(SwitchSceneCoroutine());
    }

    private IEnumerator SwitchSceneCoroutine()
    {
        yield return new WaitForSeconds(2f);
        SceneLoaderManager.Instance.LoadLevelAsAsync("Information");
    }
    private void OnGameQuit()
    {
        Application.Quit();
    }
    
}
