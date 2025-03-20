using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    private Dictionary<string, string> placementDependencies = new Dictionary<string, string>
    {
        {"cpuFan","cpu" }
    };

    private HashSet<string> placedComponents = new HashSet<string>();
    [SerializeField] private List<string> triggerName;
    //[SerializeField] private Vector3 correctPartsRotation;
    [SerializeField] private Transform correctPartsRotation;
    [SerializeField] private float allowedTolerance;
    [SerializeField] private List<Transform> correctParts;
    
    public List<string> TriggerName { get => triggerName; }
   
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.ResetCamera();
        }
    }

    public bool IsNameOnTriggerList(string name)
    {
        return triggerName.Contains(name);
    }

    private Transform GetCorrectPartsByName(Transform givenPartName)
    {
        var currentPartName = givenPartName.gameObject.GetComponent<PcPart>();
        if (currentPartName != null)
        {
            for (int i = 0; i < correctParts.Count; i++)
            {
                var namePart = correctParts[i].gameObject.GetComponent<NameController>().partsName;
                if (namePart == currentPartName.partsName)
                {
                    return correctParts[i].transform;
                }
            }
        }

        Debug.Log($"No parts found");
        return null;
    }

    public bool IsRotationCorrect(Transform givenPartRotation)
    {
        var currentPart =  GetCorrectPartsByName(givenPartRotation);
        float rotationDifference = Quaternion.Angle(givenPartRotation.rotation, currentPart.rotation);
        return rotationDifference <= allowedTolerance;
    }

    private void OnMouseDown()
    {
        //Debug.Log(triggerName);
        //GameManager.Instance.SwitchCamera(triggerName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("draggable"))
        {
            var comp = other.gameObject.GetComponent<NameController>();
            var drag = comp.GetComponent<DragAndDrop>();
            LocationTriggerManager.Instance.CurrentLocationTrigger = this;

            if (!drag.isDragging && IsNameOnTriggerList(comp.partsName))
            {
                string partName = comp.partsName;
                if (PCComponentManager.Instance.placementDependencies.TryGetValue(partName,out string requiredComponent))
                {
                    Debug.Log($"requireComponents{requiredComponent}");
                    Debug.Log($"place components{placedComponents}");
                    if (PCComponentManager.Instance.placedComponents.Contains(requiredComponent))
                    {
                        PlaceComponent(partName,drag,comp);
                        TutorialManager.Instance.CompleteTutorialSteps();
                    }
                    else
                    {
                        GiveFeedback(drag, false);
                        
                        Debug.Log("need to place first the require parts");
                    }
                }
                else
                {
                    PlaceComponent(partName, drag, comp);
                   
                    // GiveFeedback(drag, false);
                    Debug.Log("the place component doesnt have requirement");
                    TutorialManager.Instance.CompleteTutorialSteps();
                }
            }
            else
            {
                drag.ResetPosition(true);
                UIManager.Instance.WrongFeedback();
                Debug.Log("not in trigger list");
                StatisticsManager.Instance.CountPlaceMistake();
            }
            #region
            //if (!drag.isDragging && comp.partsName == triggerName)
            //{
            //    drag.ResetPosition();
            //    GameManager.Instance.SwitchCamera(comp.partsName);
            //    PCComponentManager.Instance.GetParts(comp.partsName);
            //    UIManager.Instance.ShowControlPanelUI(true);
            //}
            //else
            //{
            //    drag.ResetPosition(true);
            //    UIManager.Instance.ShowControlPanelUI(false);
            //    UIManager.Instance.WrongFeedback();
            //}
            #endregion

        }
    }

    private void PlaceComponent(string partName,DragAndDrop drag,NameController comp)
    {
        PCComponentManager.Instance.placedComponents.Add(partName);
        drag.ResetPosition();
        GameManager.Instance.SwitchCamera(comp.partsName);
        PCComponentManager.Instance.GetParts(comp.partsName);
        UIManager.Instance.ShowControlPanelUI(true);
        //UIManager.Instance.OnHideChoicesPanel();
    }

    private void GiveFeedback(DragAndDrop drag,bool isCorrect)
    {
        drag.ResetPosition(true);
        UIManager.Instance.ShowControlPanelUI(false);
        if (!isCorrect)
        {
            UIManager.Instance.WrongFeedback();
        }

        Debug.Log("Resetting Position");
    }




}

//////
///player drag the pc parts
///player drop the pc parts to location
///locatin trigger checks the part if the name is  the same of trigger name
///if the parts name is the same the trigger will check if the quest with this id have dependency quest\
///if there is a dependency quest check if that dependency quest is completed
///if it is completed location trigger will allow to place that pc parts
///if not completed a promp will appear telling to finish that quest first
///
