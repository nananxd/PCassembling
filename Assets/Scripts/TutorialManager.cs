using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Tutorial
{
    public string tutorialId;
    [TextArea(5,2)]
    public string tutorialName;
    [Space(2)]
    
   
    public List<TutorialStep> steps;
    public bool isDone; 
}

[System.Serializable]
public class TutorialStep
{
    [Header("this is just an id")]
    public string step;
    public bool isComplete;
}
public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    [SerializeField] private BaseInteractivity currentSelectedInteractivity;
    public List<Tutorial> tutorials;
    [SerializeField] private Tutorial currentTutorial;

    public List<BaseInteractivity> objectsToHide;

    public int currentIndex;

    public Tutorial CurrentTutorial { get => currentTutorial; set => currentTutorial = value; }
    public BaseInteractivity CurrentSelectedInteractivity { get => currentSelectedInteractivity; set => currentSelectedInteractivity = value; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (SceneLoaderManager.Instance.currentGameType == GameType.tutorial)
        {
            StartCoroutine(GetAllObjectsToHideCoroutine());
            SetInitialTutorial();
        }
       
    }

    private IEnumerator GetAllObjectsToHideCoroutine()
    {
        yield return new WaitForSeconds(1f);
        objectsToHide = FindObjectsOfType<BaseInteractivity>(includeInactive: true).ToList();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    NexTutorial();
        //}
    }



    public void SetInitialTutorial()
    {
        CurrentTutorial = tutorials[0];
        DisableOtheComponents();
    }

    private void NexTutorial()
    {
        if (currentIndex + 1 >= tutorials.Count)
        {
            Debug.LogWarning("No more tutorials available.");
            return;
        }
        currentIndex++;
        CurrentTutorial = tutorials[currentIndex];
        DisableOtheComponents();

    }

    private void CompleteTutorial()
    {

        if (currentTutorial.steps.All(step => step.isComplete))// check if the current tutorial list of steps mark as completed
        {
            currentTutorial.isDone = true;// set the current tutorial done or complete
            Debug.Log("Current tutorial Done");
            NexTutorial();
        }
        //else if(currentTutorial.steps.Count <= 0)
        //{
        //    currentTutorial.isDone = true;
        //    Debug.Log("Current tutorial is not finished");
        //}
    }

    public void CompleteTutorialSteps(string stepsID = "")
    {
        if (SceneLoaderManager.Instance.currentGameType == GameType.asessment || SceneLoaderManager.Instance.currentGameType == GameType.practice)
        {
            return;
        }

        var foundStep = currentTutorial.steps.Find(step => step.step == currentSelectedInteractivity.id);


        if (foundStep != null)
        {
            foundStep.isComplete = true;
            Debug.Log($"current tutorial :{currentTutorial.tutorialId},steps {foundStep.step},completed");
            CompleteTutorial();
        }

       
    }

    public void CompleteTutorialSteps(bool isSlot)
    {
        if (SceneLoaderManager.Instance.currentGameType == GameType.asessment || SceneLoaderManager.Instance.currentGameType == GameType.practice)
        {
            return;
        }


        if (isSlot)
        {
            var foundStep = currentTutorial.steps.FindIndex(step => step.step == currentSelectedInteractivity.id && !step.isComplete);
            if (foundStep != -1)
            {
                currentTutorial.steps[foundStep].isComplete = true;
                Debug.Log($"current tutorial: {currentTutorial.tutorialId}, step {currentTutorial.steps[foundStep].step} completed");
                CompleteTutorial();
            }
        }
    }

    public void DisableOtheComponents()
    {
        //var foundComponents = currentTutorial.steps.Select(step => new { TutorialStep = step,Object = objectsToHide.FirstOrDefault(i => i.id == step.step)})
        //    .Where(x =>x.Object != null)
        //    .ToList();

        var stepsHashset = currentTutorial.steps.Select(step => step.step).ToHashSet();
        objectsToHide.ForEach(interactivity => 
        {
            if (stepsHashset.Contains(interactivity.id))
            {
                interactivity.EnableObject();
            }
            else
            {
                interactivity.DisableObject();
            }
        });

       
    }

    
}
