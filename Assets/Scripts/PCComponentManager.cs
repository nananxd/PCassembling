using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PCComponentManager : MonoBehaviour
{
    public static PCComponentManager Instance;

    public Dictionary<string, string> placementDependencies = new Dictionary<string, string>
    {
        {"thermalPaste","cpu" },
        {"cpuFan","thermalPaste" }
    };

    public HashSet<string> placedComponents = new HashSet<string>();
    public PcPart CurrentSelectedPart { get => currentSelectedParts; set => currentSelectedParts = value; }
    [Header("Optic Disk Armature and Cable")]
    [SerializeField] private GameObject opticArmature;
    [SerializeField] private GameObject opticCable;
    [SerializeField] private Transform opticParent1, opticParent2;

    [Header("Hdd Armature and Cable")]
    [SerializeField] private GameObject hddArmature;
    [SerializeField] private GameObject hddCable;
    [SerializeField] private Transform hddParent1, hddParent2;

    [Header("Controllable Parts")]
    [SerializeField] private List<PcPart> pcParts;
    [SerializeField] private List<Transform> componentPositions;
    [Header("Correct  Parts")]
    [SerializeField] private List<GameObject> correctPositionParts;
    [SerializeField] private PcPart currentSelectedParts;

    [Header("Component Slots")]
    [SerializeField] private List<ComponentSlot> componentSlots;

    [Header("Outline Object")]
    [SerializeField] private List<OutlineObject> outlineObjects;

    [Header("Interactable component parts")]
    [SerializeField] private List<InteractablePart> interactableParts;
    [SerializeField] private InteractablePart currentSelectedInteractablePart;
    public InteractablePart CurrentSelectedInteractablePart { get => currentSelectedInteractablePart; set => currentSelectedInteractablePart = value; }

    private float allowedTolerance = 10f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetupInteractableParts();
    }

    public void HddChangeParent()
    {
        hddArmature.transform.SetParent(hddParent2);
        hddCable.transform.SetParent(hddParent2);
    }

    public void OpticChangeParent()
    {
        opticArmature.transform.SetParent(opticParent2);
        opticCable.transform.SetParent(opticParent2);
    }

    public void AddToPlaceComponents(string componentName)
    {
        placedComponents.Add(componentName);
    }
    public void GetParts(string name)
    {
        var part = pcParts.Find(p => p.partsName == name);
        if (part)
        {
            currentSelectedParts = part;
            currentSelectedParts.gameObject.SetActive(true);
        }
    }

    public void GetParts(string name,bool isActive = false)
    {
        var part = pcParts.Find(p => p.partsName == name);
        if (part)
        {
            currentSelectedParts = part;
            currentSelectedParts.gameObject.SetActive(isActive);
        }
    }

    public void DeactivateParts(string name)
    {
        var part = pcParts.Find(p => p.partsName == name);
        if (part)
        {
            currentSelectedParts = part;
            currentSelectedParts.gameObject.SetActive(false);
        }
    }

    public Transform GetPosition(string partName)
    {
        for (int i = 0; i < componentPositions.Count; i++)
        {
            var current = componentPositions[i].gameObject.GetComponent<NameController>();
            if (current.partsName == partName)
            {
                return componentPositions[i];
            }
        }

        Debug.Log($"no transform found on the given name{partName}");
        return null;
    }

    public void ActivatePart(string partName)
    {
        for (int i = 0; i < correctPositionParts.Count; i++)
        {
            var currentPartActivator = correctPositionParts[i].gameObject.GetComponent<ComponentPartActivator>();
            var currentPartName = correctPositionParts[i].gameObject.GetComponent<NameController>();

            if (currentPartName != null && currentPartName.partsName == partName)
            {
                //correctPositionParts[i].gameObject.SetActive(true);
                currentPartActivator.Activatepart();
            }
        }
    }

    public ComponentSlot GetSlot(string name)
    {
        var selectedSlot = componentSlots.Find(s => s.SlotId == name);
        if (selectedSlot != null)
        {
            return selectedSlot;
        }

        Debug.Log($"Component slot with id:{name},cannot be found");

        return null;
    }

    public void SetComponentSlotIndicator(ComponentSlot currentSlot)
    {
        for (int i = 0; i < componentSlots.Count; i++)
        {
            if (componentSlots[i].SlotId == currentSlot.SlotId)
            {
                componentSlots[i].ActivateIndicator(true);
            }
            else
            {
                componentSlots[i].ActivateIndicator(false);
            }
        }
    }

    public void HideCurrentSlotIndicator(ComponentSlot currentSlot)
    {
        for (int i = 0; i < componentSlots.Count; i++)
        {
            if (componentSlots[i].SlotId == currentSlot.SlotId)
            {
                componentSlots[i].ActivateIndicator(false);
            }
            else
            {
                componentSlots[i].ActivateIndicator(false);
            }
        }
    }

    private void SetupInteractableParts()
    {
        for (int i = 0; i < interactableParts.Count; i++)
        {
            interactableParts[i].Setup();
        }
    }

    public void HighlightObject(NameController currentSelected,bool isEnable = true)
    {
        var sceneLoader = SceneLoaderManager.Instance;
        if (sceneLoader.currentGameType == GameType.tutorial)
        {
            var selectedPart = outlineObjects.Find(x => x.gameObject.GetComponent<NameController>().partsName == currentSelected.partsName);
            OutlineObject objectOutline = selectedPart.GetComponent<OutlineObject>();
            selectedPart.gameObject.SetActive(isEnable);
        }
        
        //if (objectOutline != null)
        //{
        //    objectOutline.EnableOutlineObject(isEnable);
        //}
        
    }


    public void HighlightObject(string currentSelected,bool isEnable = true)
    {
        var sceneLoader = SceneLoaderManager.Instance;
        if (sceneLoader.currentGameType == GameType.tutorial)
        {
            var selectedPart = outlineObjects.Find(x => x.gameObject.GetComponent<NameController>().partsName == currentSelected);
            if (selectedPart == null)
            {
                return;
            }
            OutlineObject objectOutline = selectedPart.GetComponent<OutlineObject>();
            selectedPart.gameObject.SetActive(isEnable);
        }
    }

    public bool IsRotationCorrect(Transform givenRotation)
    {
        var givenName = givenRotation.gameObject.GetComponent<PcPart>().partsName;
        var correctPart = correctPositionParts.Find(x => x.gameObject.GetComponent<NameController>().partsName == givenName);
        if (correctPart == null)
        {
            Debug.Log($"correct part {correctPart} is null");
        }
        float rotationDifference = Quaternion.Angle(givenRotation.rotation, correctPart.transform.rotation);
        return rotationDifference <= allowedTolerance;
    }

    
}
