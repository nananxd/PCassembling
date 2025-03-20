using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Disassemble;

[System.Serializable]
public class Step
{
    public string questID;
    public string questName;
    public string questStepToFinish;
    public string dependencyStepId;//reference to other step in order for this step to complete
    public bool isComplete;
}

[System.Serializable]
public class ComponentParts
{
    public ComponentGroup group;
    public string componentsName;
    public List<Step> componentSteps = new List<Step>();
    public bool isComplete;
}

[System.Serializable]
public class NotificationMessage
{
    public string groupId;
    public string notifMessage;
}
public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public AsessmentType selectedAsessment;
    public QuestListSO currentQuest;

    [Header("Notification Messages")]
    [SerializeField] private List<NotificationMessage> messages;
   
    [SerializeField] private string asessmentKeyValue;
    [SerializeField] private List<QuestListSO> questListSO;
    [SerializeField] private List<ComponentPartSteps> pcComponents;

    [SerializeField] private List<ComponentParts> pcComponentsQuest;


    [Header("For Testing")]
    public string testComponentId;
    public string testStepId;


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.S))
        {
            TestQuest();
        }

       
    }

    private void Start()
    {
        Setup();
    }

    #region Notification Message
    public string GetMessage(string id)
    {
        var selectedMessage =  messages.Find(m => m.groupId == id);
        if (selectedMessage != null)
        {
            return selectedMessage.notifMessage;
        }

        Debug.Log($"No message found with id :{id}");
        return null;
    }
    #endregion

    private void Setup()
    {
        //asessmentKeyValue =  PlayerPrefs.GetString(ConstantData.ASESSMENT_KEY);
        //currentQuest = GetQuestList(asessmentKeyValue);

        for (int i = 0; i < pcComponents.Count; i++)
        {
            ComponentParts newComponent = new ComponentParts();
            newComponent.componentsName = pcComponents[i].componentName;
            newComponent.group = pcComponents[i].group;
            for (int j = 0; j < pcComponents[i].stepsToComplete.Count; j++)
            {
                Step newStep = new Step();
                newStep.questID = pcComponents[i].stepsToComplete[j].questID;
                newStep.questName = pcComponents[i].stepsToComplete[j].questName;
                newStep.questStepToFinish = pcComponents[i].stepsToComplete[j].questStepToFinish;
                newStep.dependencyStepId = pcComponents[i].stepsToComplete[j].dependencyStepId;
                newStep.isComplete = pcComponents[i].stepsToComplete[j].isComplete;

                newComponent.componentSteps.Add(newStep);
            }

            pcComponentsQuest.Add(newComponent);
        }
    }

    public void CheckGroupIfComplete(ComponentGroup group)
    {
        var groups = pcComponentsQuest.FindAll(g => g.group == group);
        var steps = groups.All(s => s.componentSteps.All(step => step.isComplete == true)) ;
        Debug.Log($"CheckIfGroupComplete, bool steps:{steps}");

        if (steps)
        {
           
            Debug.Log($"group:{group} quest all complete");
            var message = GetMessage(group.ToString());
            UIManager.Instance.SetNotificationInfoUI(message);
            UIManager.Instance.ShowNotification();

            UIManager.Instance.SetComponentGroupComplete();
            GameManager.Instance.SetComponentGroup();
           
        }
        else
        {
            Debug.Log($"group:{group} quest not complete");
        }
    }

    public void CheckGroupIfComplete(ComponentGroup group, Disassemble.UIManager uiManager)
    {
        var groups = pcComponentsQuest.FindAll(g => g.group == group);
        var steps = groups.All(s => s.componentSteps.All(step => step.isComplete == true));
        Debug.Log($"CheckIfGroupComplete, bool steps:{steps}");

        if (steps)
        {

            Debug.Log($"group:{group} quest all complete");
            var message = GetMessage(group.ToString());
            uiManager.SetNotificationInfoUI(message);
            uiManager.ShowNotification();

            uiManager.SetComponentGroupComplete();
            //GameManager.Instance.SetComponentGroup();

        }
        else
        {
            Debug.Log($"group:{group} quest not complete");
        }
    }

   

    public bool IsAllQuestFinished()
    {
       
        return pcComponentsQuest.All(q=> q.componentSteps.All(s => s.isComplete == true));
    }

    public ComponentPartSteps GetComponentPartByName(string componentName)
    {
        var selectedComponent = pcComponents.Find(c => c.componentName == componentName);
        if (selectedComponent != null)
        {
            return selectedComponent;
        }

        Debug.Log($"No component part found");
        return null;
    }
    public QuestListSO GetQuestList(string assesType)
    {
        var curQuestList = questListSO.Find(q => q.assesmentType.ToString() == assesType);
        if (!curQuestList)
        {
            Debug.Log("No quest list found");
            return null;
        }
            return curQuestList;
    }

    public bool CheckIfQuestCompleted(string componentPartName)// All quest of component
    {
        var selectedComponent = pcComponentsQuest.Find(x => x.componentsName == componentPartName);
        if (selectedComponent == null)
        {
            Debug.LogError($"cannot find component with name{componentPartName}");
            return false;
        }
        Debug.Log("All quest completed");
        return selectedComponent.componentSteps.All(s => s.isComplete);
    }

    public bool CheckIfQuestCompletedFilter(ComponentGroup group,string componentName)// for checking motherboard quest
    {
        var selectedGroup = pcComponentsQuest.FindAll(x => x.group == group);
        //if (selectedGroup.Count == 0) return false;
        return selectedGroup.Where(compName => compName.componentsName != componentName ).All(step => step.componentSteps.All(s => s.isComplete));
    }

    public void CheckIfComponentQuestComplete(string componentName)
    {
        var selectedComponent = pcComponentsQuest.Find(x => x.componentsName == componentName);
        if (selectedComponent == null)
        {
            Debug.LogError($"cannot find component with name{componentName}");
            //return false;
        }
    }

    public void CompleteCurrentQuest(string id)
    {
        var curQuest = currentQuest.quests.Find(q => q.questID == id);
        curQuest.isComplete = true;

    }

    public void CompleteStep(string componentPartName,string stepId)
    {
        var selectedComponent = pcComponentsQuest.Find(x => x.componentsName == componentPartName);
        var selectedStep = selectedComponent.componentSteps.Find(step=> step.questName == stepId);

        if (selectedStep != null)
        {
            if (selectedStep.dependencyStepId == "N/A")
            {
                selectedStep.isComplete = true;
            }
            else
            {
                var dependencyStep = selectedComponent.componentSteps.Find(dependency => dependency.questID == selectedStep.dependencyStepId);
                if (dependencyStep.isComplete)
                {
                    selectedStep.isComplete = true;
                }
                else
                {
                    Debug.Log("Complete first the require step to finish this selected step");
                }
            }
        }
    }

    public bool GetQuestById(string componentName)
    {
        var foundQuest = pcComponentsQuest.Find(x => x.componentsName == componentName);
        return foundQuest.componentSteps.All(quest => quest.isComplete);
    }

    public void TestQuest()
    {
        CompleteStep(testComponentId,testStepId);
        CheckGroupIfComplete(GameManager.Instance.currentSelectedGroup);
        CheckIfQuestCompleted(testComponentId);
        if (GameManager.Instance.currentSelectedGroup == ComponentGroup.motherboard)
        {
            if (CheckIfQuestCompletedFilter(ComponentGroup.motherboard,"motherboard"))
            {
                UIManager.Instance.EnableInstallMotherboardUI(true);
            }
            
        }
        else
        
        Debug.Log($"TestQuest:CheckIfQuestCompleted:{CheckIfQuestCompleted(testComponentId)}");
    }
}
