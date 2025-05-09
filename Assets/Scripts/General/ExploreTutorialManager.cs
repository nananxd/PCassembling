using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class ExploreTutorial
{
    public string tutorialId;
    public bool isCompletedByButton;
    [TextArea(5, 2)]
    public string tutorialName;
    [Space(2)]
    public UnityEvent eventToEnable, eventToDisable;
    public bool isDone;
}


[System.Serializable]
public class ExploreSteps
{

}
public class ExploreTutorialManager : MonoBehaviour
{
    public static ExploreTutorialManager Instance;
    public List<ExploreTutorial> tutorials;
    [SerializeField] private ExploreTutorial currentTutorial;
    public int currentIndex;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetInitialTutorial();
    }

    public void SetInitialTutorial()
    {
        currentTutorial = tutorials[0];
        //DisableOtheComponents();
        currentTutorial.eventToEnable?.Invoke();
    }

    private void NexTutorial()
    {
        if (currentIndex + 1 >= tutorials.Count)
        {
            Debug.LogWarning("No more tutorials available.");
            return;
        }
        currentTutorial.eventToDisable?.Invoke();

        //foreach (var item in currentTutorial.steps)
        //{
        //    item.eventToDisable?.Invoke();
        //}

        currentIndex++;
        currentTutorial = tutorials[currentIndex];
        currentTutorial.eventToEnable?.Invoke();
        //DisableOtheComponents();

        //currentTutorial.imediateEvent?.Invoke();
        //if (currentTutorial.hasDelay)
        //{
        //    StartCoroutine(EventToEnableCoroutine());
        //}
        //else
        //{
            
        //}

        // 

    }

    public void CompleteTutorialSteps(string stepsID = "")
    {
        //if (SceneLoaderManager.Instance.currentGameType == GameType.asessment || SceneLoaderManager.Instance.currentGameType == GameType.practice)
        //{
        //    return;
        //}

        if (currentTutorial.isCompletedByButton)
        {
            currentTutorial.isDone = true;
            //StartCoroutine(NexTutorialCoroutine());
            currentTutorial.eventToDisable?.Invoke();
            NexTutorial();
            return;
        }

        //var foundStep = currentTutorial.steps.Find(step => step.step == currentSelectedInteractivity.id);

        //foreach (var item in currentTutorial.steps)
        //{
        //    item.eventToDisable?.Invoke();
        //}


        //if (foundStep != null)
        //{
        //    foundStep.isComplete = true;
        //    Debug.Log($"current tutorial :{currentTutorial.tutorialId},steps {foundStep.step},completed");
        //    foundStep.eventToEnable?.Invoke();
        //    CompleteTutorial();
        //}


    }


}
