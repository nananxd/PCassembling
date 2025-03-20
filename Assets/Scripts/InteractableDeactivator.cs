using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDeactivator : MonoBehaviour
{
    public GameObject interactableToDeactivate;
    public void DeactivateObject()
    {
        interactableToDeactivate.gameObject.SetActive(false);
    }
}
