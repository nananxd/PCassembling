using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace Disassemble
{
    [System.Serializable]
    public class DisassembleTutorial
    {
        public string tutorialId;
        public UnityEvent buttonEvent;
        [TextArea(5, 2)]
        public string tutorialName;
        [Space(2)]
        public UnityEvent eventToEnable;
        [Space(2)]
        public UnityEvent eventToDisable;

        public List<TutorialStep> steps;
        public bool hasDelay;
        public bool isCompletedByButton;
        public bool isAutoComplete;
        public bool isDone;
    }

    //[System.Serializable]
    //public class TutorialStep
    //{
    //    [Header("this is just an id")]
    //    public string step;
    //    public bool isComplete;
    //}  
    public class DisassembleTutorialManager : MonoBehaviour
    {
        public static DisassembleTutorialManager Instance;
        [SerializeField] private int currentIndex;
        public List<DisassembleTutorial> tutorials;

        [SerializeField] private DisassembleTutorial currentTutorial;
        public List<BaseInteractivity> objectsToHide;
        public DisassembleTutorial CurrentTutorial { get => currentTutorial; set => currentTutorial = value; }

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            if (SceneLoaderManager.Instance.currentGameType == GameType.tutorial )
            {
                SetInitialTutorial();
            }
                
        }

        public void CompleteTutorialSteps(string stepsID = "")
        {
            if (SceneLoaderManager.Instance.currentGameType == GameType.asessment || SceneLoaderManager.Instance.currentGameType == GameType.practice)
            {
                return;
            }

            if (currentTutorial.isCompletedByButton)
            {
                currentTutorial.isDone = true;
                //StartCoroutine(NexTutorialCoroutine());
                NexTutorial();
                return;
            }

            if (currentTutorial.isAutoComplete)
            {
                //currentTutorial.isDone = true;
                //CompleteTutorial(true);
                StartCoroutine(CompleteTutorialCoroutine(true));
                return;
            }

            //var foundStep = currentTutorial.steps.Find(step => step.step == stepsID);
            var foundStep = currentTutorial.steps.FindIndex(step => step.step == stepsID &&  !step.isComplete);

            foreach (var item in currentTutorial.steps)
            {
                item.eventToDisable?.Invoke();
            }


            //if (foundStep != null)
            //{
            //    foundStep.isComplete = true;
            //    Debug.Log($"current tutorial :{currentTutorial.tutorialId},steps {foundStep.step},completed");
            //    foundStep.eventToEnable?.Invoke();
            //    CompleteTutorial();
            //}
            //else
            //{
            //    Debug.Log($"cannot found step:{stepsID},in the current tutorial");
            //}


            if (foundStep != -1)
            {
                currentTutorial.steps[foundStep].isComplete = true;
                Debug.Log($"current tutorial: {currentTutorial.tutorialId}, step {currentTutorial.steps[foundStep].step} completed");
                currentTutorial.steps[foundStep].eventToEnable?.Invoke();
                CompleteTutorial();
            }


        }

        private void CompleteTutorial(bool isAutoComplete = false)
        {
            if (isAutoComplete)
            {
                currentTutorial.isDone = true;
                NexTutorial();
            }
            else
            {
                if (currentTutorial.steps.All(step => step.isComplete))// check if the current tutorial list of steps mark as completed
                {
                    currentTutorial.isDone = true;// set the current tutorial done or complete
                    Debug.Log("Current tutorial Done");
                    NexTutorial();
                }
            }
            
        }

        private IEnumerator CompleteTutorialCoroutine(bool isAutoComplete)
        {
            yield return new WaitForSeconds(1.5f);
            CompleteTutorial(isAutoComplete);
        }

        private void NexTutorial()
        {
            if (currentIndex + 1 >= tutorials.Count)
            {
                Debug.LogWarning("No more tutorials available.");
                return;
            }
            currentTutorial.eventToDisable?.Invoke();// execute the event to disable  of current tutorial  before moving to the next tutorial

            currentIndex++;
            currentTutorial = tutorials[currentIndex];
            DisableOtheComponents();// disable all object that is not related to tutorial
            //currentTutorial.eventToEnable?.Invoke();//enable all objects related to tutorial
           

            if (currentTutorial.hasDelay)
            {
                StartCoroutine(DelayEventToEnableCoroutine());
            }
            else
            {
                //currentTutorial.eventToEnable?.Invoke();
                StartCoroutine(EventEnableCoroutine());
            }

        }
        private IEnumerator DelayEventToEnableCoroutine()
        {
            yield return new WaitForSeconds(6f);
            currentTutorial.eventToEnable?.Invoke();
        }
        private IEnumerator EventEnableCoroutine()
        {
            yield return new WaitForSeconds(.5f);
            currentTutorial.eventToEnable?.Invoke();
        }

        public void SetInitialTutorial()
        {
            currentTutorial = tutorials[0];
            DisableOtheComponents();
            currentTutorial.eventToEnable?.Invoke();
        }
        public void DisableOtheComponents()
        {
            //var stepsHashset = currentTutorial.steps.Select(step => step.step).ToHashSet();
            //objectsToHide.ForEach(interactivity =>
            //{
            //    if (stepsHashset.Contains(interactivity.id))
            //    {
            //        interactivity.EnableObject();
            //    }
            //    else
            //    {
            //        interactivity.DisableObject();
            //    }
            //});

            objectsToHide.ForEach(interactivity => {
                interactivity.DisableObject();
            });
        }

        public bool IsInteractivityInCurrentQuest(string interactivityName)
        {
            var stepsHashset = currentTutorial.steps.Select(step => step.step).ToHashSet();

            if (stepsHashset.Contains(interactivityName))
            {
                return true;
            }

            return false;
        }

    }
}

