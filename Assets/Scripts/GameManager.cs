using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public enum AssesmentType
{

    assemble,
    disassemble
}

public enum GameType
{
    none,
    tutorial,
    practice,
    asessment
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameType selectedGameType;
    public AssesmentType selectedAssesmentType;
    public ComponentGroup currentSelectedGroup;
    [SerializeField] private string currentMainCamera;
    [SerializeField] private List<CinemachineVirtualCamera> virtualCameras;//camera for each parts
    [SerializeField] private List<CinemachineVirtualCamera> mainVirtualCameras;
    [SerializeField] private CinemachineBrain brainCamera;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private bool isMotherboardInstalled;
    private CinemachineVirtualCamera currentCam, prevCam;

    public List<ComponentError> temComponentError;

    public string CurrentMainCamera { get => currentMainCamera; set => currentMainCamera = value; }
    public bool IsMotherboardInstalled { get => isMotherboardInstalled; set => isMotherboardInstalled = value; }
    public SaveManager SaveMan { get => saveManager; set => saveManager = value; }

    private void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        //SetupGamePlay();
        SceneLoaderManager.Instance.LoadLevel("LoadingScene", true);
        if (SceneLoaderManager.Instance.currentGameType == GameType.asessment || SceneLoaderManager.Instance.currentGameType == GameType.practice)
        {
            SoundManager.Instance.PlayBGM("BG");
        }

        if (SceneLoaderManager.Instance.currentGameType == GameType.asessment)
        {
            UIManager.Instance.SetNotificationInfoUI("Complete the assesment within 10 minutes");
            UIManager.Instance.ShowNotification();
        }
    }

    void Update()
    {
        SetupQuiz();
    }

    public void SetupQuiz()
    {
        var quizMan = QuizManager.Instance;
        if (!quizMan.CanStartQuiz)
        {
            return;
        }

        quizMan.CurrentWaiTime += Time.deltaTime;

        if (quizMan.CurrentWaiTime >= quizMan.WaitTime)
        {
            quizMan.SetQuiz();
            quizMan.CurrentWaiTime = 0;
        }

    }

    public void Save()
    {
        if (SceneLoaderManager.Instance.currentGameType == GameType.asessment)
        {
            var statisticMan = StatisticsManager.Instance;
            saveManager.SaveDataResult(SceneLoaderManager.Instance.currentAssesmentType.ToString(),statisticMan.Timer,statisticMan.mistakeCounter,statisticMan.correctAnswersCounter, saveManager.OverallValue(statisticMan.mistakeCounter, statisticMan.Timer));
            //StartCoroutine(SaveErrorComponentCoroutine());
            Debug.Log("Save Data");
        }
        else
        {
            Debug.Log("Game type is not assesment");
        }
    }

    public void SetupGamePlay()
    {
        switch (SceneLoaderManager.Instance.currentGameType)
        {
            case GameType.none:
                break;
            case GameType.tutorial:
                Debug.Log("Tutorial");
                break;
            case GameType.practice:
                Debug.Log("Practice");
                break;
            case GameType.asessment:
                StartTimer();
                Debug.Log("Assesment");
                break;
           
        }
    }
    public void SwitchCamera(string triggerName)
    {
        currentCam = virtualCameras.Find(x=> x.name == triggerName);
        currentCam.Priority = 11;
        //currentMainCamera = currentCam.name;
        for (int i = 0; i < virtualCameras.Count; i++)
        {
            if (virtualCameras[i].name != triggerName)
            {
                virtualCameras[i].Priority = 10;
            }
        }
    }

    public void ResetCamera()
    {
        currentCam.Priority = 10;
        //var mainCam = virtualCameras.Find(x => x.name == "motherBoardVirtualCam");
        var mainCam = virtualCameras.Find(x => x.name == currentMainCamera);
        mainCam.Priority = 11;
        UIManager.Instance.ShowControlPanelUI(false);
    }

    public RaycastHit CastRay()
    {

        float mousePosX = Input.mousePosition.x;
        float mousePosY = Input.mousePosition.y;
        Camera cam = Camera.main;
        Vector3 screenMousePosFar = new Vector3(mousePosX, mousePosY, cam.farClipPlane);
        Vector3 screeMousePosNear = new Vector3(mousePosX, mousePosY, cam.nearClipPlane);

        Vector3 screenWorldMouseFar = cam.ScreenToWorldPoint(screenMousePosFar);
        Vector3 screenWorldMouseNear = cam.ScreenToWorldPoint(screeMousePosNear);

        RaycastHit hit;

        Physics.Raycast(screenWorldMouseNear, (screenWorldMouseFar - screenWorldMouseNear).normalized, out hit);
        // Debug.LogError($"CastRay{hit.transform.name}");
        Debug.Log($"Mouse Position in Build: {Input.mousePosition}");
        // UIManager.Instance.mousePositionText.text = Input.mousePosition.ToString();
        return hit;
    }

    public RaycastHit CastRay(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        return hit;
    }

    public void SetComponentGroup()
    {
        switch (currentSelectedGroup)
        {
            case ComponentGroup.motherboard:
                currentSelectedGroup = ComponentGroup.external;
                break;
            case ComponentGroup.external:
                currentSelectedGroup = ComponentGroup.cables;
                break;
            case ComponentGroup.cables:
                Debug.Log("All groups completed.");
                break;
          
        }
    }

    public void StartTimer()
    {
        if (SceneLoaderManager.Instance.currentGameType == GameType.asessment)
        {
            StatisticsManager.Instance.IsCountdownStart = true;
            QuizManager.Instance.CanStartQuiz = true;
        }
       
    }

    public void AddComponentError(string compName,int count)
    {
        var foundComp = temComponentError.Find(x => x.componentName == compName);
        if (foundComp != null)
        {
            foundComp.errorCount += count;
        }
        else
        {
            //temComponentError.Add(new ComponentError(compName,count));
            ComponentError errorSave = new ComponentError
            {
                componentName = compName,
                errorCount = count
            };

            temComponentError.Add(errorSave);
        }
    }

    public IEnumerator SaveErrorComponentCoroutine()
    {
        yield return new WaitForSeconds(2f);
        SaveAssemblyErrorComp();
    }
    public void SaveAssemblyErrorComp()
    {
        if (SceneLoaderManager.Instance.currentGameType == GameType.asessment)
        {
            Debug.Log("Saving component error");
            for (int i = 0; i < temComponentError.Count; i++)
            {
                var comp = temComponentError[i];
                saveManager.SaveErrorList(comp.componentName, comp.errorCount);
            }
        }
        

        //temComponentError.Clear();
    }
}
