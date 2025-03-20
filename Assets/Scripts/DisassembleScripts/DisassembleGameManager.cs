using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Disassemble;

public enum GroupParent
{
    cpu,
    powersupply,
    hdd,
    opticdisk,
    ram,
    motherboard
}
public class DisassembleGameManager : MonoBehaviour
{
    public static DisassembleGameManager Instance;
    public DisassembleParts currentDisassemblePart;
    [SerializeField] private List<DisassembleOutlineObject> disassebmleOutlineObjects;
    [SerializeField] private List<DisassembleInteractionParent> componentGroupParents;
    [SerializeField] private DisassembleInteractionParent currentSelectedGroupParent;
    [SerializeField] private List<DisassembleInteraction> disassembleInteractions;

    [Header("Transform to follow")]
    [SerializeField] private Transform upTransform, rightTransform, downTransform;
    [SerializeField] private List<Transform> correctPositionsTransform;
    [SerializeField] private List<Transform> correctRightPosition;

    [Header("Screws transform to follow")]
    [SerializeField] private Transform screwsUpTrans;

    [SerializeField] private DisassembleInteraction currentSelectedInteraction;
    [SerializeField] private BaseInteractivity interactivity;// trigger in start

    public ComponentGroup currentSelectedGroup;
    [SerializeField] private SaveManager saveManager;

    [Header("PC Case position")]
    public GameObject pcCase;
    public Transform pcCaseCorrectPos;
    public Transform pcCaseParent, originalParent;

    public DisassembleInteractionParent CurrentSelectedGroupParent { get => currentSelectedGroupParent; set => currentSelectedGroupParent = value; }
    public Transform UpTransform { get => upTransform; set => upTransform = value; }
    public Transform RightTransform { get => rightTransform; set => rightTransform = value; }
    public Transform DownTransform { get => downTransform; set => downTransform = value; }
    public Transform ScrewsUpTrans { get => screwsUpTrans; set => screwsUpTrans = value; }
    public DisassembleInteraction CurrentSelectedInteraction { get => currentSelectedInteraction; set => currentSelectedInteraction = value; }
    public List<DisassembleInteraction> DisassembleInteractions { get => disassembleInteractions; set => disassembleInteractions = value; }

    public Dictionary<string, List<string>> partsDependency = new Dictionary<string, List<string>>
    {
        {"cpu",new List<string>{"fan"} },
        {"powersupply", new List<string> { "4x4pin", "24pin", "powerHddCable","powerOpticCable","powerSupplyScrews"}},
        {"hdd", new List<string>{"hddSata","hddScrews","powerHddCable"}},
        {"opticdisk",new List<string>{"opticSata","opticScrews","powerOpticCable"}},
        //{"motherboard",new List<string>{ "cpu", "fan","ram1","ram2","4x4pin","24pin","motherboardScrews"}}
        {"motherboard",new List<string>{ "4x4pin","24pin","motherboardScrews","hddSata","opticSata","powerHddCable","powerOpticCable","frontPanel"}}
    };

    public HashSet<string> removePart = new HashSet<string>();

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        SceneLoaderManager.Instance.LoadLevel("LoadingScene", true);
       
