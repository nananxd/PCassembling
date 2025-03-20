using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class ManagerUI : MonoBehaviour
{
    public static ManagerUI Instance;
    [Header("Dotween settings")]
    [SerializeField] private float duration;
    [SerializeField] private Ease easeType;

    [Header("Model control ui")]
    [SerializeField] private CanvasGroup modelControlCanvasGroup;
    [SerializeField] private Button leftButton, rightButton;

    [Header("Inventory ui")]
    [SerializeField] private RectTransform inventParent;
    [SerializeField] private GameObject inventPrefab;
    [SerializeField] private CanvasGroup inventoryCanvasGroup;

    [Header("Info UI")]
    [SerializeField] private CanvasGroup infoCanvasGroup;
    [SerializeField] private Button infoOpenBtn, infoCloseBtn;
    [SerializeField] private TextMeshProUGUI infoText,titleText;

    [Header("Menu UI")]
    [SerializeField] private CanvasGroup menuCanvasGroup;
    [SerializeField] private RectTransform menuUI;
    [SerializeField] private Vector3 hidePosition, showPosition;
    [SerializeField] private Button menuRestartBtn, menuMainMenuBtn,openMenuBtn;
    [SerializeField] private List<CanvasGroup> menuBtnCanvasGroup;

    private string info,title;

    public Button InfoOpenBtn { get => infoOpenBtn; set => infoOpenBtn = value; }

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        SceneLoaderManager.Instance.LoadLevel("LoadingScene", true);
        Setup();
        SetupButtonOnclick();
    }

    private void Setup()
    {
        var modelSO =  ModelsManager.Instance.modelSO.models;
        for (int i = 0; i < modelSO.Count; i++)
        {
            GameObject inventGO = Instantiate(inventPrefab,inventParent.transform);
            var modelUI = inventGO.GetComponent<ModelUI>();
            modelUI.Setup(modelSO[i].modelName,modelSO[i].modelVisual);
        }

        ShowModelControl(false);
        InfoOpenBtn.onClick.AddListener(OnClickInfo);
        infoCloseBtn.onClick.AddListener(OnCloseInfo);

        leftButton.onClick.AddListener(()=> ModelsManager.Instance.RotateModel(true));
        rightButton.onClick.AddListener(() => ModelsManager.Instance.RotateModel(false));

        InfoOpenBtn.transform.localScale = Vector3.zero;
    }

    private void SetupButtonOnclick()
    {
        var sceneLoader = SceneLoaderManager.Instance;

        //restartBtn.onClick.AddListener(sceneLoader.RestartLevel);
        //mainMenuBtn.onClick.AddListener(() => sceneLoader.LoadLevelAsAsync("MainMenu"));

        menuRestartBtn.onClick.AddListener(sceneLoader.RestartLevel);
        menuMainMenuBtn.onClick.AddListener(() => sceneLoader.LoadLevelAsAsync("MainMenu"));

       // statCloseButton.onClick.AddListener(() => sceneLoader.LoadLevelAsAsync("MainMenu"));
    }

    public void SetInfoDetails(string infoDesc,string infoTitle)
    {
        info = infoDesc;
        title = infoTitle;

        infoText.text = info;
        titleText.text = title;

        Debug.Log("Setting info");
    }

    public void ShowOrHideCanvas(CanvasGroup canvas,bool isShown)
    {
        if (isShown)
        {
            canvas.DOFade(1f,duration).SetEase(easeType);
            canvas.blocksRaycasts = true;
            canvas.interactable = true;
        }
        else
        {
            canvas.DOFade(0, duration).SetEase(easeType);
            canvas.blocksRaycasts = false;
            canvas.interactable = false;
        }
    }

    public void ShowModelControl(bool isShown)
    {
        ShowOrHideCanvas(modelControlCanvasGroup, isShown);
       
           
    }

    public void OnClickInfo()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(()=> CameraControlManager.Instance.ZoomIn());
        sequence.AppendCallback(() => ShowOrHideCanvas(modelControlCanvasGroup, false));
        sequence.AppendCallback(()=> ShowOrHideCanvas(infoCanvasGroup,true));
        sequence.AppendCallback(() => ShowOrHideCanvas(inventoryCanvasGroup, false));
        sequence.Append(titleText.DOFade(1f,duration).SetEase(easeType));
        sequence.Append(infoText.DOFade(1f,duration).SetEase(easeType));
        sequence.Join(InfoOpenBtn.transform.DOScale(Vector3.zero,duration).SetEase(easeType));
    }

    public void OnCloseInfo()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() => CameraControlManager.Instance.ZoomOut());    
        sequence.AppendCallback(() => ShowOrHideCanvas(infoCanvasGroup, false));
       
        sequence.Append(titleText.DOFade(0, duration).SetEase(easeType));
        sequence.Append(infoText.DOFade(0, duration).SetEase(easeType));
        sequence.Join(InfoOpenBtn.transform.DOScale(Vector3.one, duration).SetEase(easeType));
        sequence.AppendCallback(() => ShowOrHideCanvas(modelControlCanvasGroup, true));
        sequence.AppendCallback(() => ShowOrHideCanvas(inventoryCanvasGroup, true));
    }

    public void OnClickMenu()
    {
        Sequence sequence = DOTween.Sequence();
      
        sequence.Append(menuUI.DOScaleX(1f,duration).SetEase(easeType));       
        sequence.Join(menuUI.DOScaleY(.003f,duration).SetEase(easeType));
       
        sequence.AppendInterval(.1f);
        //sequence.Append(menuUI.DOAnchorPos(showPosition, duration).SetEase(easeType));
        sequence.Join(menuCanvasGroup.DOFade(1f, .1f).SetEase(Ease.InSine));
        sequence.Append(menuUI.DOScaleY(1f, duration).SetEase(easeType));
       
        foreach (var item in menuBtnCanvasGroup)
        {
            sequence.Append(item.DOFade(1f,.1f).SetEase(Ease.InSine));
            sequence.Join(item.transform.DOScale(Vector3.one, .1f).SetEase(Ease.InSine));
        }

      
       


        menuCanvasGroup.blocksRaycasts = true;
        menuCanvasGroup.interactable = true;
        //openMenuBtn.enabled = false;


    }

    public void OnClickResume()
    {
        Sequence sequence = DOTween.Sequence();
        //sequence.Join(menuUI.DOAnchorPos(hidePosition, duration).SetEase(easeType));
        foreach (var item in menuBtnCanvasGroup)
        {
            sequence.Join(item.DOFade(0, .1f).SetEase(easeType));
            sequence.Join(item.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), .1f).SetEase(easeType));
        }
        sequence.Append(menuUI.DOScaleY(.003f, duration).SetEase(easeType));
        
        sequence.AppendInterval(.1f);
        
        sequence.Append(menuUI.DOScaleX(0, duration).SetEase(easeType));
        sequence.Join(menuUI.DOScaleY(0, duration).SetEase(easeType));
        sequence.Join(menuCanvasGroup.DOFade(0, .3f).SetEase(Ease.OutSine));
        //sequence.AppendInterval(.1f);

       


        menuCanvasGroup.blocksRaycasts = false;
        menuCanvasGroup.interactable = false;
        //openMenuBtn.enabled = true;

    }

    public void OnClickLeft()
    {

    }

    public void OnClickRight()
    {

    }

   
}
