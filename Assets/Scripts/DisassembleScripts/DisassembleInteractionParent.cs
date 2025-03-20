using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisassembleInteractionParent : MonoBehaviour
{
    [Tooltip("This is for parent group")]public GroupParent group;
    [SerializeField] private List<GameObject> groupComponent;
    [SerializeField] private List<DisassembleInteraction> interactionGroups;
    [SerializeField] private List<PartUI> groupsUI;


    public List<PartUI> GroupsUI { get => groupsUI; set => groupsUI = value; }
    public List<DisassembleInteraction> InteractionGroups { get => interactionGroups; }

    void Start()
    {
       
    }

    private void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        interactionGroups = new List<DisassembleInteraction>();
        foreach (var item in groupComponent)
        {
            var interactable = item.GetComponent<DisassembleInteraction>();
            if (interactable != null)
            {
                interactionGroups.Add(interactable);
                DisassembleGameManager.Instance.DisassembleInteractions.Add(interactable);
            }          
        }
    }

    public void DisableObject(string nameToDisable) // removing the 3d object from the list and animating them  getting remove
    {
        var currentIntearactable = interactionGroups.Find(x => x.partsName == nameToDisable);
        if (currentIntearactable.IsPowerSupplyCable)
        {
            currentIntearactable.MoveCable();
        }
        else if (currentIntearactable.IsScrew)
        {
            currentIntearactable.MoveScrews();
        }
        else if (currentIntearactable.isFrontPanel)
        {
            currentIntearactable.MoveFrontPanel();
        }
        else
        {
            currentIntearactable.OnMove();
            //currentIntearactable.DisableObject();
        }
        
       
    }

    public void RemoveParts(string nameToRemove) // removing from list
    {
        var interactionToRemove = interactionGroups.Find(x => x.partsName == nameToRemove);
        interactionGroups.Remove(interactionToRemove);

        var itemToRemove = groupsUI.Find(item => item.CurrentPartName == nameToRemove);
        groupsUI.Remove(itemToRemove);
        if (itemToRemove != null)
        {
            Destroy(itemToRemove.gameObject);
        }
       

       
    }

    
}
