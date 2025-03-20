using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentPartsController : MonoBehaviour
{
    [SerializeField] private List<GameObject> componentParts;

    public void ActivatePart()
    {
        for (int i = 0; i < componentParts.Count; i++)
        {
            if (!componentParts[i].activeSelf && componentParts[i] != null)
            {
                componentParts[i].SetActive(true);
                break;
            }
        }
    }
}