        currentDisassemblePart = DisassembleParts.cables;      
        SetupGamePlay();
        StartCoroutine(StartTutorialCoroutine());


    }

    private void Update()
    {
      
        SetupQuiz();
    }

    #region Set parent of cpu,ram and fan

    public void SetParentToOriginal()
    {
        var cpu = disassebmleOutlineObjects.Find(cpu => cpu.outlineName == "cpu");
        var fan = disassebmleOutlineObjects.Find(cpu => cpu.outlineName == "fan");
        var ram1 = disassebmleOutlineObjects.Find(cpu => cpu.outlineName == "ram1");
        var ram2 = disassebmleOutlineObjects.Find(cpu => cpu.outlineName == "ram2");

        cpu.gameObject.transform.SetParent(originalParent);
        fan.gameObject.transform.SetParent(originalParent);
        ram1.gameObject.transform.SetParent(originalParent);
        ram2.gameObject.transform.SetParent(originalParent);


    }

    #endregion

    #region Tutorial Related
    private IEnumerator StartTutorialCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        //TutorialManager.Instance.CurrentSelectedInteractivity = interactivity;
        //TutorialManager.Instance.CompleteTutorialSteps();
        DisassembleTutorialManager.Instance.CompleteTutorialSteps("start");
    }

    public void EnableMethod()
    {
        Debug.Log("Start tutorial");
    }

    public void DisassembleInteractionEnableByName(string partName)
    {
        var foundInteraction = disassembleInteractions.Find(x => x.partsName == partName);
        foundInteraction.enabled = true;
    }

    public void AllDisassembleInteractionEnableOrDisable(bool isEnable)
    {
        foreach (var parent in componentGroupParents)
        {
            foreach (var interaction in parent.InteractionGroups)
            {
                interaction.enabled = isEnable;
            }
        }
    }

    #endregion

    #region Diassemble outline objects

    public void SetOutlineObjects()
    {
        var sceneLoader = SceneLoaderManager.Instance;
        if (sceneLoader == null)
        {
            Debug.Log($"scene loader manager is null");
            return;
        }

        if (sceneLoader.currentGameType == GameType.tutorial)
        {
            SetCurrentDisassembleOutlineObject();
        }
        else
        {
            Debug.Log($"Game type:{sceneLoader.currentGameType} is not equal to tutorial");
            return;
        }

        // SetCurrentDisassembleOutlineObject();


    }

    public void SetCurrentDisassembleOutlineObject()
    {
        if (FindAllCompleteOutlineObjects(DisassembleParts.cables))
        {
            currentDisassemblePart = DisassembleParts.screws;
            SetOutlineObjectToDisable();
            SetOutlineObjectToEnable();
            Debug.Log("DONE cables");
        }

        if (FindAllCompleteOutlineObjects(DisassembleParts.screws))
        {
            currentDisassemblePart = DisassembleParts.other;
            SetOutlineObjectToDisable();
            SetOutlineObjectToEnable();
            Debug.Log("DONE screws");
        }

        if (FindAllCompleteOutlineObjects(DisassembleParts.other))
        {
            currentDisassemblePart = DisassembleParts.none;
            SetOutlineObjectToDisable();
            SetOutlineObjectToEnable();
            Debug.Log("DONE other");
        }
    }
    public void SetOutlineObjectToDisable()
    {
        var filteredOutlineObjects = GetOutLineObjectToDisable(currentDisassemblePart);
        DisableOutlineObjects(filteredOutlineObjects);
        Debug.Log($"Disable filter outline objects count:{filteredOutlineObjects.Count} - current disassemble part: {currentDisassemblePart}");
              
    }

    public void SetOutlineObjectToEnable()
    {
        var filteredOutlineObjects = GetOutlineObjectToEnable(currentDisassemblePart);
        EnableOutlineObjects(filteredOutlineObjects);
        Debug.Log($"Enable filter outline objects count:{filteredOutlineObjects.Count} - current disassemble part: {currentDisassemblePart}");

    }

    private void DisableOutlineObjects(List<DisassembleOutlineObject> objectsOutline)
    {
        foreach (var item in objectsOutline)
        {
            item.DisableOutline();
        }
    }

    public void EnableOutlineObjects(List<DisassembleOutlineObject> objectsOutline)
    {
        foreach (var item in objectsOutline)
        {
            item.EnableOutline();
        }
    }
    public List<DisassembleOutlineObject> GetOutLineObjectToDisable(DisassembleParts parts)
    {
        var filteredObjects = disassebmleOutlineObjects.FindAll(x=> x.disassemblePart != parts);
        if (filteredObjects == null)
        {
            return null;
        }
         return filteredObjects;
                
    }

    public List<DisassembleOutlineObject> GetOutlineObjectToEnable(DisassembleParts parts)
    {
        var filteredObjects = disassebmleOutlineObjects.FindAll(x => x.disassemblePart == parts);
        if (filteredObjects == null)
        {
            return null;
        }
        return filteredObjects;

    }

    public bool FindAllCompleteOutlineObjects(DisassembleParts parts)
    {
        var filteredObjects = disassebmleOutlineObjects.FindAll(x => x.disassemblePart == parts);
        return filteredObjects.All(x => x.isDone);
    }

    public void DisableOutlineObject(DisassembleOutlineObject outlineObject)
    {
        var filterObject = disassebmleOutlineObjects.Find(x=> x == outlineObject);
        filterObject.DisableOutline();
        filterObject.isDone = true;
    }

    public void EnableOutlineObject(DisassembleOutlineObject outlineObject)
    {
        var filterObject = disassebmleOutlineObjects.Find(x => x == outlineObject);
        filterObject.EnableOutline();
        //filterObject.isDone = true;
    }

    public void DisableOutlineObjectByName(string outlineName)
    {
        var filterObject = disassebmleOutlineObjects.Find(x => x.outlineName == outlineName);
        filterObject.DisableOutline();
    }

    public void EnableOutlineObjectByName(string outlineName)
    {
        var filterObject = disassebmleOutlineObjects.Find(x => x.outlineName == outlineName);
        filterObject.EnableOutline();
    }

    public void EnableOrDisableOutline(bool isEnable)
    {
        if (isEnable)
        {
            EnableOutlineObjects(disassebmleOutlineObjects);
        }
        else
        {
            DisableOutlineObjects(disassebmleOutlineObjects);
        }
    }
       
    #endregion
    public void Save()
    {
        if (SceneLoaderManager.Instance.currentGameType == GameType.asessment)
        {
            var statisticMan = StatisticsManager.Instance;
            saveManager.SaveDataResult(SceneLoaderManager.Instance.currentAssesmentType.ToString(), statisticMan.Timer, statisticMan.mistakeCounter, statisticMan.correctAnswersCounter, saveManager.OverallValue(statisticMan.mistakeCounter));
            Debug.Log("Save Data");
        }
        else
        {
            Debug.Log("Game type is not assesment");
        }
    }

    #region Correct Position of Component Parts Transform
   
    public void SetDownTransform(Transform selectedTransform)
    {
        downTransform = selectedTransform;
    }

    public void SetScrewUpTransform(Transform selectedTransform)
    {
        ScrewsUpTrans = selectedTransform;
    }

    public void SetRightTransform(Transform selectedTransform)
    {
        rightTransform = selectedTransform;
    }

    public Transform GetRightTransform(string partName)
    {
        var selectedTransform = correctRightPosition.Find(e => e.gameObject.GetComponent<NameController>().partsName == partName);
        if (selectedTransform != null)
        {
            return selectedTransform;
        }
        Debug.Log($"No transform found with name:{partName}");
        return null;
    }

    public Transform GetDownTransform(string partName)
    {
        var selectedTransform = correctPositionsTransform.Find(e => e.gameObject.GetComponent<NameController>().partsName == partName);
        if (selectedTransform != null)
        {
            return selectedTransform;
        }
        Debug.Log($"No transform found with name:{partName}");
        return null;
    }

    #endregion

    public DisassembleInteractionParent CheckParent(DisassembleInteraction currentInteraction)
    {
        var selectedParent =  componentGroupParents.Find(parent => parent.group.ToString() == currentInteraction.groupParent);
        return selectedParent;
    }

    public void HideComponentPart(DisassembleInteraction currentInteraction)
    {
        
    }

    public bool CheckIfPartRemove(string partName)
    {
        if (!partsDependency.ContainsKey(partName))
            return true; // No dependencies, can remove

        foreach (var depedency in partsDependency[partName])
        {
            if (!removePart.Contains(depedency))
            {
                return false;
            }

        }

        return true;
    }

    public void AddToRemovePart(string partName)
    {
        if (!removePart.Contains(partName))
        {
            removePart.Add(partName);
        }
        
    }

    public void CheckIfGameFinished(string componentPartName,string partName)
    {
        QuestManager.Instance.CompleteStep(componentPartName, partName);
        QuestManager.Instance.CheckGroupIfComplete(currentSelectedGroup,Disassemble.UIManager.Instance);
        QuestManager.Instance.CheckIfQuestCompleted(componentPartName);

        if(QuestManager.Instance.IsAllQuestFinished())
        {
            StatisticsManager.Instance.IsTimerStart = false;
            StatisticsManager.Instance.CanTimerStart = false;
            Disassemble.UIManager.Instance.StartCoroutine(Disassemble.UIManager.Instance.SetStatisticCoroutine());
            QuizManager.Instance.CanStartQuiz = false;
            Save();
        }
    }

    #region Quiz
    public void SetupQuiz()
    {
        var quizMan = QuizManager.Instance;
        var sceneLoader = SceneLoaderManager.Instance;
        if (!quizMan.CanStartQuiz)
        {
            return;
        }

        quizMan.CurrentWaiTime += Time.deltaTime;

        if (quizMan.CurrentWaiTime >= quizMan.WaitTime)
        {
            quizMan.SetQuiz(Disassemble.UIManager.Instance);
            quizMan.CurrentWaiTime = 0;
        }

    }
    #endregion

    public void StartTimer()
    {
        if (SceneLoaderManager.Instance.currentGameType == GameType.asessment)
        {
            StatisticsManager.Instance.IsCountdownStart = true;
            QuizManager.Instance.CanStartQuiz = true;
        }
       
    }

    

    public void SetupGamePlay()
    {
        DisableOutlineObjects(disassebmleOutlineObjects);
        switch (SceneLoaderManager.Instance.currentGameType)
        {
            case GameType.none:
                break;
            case GameType.tutorial:
                
                //SetOutlineObjectToEnable();// enable guides
                // disable timer,disable save,disable quiz
                Debug.Log("Tutorial");
                break;
            case GameType.practice:
                Debug.Log("Practice");
                break;
            case GameType.asessment:
                StatisticsManager.Instance.IsTimerStart = true;
                StartTimer();
                Debug.Log("Assesment");
                break;
            default:
                break;
        }
    }


}
