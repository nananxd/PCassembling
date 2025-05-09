using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Linq;
using System.Text;

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
    [SerializeField] private TextMeshProUGUI modeTxt;

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
    [SerializeField] private CanvasGroup summaryCanvas, chartCanvas,analyticsAssemblyCanvas,analyticsDisassemblyCanvas;
    [SerializeField] private Button summaryBtn, chartBtn,analyticsAssemblyButton,analyticsDisassemblyButton;
    

    [Header("Button Tracker")]
    [SerializeField] private SelectedButtonTracker buttonTracker;
    [Header("Summary and Chart button tracker")]
    [SerializeField] private SelectedButtonTracker tabButtonTracker;

    [Header("Assembly analytics")]
    [SerializeField] private GameObject assemblyTopErrorUI,assemblyErrorParent;
    [SerializeField] private TextMeshProUGUI totalAssembly;
    [SerializeField] private TextMeshProUGUI assemblyAveTime;
    [SerializeField] private TextMeshProUGUI assemblyFastestTime;
    [SerializeField] private TextMeshProUGUI assemblySlowestTime;
    [SerializeField] private TextMeshProUGUI assemblyMostMistake;
    [SerializeField] private TextMeshProUGUI assemblyStatus;
    [SerializeField] private TextMeshProUGUI assemblyTopError;
    [Header("Disassembly analytics")]
    [SerializeField] private GameObject disassemblyTopErrorUI,disassemblyErrorParent;
    [SerializeField] private TextMeshProUGUI totalDisassembly;
    [SerializeField] private TextMeshProUGUI disassemblyAveTime;
    [SerializeField] private TextMeshProUGUI disassemblyFastestTime;
    [SerializeField] private TextMeshProUGUI disassemblySlowestTime;
    [SerializeField] private TextMeshProUGUI disassemblyMostMistake;
    [SerializeField] private TextMeshProUGUI disassemblyStatus;
    [SerializeField] private TextMeshProUGUI disassemblyTopError;

    [SerializeField] private float totalAssemblyTime,totalDisassemblyTime;

    [Header("Color of Status Text")]
    [SerializeField] private Color improvingColor;
    [SerializeField] private Color needImprovementColor;

    public int improving ;
    public int gettingWorse;
    public int same;

    public GameObject LoadingScreen { get => loadingScreen; set => loadingScreen = value; }
    public SaveManager SaveMan { get => saveMan; set => saveMan = value; }

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        InitializeSaveData();
        InitializeAnalytics();
        Setup();
    }



    public void InitializeSaveData()
    {
        var loadedData = saveMan.LoadDataResult();
        TimeUtility(loadedData.saveDataList);
        ErrorStatus(loadedData.saveDataList);
        for (int i = 0; i < loadedData.saveDataList.Count; i++)
        {
            var data = loadedData.saveDataList[i];
            GameObject go = Instantiate(testPref,parent);
            var statUI = go.GetComponent<StatLogUI>();
            statUI.SetUI(data.gameMode,data.timeTake,data.correctCounts,data.mistakesCount,data.timeStamp,data.overall);
            TotalTimeTaken(data.gameMode,data.timeTake);
            go.SetActive(true);
        }

        //totalAssembly.text = $"{ FormatSecondsToMinutesSeconds(totalAssemblyTime)}";
        //totalDisassembly.text = $"{FormatSecondsToMinutesSeconds(totalDisassemblyTime)}";

        totalAssembly.text = $"{PlayerPrefs.GetInt("assemble")}";
        totalDisassembly.text = $"{PlayerPrefs.GetInt("disassemble")}";

        //totalAssembly.text = $"Total Assemblies \t{ FormatSecondsToMinutesSeconds(totalAssemblyTime)}";
        //totalDisassembly.text = $"Total Disassemblies \t{FormatSecondsToMinutesSeconds(totalDisassemblyTime)}";
    }

    private void ErrorStatus(List<SaveData> data)
    {
        var assemble = data.FindAll(x => x.gameMode.ToLower() == "assemble");
        var disassemble = data.FindAll(y => y.gameMode.ToLower() == "disassemble");

        if (assemble.Count > 0)
        {
            var assembleError = assemble.Select(e => e.mistakesCount).ToList();
            //assemblyStatus.text = $"Status \t{AnalyzeErrorTrend(assembleError)}";
            assemblyStatus.text = $"{AnalyzeErrorTrend(assembleError)}";
            assemblyStatus.color = GetStatusColor(AnalyzeErrorTrend(assembleError));
        }

        if (disassemble.Count > 0)
        {
            var disassembleError = disassemble.Select(e => e.mistakesCount).ToList();
            //disassemblyStatus.text = $"Status \t{AnalyzeErrorTrend(disassembleError)}";
            disassemblyStatus.text = $"{AnalyzeErrorTrend(disassembleError)}";
            disassemblyStatus.color = GetStatusColor(AnalyzeErrorTrend(disassembleError));
        }
    }

    private void TimeUtility(List<SaveData> saveData)
    {
        var assemble = saveData.FindAll(x => x.gameMode.ToLower() == "assemble");
        var disassemble = saveData.FindAll( y => y.gameMode.ToLower() == "disassemble");

       

        if (assemble.Count > 0)
        {
            var aAverage = assemble.Average(t => t.timeTake);
            var aSlowest = assemble.Min(t => t.timeTake);
            var aFastest = assemble.Max(t => t.timeTake);

            //assemblyAveTime.text = $"Average Assembly Time \t{FormatSecondsToMinutesSeconds(aAverage)}";
            //assemblyFastestTime.text = $"Fastest Assembly Completion\t{FormatSecondsToMinutesSeconds(aSlowest)}";
            //assemblySlowestTime.text = $"Slowest Assembly Completion\t{FormatSecondsToMinutesSeconds(aFastest)}";

            assemblyAveTime.text = $"{FormatSecondsToMinutesSeconds(aAverage)}";
            assemblyFastestTime.text = $"{FormatSecondsToMinutesSeconds(aSlowest)}";
            assemblySlowestTime.text = $"{FormatSecondsToMinutesSeconds(aFastest)}";
        }


        if (disassemble.Count > 0)
        {
            var dAverage = disassemble.Average(t => t.timeTake);
            var dSlowest = disassemble.Min(t => t.timeTake);
            var dFastest = disassemble.Max(t => t.timeTake);

            //disassemblyAveTime.text = $"Average Disassembly Time\t{FormatSecondsToMinutesSeconds(dAverage)}";
            //disassemblyFastestTime.text = $"Average Disassembly Time\t{FormatSecondsToMinutesSeconds(dSlowest)}";
            //disassemblySlowestTime.text = $"Average Disassembly Time\t{FormatSecondsToMinutesSeconds(dFastest)}";

            disassemblyAveTime.text = $"{FormatSecondsToMinutesSeconds(dAverage)}";
            disassemblyFastestTime.text = $"{FormatSecondsToMinutesSeconds(dSlowest)}";
            disassemblySlowestTime.text = $"{FormatSecondsToMinutesSeconds(dFastest)}";
        }     

       

    }

    public void InitializeAnalytics()
    {
        var loadedData = saveMan.LoadDataErrorList();
        var assemble = loadedData.componentErrors.FindAll(x => !string.IsNullOrEmpty(x.gameMode) && x.gameMode.ToLower() == "assemble");
        var disassemble = loadedData.componentErrors.FindAll(y => !string.IsNullOrEmpty(y.gameMode) && y.gameMode.ToLower() == "disassemble");

        var sortedData = loadedData.componentErrors.OrderByDescending(e => e.errorCount).Take(5).ToList();

        var aSortedData = assemble.OrderByDescending(e => e.errorCount).Take(5).ToList();
        var dSortedData = disassemble.OrderByDescending(e => e.errorCount).Take(5).ToList();

        //if (aSortedData.Count > 0)
        //{
        //    assemblyMostMistake.text = $"Most Common Assembly Mistake\t{aSortedData[0].componentName.ToUpper()}={aSortedData[0].errorCount}";
        //}

        //if (dSortedData.Count > 0)
        //{
        //    disassemblyMostMistake.text = $"Most Common Disassembly Mistake\t{dSortedData[0].componentName.ToUpper()}={dSortedData[0].errorCount}";
        //}
        
       

        StringBuilder sb = new StringBuilder();
        StringBuilder sb2 = new StringBuilder();

        for (int i = 0; i < aSortedData.Count; i++)
        {
            var data = aSortedData[i];
            sb.AppendLine($"{data.componentName.ToUpper()} = {data.errorCount}");

            var gO = Instantiate(assemblyTopErrorUI);
            gO.SetActive(true);
           
            gO.transform.SetParent(assemblyErrorParent.transform);
            var ui = gO.GetComponent<TopErrorUI>();
            ui.Setup(CheckComponentName(data.componentName) ,data.errorCount.ToString());
            gO.transform.localScale =  Vector3.one;
        }

        for (int i = 0; i < dSortedData.Count; i++)
        {
            var data = dSortedData[i];
            sb2.AppendLine($"{data.componentName.ToUpper()} = {data.errorCount}");


            var gO = Instantiate(disassemblyTopErrorUI);
            gO.SetActive(true);
           
            gO.transform.SetParent(disassemblyErrorParent.transform);
            var ui = gO.GetComponent<TopErrorUI>();
            ui.Setup(CheckComponentName(data.componentName), data.errorCount.ToString());
            gO.transform.localScale = Vector3.one;
        }

        //assemblyTopError.text = $"Top Components Where Errors Usually Occur\n \n{sb} ";
        //disassemblyTopError.text = $"Top Components Where Errors Usually Occur\n\n{sb2}";

    }

    private void TotalTimeTaken(string mode, float time)
    {
        if (mode.ToLower() == "assemble")
        {
            totalAssemblyTime += time;
           
           
        }
        else
        {
            totalDisassemblyTime += time;
            
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
        analyticsAssemblyButton.onClick.AddListener(OnAssembleAnalyticsClick);
        analyticsDisassemblyButton.onClick.AddListener(OnDisassembleAnalyticsClick);
    }

    public void OnChartClick()
    {
        HideOrShowCanvas(chartCanvas,true);
        HideOrShowCanvas(summaryCanvas,false);
        HideOrShowCanvas(analyticsAssemblyCanvas,false);
        HideOrShowCanvas(analyticsDisassemblyCanvas, false);

        Sequence sequence = DOTween.Sequence();
        sequence.Join(chartCanvas.DOFade(1f,duration).SetEase(easeType));
        sequence.Join(summaryCanvas.DOFade(0, duration).SetEase(easeType));
        sequence.Join(analyticsAssemblyCanvas.DOFade(0,duration).SetEase(easeType));
        sequence.Join(analyticsDisassemblyCanvas.DOFade(0, duration).SetEase(easeType));
    }

    public void OnSummaryClick()
    {
        HideOrShowCanvas(chartCanvas, false);
        HideOrShowCanvas(summaryCanvas, true);
        HideOrShowCanvas(analyticsAssemblyCanvas, false);
        HideOrShowCanvas(analyticsDisassemblyCanvas, false);
        Sequence sequence = DOTween.Sequence();
        sequence.Join(chartCanvas.DOFade(0, duration).SetEase(easeType));
        sequence.Join(summaryCanvas.DOFade(1f, duration).SetEase(easeType));
        sequence.Join(analyticsAssemblyCanvas.DOFade(0, duration).SetEase(easeType));
        sequence.Join(analyticsDisassemblyCanvas.DOFade(0, duration).SetEase(easeType));
    }

    public void OnAssembleAnalyticsClick()
    {
        HideOrShowCanvas(chartCanvas, false);
        HideOrShowCanvas(summaryCanvas, false);
        HideOrShowCanvas(analyticsAssemblyCanvas, true);
        HideOrShowCanvas(analyticsDisassemblyCanvas, false);
        Sequence sequence = DOTween.Sequence();
        sequence.Join(chartCanvas.DOFade(0, duration).SetEase(easeType));
        sequence.Join(summaryCanvas.DOFade(0, duration).SetEase(easeType));
        sequence.Join(analyticsAssemblyCanvas.DOFade(1f, duration).SetEase(easeType));
        sequence.Join(analyticsDisassemblyCanvas.DOFade(0, duration).SetEase(easeType));
    }

    public void OnDisassembleAnalyticsClick()
    {
        HideOrShowCanvas(chartCanvas, false);
        HideOrShowCanvas(summaryCanvas, false);
        HideOrShowCanvas(analyticsAssemblyCanvas, false);
        HideOrShowCanvas(analyticsDisassemblyCanvas, true);

        Sequence sequence = DOTween.Sequence();
        sequence.Join(chartCanvas.DOFade(0, duration).SetEase(easeType));
        sequence.Join(summaryCanvas.DOFade(0, duration).SetEase(easeType));
        sequence.Join(analyticsAssemblyCanvas.DOFade(0, duration).SetEase(easeType));
        sequence.Join(analyticsDisassemblyCanvas.DOFade(1f, duration).SetEase(easeType));
    }

    public void OnSelectMode(GameType gameType)
    {
        string cameraName = "";
        SceneLoaderManager.Instance.currentGameType = gameType;
        switch (gameType)
        {
           
            case GameType.tutorial:
                cameraName = "tutorial";
                modeTxt.text = "Tutorial Mode";
                break;
            case GameType.practice:
                cameraName = "practice";
                modeTxt.text = "Practice Mode";
                break;
            case GameType.asessment:
                cameraName = "assesment";
                modeTxt.text = "Assesment Mode";
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

    string FormatSecondsToMinutesSeconds(float value)// need to be refactor
    {
        int totalSeconds = Mathf.FloorToInt(value);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        return $"{minutes:D2}:{seconds:D2}";
    }

    string AnalyzeErrorTrend(List<int> errors)
    {
         improving = 0;
         gettingWorse = 0;
         same = 0;

        if (errors.Count == 1)
        {
            return "Not enough data to show";
        }

        bool allZero = errors.All(e => e == 0); // Check if all past records are zero

        if (allZero)
            return "Consistently perfect!";


        for (int i = 1; i < errors.Count; i++)
        {
            if (errors[i] < errors[i - 1])
                improving++;
            else if (errors[i] > errors[i - 1])
                gettingWorse++;
            else
                same++;
        }

        if (improving > gettingWorse && improving > same)
            return "Improving.";
        else if (gettingWorse > improving && gettingWorse > same)
            return "Need more practice.";
        else /*if (same > improving && same > gettingWorse)*/
            return "Need more improvement.";

    }

    private Color GetStatusColor(string status)
    {
        Color selectedColor = Color.white;
        switch (status)
        {
            case "Improving.":
                selectedColor = improvingColor;
                break;
            case "Need more practice.":
                selectedColor = needImprovementColor;
                break;
            case "Need more improvement.":
                selectedColor = needImprovementColor;
                break;
            case "Consistently perfect!":
                selectedColor = improvingColor;
                break;

        }

        return selectedColor;
    }

    private string CheckComponentName(string givenName)
    {
       

        string selectedName = "";
        switch (givenName)
        {
            case "hdd":
                selectedName = "Hard Disk Drive";
                break;
            case "hddSata":
                selectedName = "HDD SATA Cable";
                break;
            case "hddSata2":
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
            case "opticalDrive":
                selectedName = "Optical Disk Drive";
                break;
            case "opticSata":
                selectedName = "ODD SATA Cable";
                break;
            case "opticSata2":
                selectedName = "ODD SATA Cable";
                break;
            case "powersupply":
                selectedName = "Power Supply";
                break;
            case "powerSupply":
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
            case "hddPin":
                selectedName = "SATA Power Connector(HDD)";
                break;
            case "powerOpticCable":
                selectedName = "SATA Power Connector(ODD)";
                break;
            case "opticDiskPin":
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
            case "cpuFan":
                selectedName = "CPU Fan";
                break;
            default:
                selectedName = givenName;
                break;


        }

        return selectedName;

      
    }


   

}
